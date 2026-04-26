using System.Threading.Tasks;

namespace KampusRitim.Application.Interfaces
{
    public interface IChatbotService
    {
        Task<string> GetChatResponseAsync(string systemPrompt, string userMessage);
    }
}
