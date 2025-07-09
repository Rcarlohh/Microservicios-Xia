using MongoDB.Driver;
using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;
using MicroservicioXia.Infrastructure.Data;

namespace MicroservicioXia.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Users;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsAsync(string email, string username)
        {
            var filter = Builders<User>.Filter.Or(
                Builders<User>.Filter.Eq(u => u.Email, email),
                Builders<User>.Filter.Eq(u => u.Username, username)
            );
            return await _users.Find(filter).AnyAsync();
        }
    }
} 