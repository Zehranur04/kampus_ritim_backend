using KampusRitim.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace KampusRitim.Infrastructure.Services
{
    public class OpenAIChatbotService : IChatbotService
    {
        private readonly string? _apiKey;
        private readonly bool _enabled;
        private readonly bool _mock;
        private readonly string _model;

        public OpenAIChatbotService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAI:ApiKey"];
            _enabled = configuration.GetValue<bool>("OpenAI:Enabled");
            _mock = configuration.GetValue<bool>("OpenAI:Mock");
            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
        }

        public async Task<string> GetChatResponseAsync(string systemPrompt, string userMessage)
        {
            if (_mock)
                return BuildMockReply(systemPrompt, userMessage);

            if (!_enabled)
                return "AI şu an kapalı. Açmak için OpenAI:Enabled=true ayarla (veya ücretsiz test için OpenAI:Mock=true).";

            if (string.IsNullOrWhiteSpace(_apiKey))
                return "OpenAI API key is missing. Set OpenAI:ApiKey via environment variables or user-secrets.";

            try
            {
                ChatClient client = new(model: _model, apiKey: _apiKey);

                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userMessage)
                };

                ChatCompletion completion = await client.CompleteChatAsync(messages);
                return completion.Content.Count > 0 ? completion.Content[0].Text : string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AI Hatası: {ex.Message}");
                return "Şu anda cevap üretilemedi. Lütfen tekrar deneyin.";
            }
        }

        private static string BuildMockReply(string systemPrompt, string userMessage)
        {
            // Basit, deterministik mock: OpenAI çağrısı yok.
            // Ama prompt içindeki "Bölüm:" bilgisini okuyup kişiselleştirmeyi test etmemizi sağlar.

            string? department = ExtractAfterLabel(systemPrompt, "Bölüm:");
            string? faculty = ExtractAfterLabel(systemPrompt, "Fakülte:");
            string? classLevel = ExtractAfterLabel(systemPrompt, "Sınıf:");

            var normalized = (userMessage ?? string.Empty).ToLowerInvariant();

            if (normalized.Contains("bölüm") && normalized.Contains("benim"))
            {
                if (!string.IsNullOrWhiteSpace(department))
                    return $"(Mock) Bölümün: {department}.";
                return "(Mock) Bölüm bilgine erişemiyorum. Profilinden bölüm ekleyebilir ya da mesajında yazabilirsin.";
            }

            var headerParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(department)) headerParts.Add($"Bölüm: {department}");
            if (!string.IsNullOrWhiteSpace(classLevel)) headerParts.Add($"Sınıf: {classLevel}");
            if (!string.IsNullOrWhiteSpace(faculty)) headerParts.Add($"Fakülte: {faculty}");

            var header = headerParts.Count > 0
                ? $"(Mock) Profilin: {string.Join(", ", headerParts)}\n\n"
                : "(Mock) Profil context yok.\n\n";

            // Çok basit öneri kuralları
            if (!string.IsNullOrWhiteSpace(department) && department.ToLowerInvariant().Contains("yazılım"))
            {
                return header +
                       "Yazılım Mühendisliği için kulüp önerileri:\n" +
                       "- Yazılım Kulübü (projeler, hackathon)\n" +
                       "- GDG on Campus (tech talk’lar)\n" +
                       "- Girişimcilik Kulübü (ürün fikri, startup)";
            }

            if (!string.IsNullOrWhiteSpace(department) && department.ToLowerInvariant().Contains("bilgisayar"))
            {
                return header +
                       "Bilgisayar Mühendisliği için kulüp önerileri:\n" +
                       "- Yapay Zeka Kulübü\n" +
                       "- Siber Güvenlik Topluluğu\n" +
                       "- Yazılım Kulübü";
            }

            return header + "Sana yardımcı olmak için buradayım. Kulüp/etkinlik isteğini biraz daha net yazarsan mock modda da yönlendirebilirim.";
        }

        private static string? ExtractAfterLabel(string text, string label)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var idx = text.IndexOf(label, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) return null;
            idx += label.Length;

            // label sonrası satır sonuna kadar al
            var remaining = text.Substring(idx).TrimStart();
            if (remaining.Length == 0) return null;
            var endIdx = remaining.IndexOfAny(new[] { '\r', '\n' });
            var value = (endIdx >= 0 ? remaining.Substring(0, endIdx) : remaining).Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
