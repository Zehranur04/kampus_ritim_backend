using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Features.Dtos; // RecommendedEventDto burada
using KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent
{
    public class GetRecommendedEventHandler : IRequestHandler<GetRecommendedEventRequest, GetRecommendedEventResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IAIRecommendationService _aiService;

        public GetRecommendedEventHandler(IAppDbContext context, IAIRecommendationService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public async Task<GetRecommendedEventResponse> Handle(GetRecommendedEventRequest request, CancellationToken cancellationToken)
        {
            // 1. Gelecek Etkinlikleri Çek
            var upcomingEvents = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Speaker)
                .Include(e => e.Club)
                .Where(e => e.Time >= DateTime.UtcNow)
                .OrderBy(e => e.Time)
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.Description,
                    e.Time,
                    e.Location,
                    e.Quota,
                    CategoryName = e.Category != null ? e.Category.Name : "Genel",
                    SpeakerName = e.Speaker != null ? e.Speaker.Name : "Belirtilmemiş",
                    ClubName = e.Club != null ? e.Club.Name : ""
                })
                .ToListAsync(cancellationToken);

            if (!upcomingEvents.Any())
            {
                return new GetRecommendedEventResponse
                {
                    CuteMessage = "Şu an güncel bir etkinlik yok, ama gözün bizde olsun! 👀",
                    Events = new List<RecommendedEventDto>()
                };
            }

            // Shuffle the candidate list so generic prompts don't always return the first 2 events.
            upcomingEvents = upcomingEvents
                .OrderBy(_ => Random.Shared.Next())
                .Take(120)
                .ToList();

            var validIds = upcomingEvents.Select(e => e.Id).ToHashSet();

            string eventsJson = JsonSerializer.Serialize(upcomingEvents);

            // 2. Prompt Hazırla
            string systemPrompt = $$"""
Sen kampüs etkinlik organizatörüsün.

GÖREV: Kullanıcının moduna/isteğine göre sadece aşağıdaki listeden en uygun 2 etkinliği seç.

MEVCUT EVENTS (tek kaynak):
{{eventsJson}}

KURALLAR:
1) Sadece geçerli bir JSON döndür. Markdown veya ekstra metin yok.
2) "Events" listesinde TAM 2 farklı etkinlik olsun.
3) Id, Title, Time, Location, Quota, CategoryName, SpeakerName değerlerini listeden aynen al. Uydurma Id/başlık YASAK.
4) Description alanına etkinliğin açıklamasını değil, senin neden önerdiğini yaz.

İSTENEN FORMAT:
{
    "CuteMessage": "...",
    "Events": [
        { "Id": 1, "Title": "...", "Description": "...", "Time": "2025-12-23T18:00:00Z", "Location": "...", "Quota": 50, "CategoryName": "...", "SpeakerName": "..." },
        { "Id": 2, "Title": "...", "Description": "...", "Time": "2025-12-24T18:00:00Z", "Location": "...", "Quota": 50, "CategoryName": "...", "SpeakerName": "..." }
    ]
}
""";

            // 3. OpenAI'ya Gönder
            var jsonResponse = await _aiService.GetJsonResponseAsync(systemPrompt, request.UserDescription);

            // 4. Cevabı İşle
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                var result = JsonSerializer.Deserialize<GetRecommendedEventResponse>(jsonResponse, options);

                if (result == null || result.Events == null)
                {
                    return new GetRecommendedEventResponse
                    {
                        CuteMessage = "Şu an sana uygun etkinlikleri eşleştiremedim. Biraz daha detay verir misin? 😊",
                        Events = new List<RecommendedEventDto>()
                    };
                }

                // Validate against DB-backed candidate list.
                result.Events = result.Events
                    .Where(e => e != null && validIds.Contains(e.Id))
                    .GroupBy(e => e.Id)
                    .Select(g => g.First())
                    .Take(2)
                    .ToList();

                if (result.Events.Count == 0)
                {
                    return new GetRecommendedEventResponse
                    {
                        CuteMessage = "Şu an sana uygun etkinlikleri eşleştiremedim. İstersen 'konser', 'teknoloji', 'atölye' gibi bir ipucu ver 😊",
                        Events = new List<RecommendedEventDto>()
                    };
                }

                if (string.IsNullOrWhiteSpace(result.CuteMessage))
                    result.CuteMessage = "Sana uygun 2 etkinlik seçtim!";

                return result;
            }
            catch
            {
                return new GetRecommendedEventResponse
                {
                    CuteMessage = "Verileri toplarken kafam karıştı ama aşağıdaki takvime bakabilirsin! 📅",
                    Events = new List<RecommendedEventDto>()
                };
            }
        }
    }
}