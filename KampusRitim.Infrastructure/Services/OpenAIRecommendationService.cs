using KampusRitim.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat; // Yüklediğin OpenAI Paketi

namespace KampusRitim.Infrastructure.Services
{
    public class OpenAIRecommendationService : IAIRecommendationService
    {
        private readonly string? _apiKey;
        private readonly bool _enabled;
        private readonly bool _mock;
        private readonly string _model;

        public OpenAIRecommendationService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
            _enabled = configuration.GetValue<bool>("OpenAI:Enabled");
            _mock = configuration.GetValue<bool>("OpenAI:Mock");
            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
        }

        public async Task<string> GetJsonResponseAsync(string systemPrompt, string userPrompt)
        {
            if (_mock)
            {
                // Handlers bunu deserialize ediyor. En güvenlisi alanları boş liste ile dönmek.
                // (Kulüp ve etkinlik handler'larının her ikisinde de CuteMessage + list alanı var.)
                if (systemPrompt.Contains("\"Clubs\"", StringComparison.OrdinalIgnoreCase) || systemPrompt.Contains("Clubs", StringComparison.OrdinalIgnoreCase))
                    return "{\"CuteMessage\":\"(Mock) Ücretsiz test modu açık.\",\"Clubs\":[]}";

                if (systemPrompt.Contains("\"Events\"", StringComparison.OrdinalIgnoreCase) || systemPrompt.Contains("Events", StringComparison.OrdinalIgnoreCase))
                    return "{\"CuteMessage\":\"(Mock) Ücretsiz test modu açık.\",\"Events\":[]}";

                return "{}";
            }

            if (!_enabled) return "{}";
            if (string.IsNullOrWhiteSpace(_apiKey)) return "{}";

            try
            {
                ChatClient client = new(model: _model, apiKey: _apiKey);

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt), // Senin Handler'da yazdığın kurallar
                    new UserChatMessage(userPrompt)      // Kullanıcının isteği
                };

                // AI'a "Bana JSON dön" garantisi vermek için bu ayarı yapıyoruz (Önemli!)
                ChatCompletionOptions options = new()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
                };

                ChatCompletion completion = await client.CompleteChatAsync(messages, options);

                return completion.Content[0].Text;
            }
            catch (Exception ex)
            {
                // Hata durumunda boş JSON dönüyoruz ki Handler patlamasın
                Console.WriteLine($"AI Hatası: {ex.Message}");
                return "{}";
            }
        }
    }
}