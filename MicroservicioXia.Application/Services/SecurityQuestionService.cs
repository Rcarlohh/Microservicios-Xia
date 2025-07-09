using System.Security.Cryptography;
using System.Text;
using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;
using MicroservicioXia.Application.Services;

namespace MicroservicioXia.Application.Services
{
    public class SecurityQuestionService : ISecurityQuestionService
    {
        private readonly ISecurityQuestionTemplateRepository _templateRepository;
        private readonly IUserRepository _userRepository;

        public SecurityQuestionService(ISecurityQuestionTemplateRepository templateRepository, IUserRepository userRepository)
        {
            _templateRepository = templateRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<SecurityQuestionTemplate>> GetAllTemplatesAsync()
        {
            return await _templateRepository.GetAllAsync();
        }

        public async Task<SecurityQuestionTemplate?> GetTemplateByIdAsync(string id)
        {
            return await _templateRepository.GetByIdAsync(id);
        }

        public async Task<SecurityQuestionTemplate> CreateTemplateAsync(SecurityQuestionTemplate template)
        {
            return await _templateRepository.CreateAsync(template);
        }

        public async Task<bool> UpdateTemplateAsync(SecurityQuestionTemplate template)
        {
            return await _templateRepository.UpdateAsync(template);
        }

        public async Task<bool> DeleteTemplateAsync(string id)
        {
            return await _templateRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SecurityQuestionTemplate>> GetActiveTemplatesAsync()
        {
            return await _templateRepository.GetActiveTemplatesAsync();
        }

        public async Task<bool> ValidateSecurityQuestionsAsync(string email, List<SecurityQuestion> providedQuestions)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return false;

            if (user.SecurityQuestions.Count != providedQuestions.Count)
                return false;

            // Comparar las respuestas hasheadas
            for (int i = 0; i < user.SecurityQuestions.Count; i++)
            {
                var userQuestion = user.SecurityQuestions[i];
                var providedQuestion = providedQuestions.FirstOrDefault(q => q.QuestionId == userQuestion.QuestionId);

                if (providedQuestion == null || userQuestion.Answer != providedQuestion.Answer)
                    return false;
            }

            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 