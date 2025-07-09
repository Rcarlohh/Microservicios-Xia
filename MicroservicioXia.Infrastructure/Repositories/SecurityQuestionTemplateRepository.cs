using MongoDB.Driver;
using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;
using MicroservicioXia.Infrastructure.Data;

namespace MicroservicioXia.Infrastructure.Repositories
{
    public class SecurityQuestionTemplateRepository : ISecurityQuestionTemplateRepository
    {
        private readonly IMongoCollection<SecurityQuestionTemplate> _templates;

        public SecurityQuestionTemplateRepository(MongoDbContext context)
        {
            _templates = context.SecurityQuestionTemplates;
        }

        public async Task<IEnumerable<SecurityQuestionTemplate>> GetAllAsync()
        {
            return await _templates.Find(_ => true).ToListAsync();
        }

        public async Task<SecurityQuestionTemplate?> GetByIdAsync(string id)
        {
            return await _templates.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<SecurityQuestionTemplate?> GetByQuestionIdAsync(int questionId)
        {
            return await _templates.Find(t => t.QuestionId == questionId).FirstOrDefaultAsync();
        }

        public async Task<SecurityQuestionTemplate> CreateAsync(SecurityQuestionTemplate template)
        {
            await _templates.InsertOneAsync(template);
            return template;
        }

        public async Task<bool> UpdateAsync(SecurityQuestionTemplate template)
        {
            var result = await _templates.ReplaceOneAsync(t => t.Id == template.Id, template);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _templates.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<SecurityQuestionTemplate>> GetActiveTemplatesAsync()
        {
            return await _templates.Find(t => t.IsActive).ToListAsync();
        }
    }
} 