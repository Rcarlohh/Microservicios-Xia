using MicroservicioXia.Core.DTOs;

namespace MicroservicioXia.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<bool> UpdateUserAsync(string id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(string id);
        Task<PasswordRecoveryDto?> GetPasswordRecoveryQuestionsAsync(string email);
        Task<bool> ResetPasswordAsync(PasswordResetDto passwordResetDto);
    }
} 