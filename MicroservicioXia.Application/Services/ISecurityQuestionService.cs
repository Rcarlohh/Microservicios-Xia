using MicroservicioXia.Core.Entities;

namespace MicroservicioXia.Application.Services
{
    public interface ISecurityQuestionService
    {
        Task<IEnumerable<SecurityQuestionTemplate>> GetAllTemplatesAsync();
        Task<SecurityQuestionTemplate?> GetTemplateByIdAsync(string id);
        Task<SecurityQuestionTemplate> CreateTemplateAsync(SecurityQuestionTemplate template);
        Task<bool> UpdateTemplateAsync(SecurityQuestionTemplate template);
        Task<bool> DeleteTemplateAsync(string id);
        Task<IEnumerable<SecurityQuestionTemplate>> GetActiveTemplatesAsync();
        Task<bool> ValidateSecurityQuestionsAsync(string email, List<SecurityQuestion> providedQuestions);
    }
} 