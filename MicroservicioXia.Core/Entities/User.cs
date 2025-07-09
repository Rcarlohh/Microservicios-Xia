using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroservicioXia.Core.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;
        
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
        
        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;
        
        [BsonElement("firstName")]
        public string FirstName { get; set; } = string.Empty;
        
        [BsonElement("lastName")]
        public string LastName { get; set; } = string.Empty;
        
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("securityQuestions")]
        public List<SecurityQuestion> SecurityQuestions { get; set; } = new List<SecurityQuestion>();
    }

    public class SecurityQuestion
    {
        [BsonElement("questionId")]
        public int QuestionId { get; set; }
        
        [BsonElement("question")]
        public string Question { get; set; } = string.Empty;
        
        [BsonElement("answer")]
        public string Answer { get; set; } = string.Empty;
    }
} 