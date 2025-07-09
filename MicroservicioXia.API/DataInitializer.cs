using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;

namespace MicroservicioXia.API
{
    public static class DataInitializer
    {
        public static async Task InitializeSecurityQuestions(ISecurityQuestionTemplateRepository repository)
        {
            var existingTemplates = await repository.GetAllAsync();
            if (existingTemplates.Any())
                return; // Ya existen plantillas, no inicializar

            var defaultQuestions = new List<SecurityQuestionTemplate>
            {
                new SecurityQuestionTemplate
                {
                    QuestionId = 1,
                    Question = "¿Cuál es el nombre de tu primera mascota?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 2,
                    Question = "¿En qué ciudad naciste?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 3,
                    Question = "¿Cuál es el nombre de tu madre?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 4,
                    Question = "¿Cuál es tu color favorito?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 5,
                    Question = "¿Cuál es el nombre de tu escuela primaria?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 6,
                    Question = "¿Cuál es tu comida favorita?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 7,
                    Question = "¿Cuál es el nombre de tu mejor amigo de la infancia?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 8,
                    Question = "¿Cuál es tu película favorita?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 9,
                    Question = "¿Cuál es el nombre de tu primer profesor?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new SecurityQuestionTemplate
                {
                    QuestionId = 10,
                    Question = "¿Cuál es tu hobby favorito?",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            foreach (var question in defaultQuestions)
            {
                await repository.CreateAsync(question);
            }
        }
    }
} 