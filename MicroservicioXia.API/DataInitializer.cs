using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MicroservicioXia.API
{
    public static class DataInitializer
    {
        public static async Task InitializeSecurityQuestions(ISecurityQuestionTemplateRepository repository)
        {
            var existingQuestions = await repository.GetAllAsync();
            if (existingQuestions.Any())
                return;

            var questions = new List<SecurityQuestionTemplate>
            {
                new SecurityQuestionTemplate
                {
                    QuestionId = 1,
                    Question = "¿Cuál es el nombre de tu primera mascota?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 2,
                    Question = "¿En qué ciudad naciste?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 3,
                    Question = "¿Cuál es el nombre de tu madre?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 4,
                    Question = "¿Cuál es tu color favorito?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 5,
                    Question = "¿Cuál es el nombre de tu escuela primaria?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 6,
                    Question = "¿Cuál es tu comida favorita?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 7,
                    Question = "¿Cuál es el nombre de tu mejor amigo de la infancia?",
                    IsActive = true
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 8,
                    Question = "¿Cuál es tu película favorita?",
                    IsActive = true
                }
            };

            foreach (var question in questions)
            {
                await repository.CreateAsync(question);
            }
        }

        public static async Task InitializeAdminUser(IUserRepository userRepository)
        {
            var existingAdmin = await userRepository.GetByUsernameAsync("admin");
            if (existingAdmin != null)
                return;

            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@xia.com",
                Password = HashPassword("admin123"),
                FirstName = "Administrador",
                LastName = "Sistema",
                Role = "Admin",
                IsActive = true,
                SecurityQuestions = new List<SecurityQuestion>
                {
                    new SecurityQuestion
                    {
                        QuestionId = 1,
                        Question = "¿Cuál es el nombre de tu primera mascota?",
                        Answer = HashPassword("admin")
                    },
                    new SecurityQuestion
                    {
                        QuestionId = 2,
                        Question = "¿En qué ciudad naciste?",
                        Answer = HashPassword("admin")
                    }
                }
            };

            await userRepository.CreateAsync(adminUser);
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 