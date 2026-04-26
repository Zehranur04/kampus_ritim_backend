using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;
        private readonly IChatDataQueryService _chatDataQueryService;
        private readonly IAIRecommendationService _aiRecommendationService;
        private readonly IAppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProfileRepository _profileRepository;

        public ChatController(
            IChatbotService chatbotService,
            IChatDataQueryService chatDataQueryService,
            IAIRecommendationService aiRecommendationService,
            IAppDbContext appDbContext,
            IConfiguration configuration,
            ICurrentUserService currentUserService,
            IProfileRepository profileRepository)
        {
            _chatbotService = chatbotService;
            _chatDataQueryService = chatDataQueryService;
            _aiRecommendationService = aiRecommendationService;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _currentUserService = currentUserService;
            _profileRepository = profileRepository;
        }

        public sealed class ChatRequest
        {
            public string Message { get; set; } = string.Empty;

            // JWT kullanmadan test edebilmek için opsiyonel context alanları
            // (Giriş yoksa veya profilde eksikse prompt'a eklenir.)
            public string? Faculty { get; set; }
            public string? Department { get; set; }
            public string? ClassLevel { get; set; }
        }

        public sealed class ChatResponse
        {
            public string Reply { get; set; } = string.Empty;
        }

        public sealed class ChatStatusResponse
        {
            public bool AiEnabled { get; set; }
            public bool AiMock { get; set; }
            public string Model { get; set; } = string.Empty;
            public string Mode { get; set; } = string.Empty;
            public bool IsAuthenticated { get; set; }
            public bool HasProfile { get; set; }
            public bool HasDepartment { get; set; }
        }

        // GET: api/chat/status
        // OpenAI çağrısı yapmaz. Sadece "AI açık mı? kullanıcı giriş yapmış mı? profilde bölüm var mı?" döner.
        [HttpGet("status")]
        public async Task<ActionResult<ChatStatusResponse>> Status()
        {
            var aiEnabled = _configuration.GetValue<bool>("OpenAI:Enabled");
            var aiMock = _configuration.GetValue<bool>("OpenAI:Mock");
            var model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";
            var mode = aiMock ? "mock" : (aiEnabled ? "real" : "off");
            var userId = _currentUserService.UserId;

            if (!userId.HasValue)
            {
                return Ok(new ChatStatusResponse
                {
                    AiEnabled = aiEnabled,
                    AiMock = aiMock,
                    Model = model,
                    Mode = mode,
                    IsAuthenticated = false,
                    HasProfile = false,
                    HasDepartment = false
                });
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId.Value);
            return Ok(new ChatStatusResponse
            {
                AiEnabled = aiEnabled,
                AiMock = aiMock,
                Model = model,
                Mode = mode,
                IsAuthenticated = true,
                HasProfile = profile != null,
                HasDepartment = !string.IsNullOrWhiteSpace(profile?.User?.Department)
            });
        }

        // POST: api/chat
        [HttpPost]
        public async Task<ActionResult<ChatResponse>> Chat([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message is required");

            // 1) Try DB-backed answers first (events/rooms/reservations). This does NOT require OpenAI.
            var userId = _currentUserService.UserId;
            var dataAnswer = await _chatDataQueryService.TryResolveAsync(request.Message, userId, HttpContext.RequestAborted);
            if (!string.IsNullOrWhiteSpace(dataAnswer))
                return Ok(new ChatResponse { Reply = dataAnswer });

            var aiEnabled = _configuration.GetValue<bool>("OpenAI:Enabled");
            var aiMock = _configuration.GetValue<bool>("OpenAI:Mock");
            if (!aiEnabled && !aiMock)
                return StatusCode(503, "AI şu an kapalı (OpenAI:Enabled=false). Mock test için OpenAI:Mock=true yapabilirsin.");

            var baseSystemPrompt = "Sen KampusRitim uygulaması için yardımcı bir asistansın. Kısa, net ve güvenli cevap ver. Gereksiz kişisel veri isteme.";

            string? profileContext = null;
            if (userId.HasValue)
            {
                var profile = await _profileRepository.GetByUserIdAsync(userId.Value);
                if (profile != null)
                {
                    var parts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(profile.User?.Faculty)) parts.Add($"Fakülte: {profile.User.Faculty}");
                    if (!string.IsNullOrWhiteSpace(profile.User?.Department)) parts.Add($"Bölüm: {profile.User.Department}");
                    if (profile.ClassLevel.HasValue) parts.Add($"Sınıf: {profile.ClassLevel}");

                    if (parts.Count > 0)
                        profileContext = string.Join(", ", parts);
                }
            }

            // JWT yoksa (veya profilde bölüm/fakülte yoksa) request içinden gelen context'i kullan
            if (profileContext == null)
            {
                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(request.Faculty)) parts.Add($"Fakülte: {request.Faculty}");
                if (!string.IsNullOrWhiteSpace(request.Department)) parts.Add($"Bölüm: {request.Department}");
                if (!string.IsNullOrWhiteSpace(request.ClassLevel)) parts.Add($"Sınıf: {request.ClassLevel}");

                if (parts.Count > 0)
                    profileContext = string.Join(", ", parts);
            }

            var systemPrompt = profileContext == null
                ? baseSystemPrompt + " Kullanıcının bölüm/fakülte bilgisi yoksa önce kısa bir soru sorarak netleştir."
                : baseSystemPrompt + $" Kullanıcı profili (güvenilir): {profileContext}. Bu bilgiyi tekrar sormadan cevaplarını kişiselleştir.";

            // If this is a club recommendation request, use AI but constrain it to DB clubs.
            if (LooksLikeClubRecommendationQuery(request.Message))
            {
                var constrained = await GetAiConstrainedClubRecommendationAsync(
                    userMessage: request.Message,
                    profileContext: profileContext,
                    cancellationToken: HttpContext.RequestAborted);

                if (!string.IsNullOrWhiteSpace(constrained))
                    return Ok(new ChatResponse { Reply = constrained });
            }

            // If this is an event recommendation request, use AI but constrain it to DB events.
            if (LooksLikeEventRecommendationQuery(request.Message))
            {
                var constrained = await GetAiConstrainedEventRecommendationAsync(
                    userMessage: request.Message,
                    profileContext: profileContext,
                    cancellationToken: HttpContext.RequestAborted);

                if (!string.IsNullOrWhiteSpace(constrained))
                    return Ok(new ChatResponse { Reply = constrained });
            }

            var reply = await _chatbotService.GetChatResponseAsync(systemPrompt, request.Message);

            return Ok(new ChatResponse { Reply = reply });
        }

        private sealed class AiClubPick
        {
            public int ClubId { get; set; }
            public string Reply { get; set; } = string.Empty;
        }

        private static bool LooksLikeClubRecommendationQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            if (!(t.Contains("kulüp") || t.Contains("kulup") || t.Contains("topluluk") || t.Contains("club")))
                return false;

            return t.Contains("öner") || t.Contains("oner") || t.Contains("tavsiye") || t.Contains("hangi") || t.Contains("katıl") || t.Contains("katil");
        }

        private static bool LooksLikeEventRecommendationQuery(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var t = text.ToLowerInvariant();
            if (!(t.Contains("etkinlik") || t.Contains("event") || t.Contains("konser") || t.Contains("atölye") || t.Contains("atolye") || t.Contains("semin") || t.Contains("konfer")))
                return false;

            return t.Contains("öner") || t.Contains("oner") || t.Contains("tavsiye") || t.Contains("recommend");
        }

        private sealed class AiEventPick
        {
            public List<int> EventIds { get; set; } = new();
            public string Reply { get; set; } = string.Empty;
        }

        private async Task<string?> GetAiConstrainedEventRecommendationAsync(string userMessage, string? profileContext, CancellationToken cancellationToken)
        {
            var events = await _appDbContext.Events
                .AsNoTracking()
                .Include(e => e.Category)
                .Include(e => e.Speaker)
                .Include(e => e.Club)
                .Where(e => e.Time >= DateTime.UtcNow)
                .OrderBy(e => e.Time)
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.Time,
                    e.Location,
                    e.Quota,
                    CategoryName = e.Category != null ? e.Category.Name : "Genel",
                    SpeakerName = e.Speaker != null ? e.Speaker.Name : "Belirtilmemiş",
                    ClubName = e.Club != null ? e.Club.Name : ""
                })
                .Take(120)
                .ToListAsync(cancellationToken);

            if (events.Count == 0)
                return "Şu an yaklaşan etkinlik görünmüyor.";

            // Shuffle so generic prompts don't always pick the first ones.
            events = events.OrderBy(_ => Random.Shared.Next()).Take(80).ToList();

            var allowedIds = events.Select(e => e.Id).ToHashSet();
            var eventsJson = JsonSerializer.Serialize(events);

            var systemPrompt = $$"""
Sen KampusRitim için etkinlik öneren bir asistansın.

GÖREV: Kullanıcının mesajına göre AŞAĞIDAKİ ETKİNLİK LİSTESİNDEN tam 2 farklı etkinlik seç.

KULLANICI PROFİLİ (varsa güvenilir): {{profileContext ?? "(yok)"}}

MEVCUT EVENTS (tek kaynak):
{{eventsJson}}

KURALLAR:
1) SADECE aşağıdaki JSON objesini döndür. Markdown veya ekstra metin yok.
2) EventIds sadece listede bulunan Id'lerden oluşmalı. Uydurma Id YASAK.
3) Reply Türkçe, 1-3 cümle: neden bu 2 etkinlik uygun?

İSTENEN JSON:
{
  "EventIds": [1, 2],
  "Reply": "..."
}
""";

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            AiEventPick? pick = null;
            var json = await _aiRecommendationService.GetJsonResponseAsync(systemPrompt, userMessage);
            try { pick = JsonSerializer.Deserialize<AiEventPick>(json, options); } catch { pick = null; }

            if (pick == null || pick.EventIds == null || pick.EventIds.Count == 0 || pick.EventIds.Any(id => !allowedIds.Contains(id)) || string.IsNullOrWhiteSpace(pick.Reply))
            {
                var strictPrompt = systemPrompt + "\n\nHATA: Önceki cevabın geçersizdi. EventIds sadece listeden olmalı ve Reply boş olamaz.";
                var json2 = await _aiRecommendationService.GetJsonResponseAsync(strictPrompt, userMessage);
                try { pick = JsonSerializer.Deserialize<AiEventPick>(json2, options); } catch { pick = null; }
            }

            if (pick == null || pick.EventIds == null || pick.EventIds.Count == 0 || pick.EventIds.Any(id => !allowedIds.Contains(id)) || string.IsNullOrWhiteSpace(pick.Reply))
                return null;

            var chosenIds = pick.EventIds
                .Where(allowedIds.Contains)
                .Distinct()
                .Take(2)
                .ToList();

            if (chosenIds.Count == 0)
                return null;

            var chosen = events
                .Where(e => chosenIds.Contains(e.Id))
                .OrderBy(e => e.Time)
                .ToList();

            var lines = chosen.Select(e => $"- {e.Title} | {e.Time:dd.MM HH:mm} | {e.Location}");
            return $"{pick.Reply}\n\nÖnerilen etkinlikler:\n" + string.Join("\n", lines);
        }

        private async Task<string?> GetAiConstrainedClubRecommendationAsync(string userMessage, string? profileContext, CancellationToken cancellationToken)
        {
            // Pull clubs with Category for richer matching.
            var clubs = await _appDbContext.Clubs
                .AsNoTracking()
                .Include(c => c.Category)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    CategoryName = c.Category != null ? c.Category.Name : ""
                })
                .OrderBy(c => c.Id)
                .Take(150)
                .ToListAsync(cancellationToken);

            if (clubs.Count == 0)
                return "Şu an sistemde kayıtlı kulüp görünmüyor.";

            var allowedIds = clubs.Select(c => c.Id).ToHashSet();
            var clubsJson = JsonSerializer.Serialize(clubs);

            var systemPrompt = $$"""
Sen KampusRitim için kulüp öneren bir asistansın.

GÖREV: Kullanıcının mesajına göre AŞAĞIDAKİ KULÜP LİSTESİNDEN tam 1 kulüp seç.

KULLANICI PROFİLİ (varsa güvenilir): {{profileContext ?? "(yok)"}}

MEVCUT KULÜPLER (tek kaynak):
{{clubsJson}}

KURALLAR:
1) SADECE aşağıdaki JSON objesini döndür. Markdown veya ekstra metin yok.
2) ClubId alanı yukarıdaki listede bulunan bir Id olmak zorunda. Uydurma Id veya uydurma kulüp adı YASAK.
3) Reply alanı Türkçe olsun; 1-3 cümlede neden uygun olduğunu söyle.
4) Kullanıcı mesajı çok belirsizse Reply içinde 1 kısa netleştirme sorusu sorabilirsin ama yine de en yakın kulübü seç.

İSTENEN JSON:
{
  "ClubId": 123,
  "Reply": "..."
}
""";

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            AiClubPick? pick = null;
            var json = await _aiRecommendationService.GetJsonResponseAsync(systemPrompt, userMessage);
            try { pick = JsonSerializer.Deserialize<AiClubPick>(json, options); } catch { pick = null; }

            // One retry with stricter guardrails if invalid.
            if (pick == null || pick.ClubId <= 0 || !allowedIds.Contains(pick.ClubId) || string.IsNullOrWhiteSpace(pick.Reply))
            {
                var strictPrompt = systemPrompt + "\n\nHATA: Önceki cevabın geçersizdi. ClubId MUTLAKA listeden olacak ve Reply boş olmayacak.";
                var json2 = await _aiRecommendationService.GetJsonResponseAsync(strictPrompt, userMessage);
                try { pick = JsonSerializer.Deserialize<AiClubPick>(json2, options); } catch { pick = null; }
            }

            if (pick == null || pick.ClubId <= 0 || !allowedIds.Contains(pick.ClubId) || string.IsNullOrWhiteSpace(pick.Reply))
                return null;

            var selected = clubs.First(c => c.Id == pick.ClubId);
            var cat = string.IsNullOrWhiteSpace(selected.CategoryName) ? "" : $" (Kategori: {selected.CategoryName})";
            return $"{pick.Reply}\n\nÖnerilen kulüp: {selected.Name}{cat}";
        }
    }
}
