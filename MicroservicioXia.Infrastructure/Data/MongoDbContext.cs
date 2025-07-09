using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MicroservicioXia.Core.Entities;

namespace MicroservicioXia.Infrastructure.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("MicroservicioXia");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<SecurityQuestionTemplate> SecurityQuestionTemplates => _database.GetCollection<SecurityQuestionTemplate>("SecurityQuestionTemplates");
    }
} 