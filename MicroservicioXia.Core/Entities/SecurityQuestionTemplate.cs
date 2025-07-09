using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroservicioXia.Core.Entities
{
    public class SecurityQuestionTemplate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        
        [BsonElement("questionId")]
        public int QuestionId { get; set; }
        
        [BsonElement("question")]
        public string Question { get; set; } = string.Empty;
        
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 