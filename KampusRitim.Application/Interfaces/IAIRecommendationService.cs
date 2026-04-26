using KampusRitim.Domain.Entity;
using System.Threading.Tasks;

namespace KampusRitim.Application.Interfaces
{
    public interface IAIRecommendationService
    {
        // SystemPrompt: Yapay zekaya rolünü ve kuralları anlattığımız metin
        // UserPrompt: Kullanıcının girdiği "canım sıkıldı" gibi metin
        Task<string> GetJsonResponseAsync(string systemPrompt, string userPrompt);
    }
}