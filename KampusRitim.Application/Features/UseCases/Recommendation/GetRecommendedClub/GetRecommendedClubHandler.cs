using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Features.Dtos; // RecommendedClubDto burada
using KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub
{
    public class GetRecommendedClubHandler : IRequestHandler<GetRecommendedClubRequest, GetRecommendedClubResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IAIRecommendationService _aiService; // İsim değiştirdik (Generic oldu)

        public GetRecommendedClubHandler(IAppDbContext context, IAIRecommendationService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        public async Task<GetRecommendedClubResponse> Handle(GetRecommendedClubRequest request, CancellationToken cancellationToken)
        {
            // 1. Veriyi Çek
            var allClubs = await _context.Clubs
                .Include(c => c.Category)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    CategoryName = c.Category != null ? c.Category.Name : "Genel"
                })
                .ToListAsync(cancellationToken);

            string clubsJson = JsonSerializer.Serialize(allClubs);

            // 2. Prompt Hazırla (OpenAI için optimize edildi)
            string systemPrompt = $@"
                Sen bir üniversite kulüp danışmanısın.
                
                GÖREV: Aşağıdaki kulüp listesinden, kullanıcının mesajına en uygun 2 kulübü seç.
                
                MEVCUT KULÜPLER: 
                {clubsJson}

                KURALLAR:
                1. Cevabın kesinlikle geçerli bir JSON formatında olmalı.
                2. 'CuteMessage' alanına öğrenciye hitap eden samimi, emojili kısa bir mesaj yaz.
                3. Seçilen kulüplerin Id, Name, Description ve CategoryName bilgilerini 'Clubs' listesine ekle.
                
                İSTENEN JSON FORMATI:
                {{
                    ""CuteMessage"": ""Harika ilgi alanların var! Senin için şunları buldum..."",
                    ""Clubs"": [
                        {{ ""Id"": 1, ""Name"": ""..."", ""Description"": ""..."", ""CategoryName"": ""..."" }}
                    ]
                }}
            ";

            // 3. OpenAI'ya Gönder
            var jsonResponse = await _aiService.GetJsonResponseAsync(systemPrompt, request.UserDescription);

            // 4. Cevabı İşle
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            try
            {
                var result = JsonSerializer.Deserialize<GetRecommendedClubResponse>(jsonResponse, options);

                // Eğer AI boş dönerse veya parse edemezse boş liste dön
                if (result == null || result.Clubs == null)
                {
                    return new GetRecommendedClubResponse
                    {
                        CuteMessage = "Üzgünüm, şu an sana uygun bir kulüp bulamadım ama arayışa devam!",
                        Clubs = new List<RecommendedClubDto>()
                    };
                }

                // Safety: AI bazen listede olmayan kulüp/Id uydurabilir. DB'de olanlarla sınırla.
                var validIds = allClubs.Select(c => c.Id).ToHashSet();
                result.Clubs = result.Clubs
                    .Where(c => c != null && validIds.Contains(c.Id))
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();

                if (result.Clubs.Count == 0)
                {
                    return new GetRecommendedClubResponse
                    {
                        CuteMessage = "Üzgünüm, şu an sana uygun bir kulüp eşleştiremedim. İstersen ilgi alanını biraz daha detaylandır 😊",
                        Clubs = new List<RecommendedClubDto>()
                    };
                }

                if (string.IsNullOrWhiteSpace(result.CuteMessage))
                    result.CuteMessage = "Senin için kulüp önerilerim hazır!";

                return result;
            }
            catch
            {
                return new GetRecommendedClubResponse
                {
                    CuteMessage = "Bağlantıda minik bir sorun oldu, tekrar dener misin? 🤖",
                    Clubs = new List<RecommendedClubDto>()
                };
            }
        }
    }
}