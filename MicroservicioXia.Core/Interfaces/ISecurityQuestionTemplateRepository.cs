using MicroservicioXia.Core.Entities;

namespace MicroservicioXia.Core.Interfaces
{
    public interface ISecurityQuestionTemplateRepository
    {
        Task<IEnumerable<SecurityQuestionTemplate>> GetAllAsync();
        Task<SecurityQuestionTemplate?> GetByIdAsync(string id);
        Task<SecurityQuestionTemplate?> GetByQuestionIdAsync(int questionId);
        Task<SecurityQuestionTemplate> CreateAsync(SecurityQuestionTemplate template);
        Task<bool> UpdateAsync(SecurityQuestionTemplate template);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<SecurityQuestionTemplate>> GetActiveTemplatesAsync();
    }
} 