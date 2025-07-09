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
        
        // Métodos para administrador
        Task<AdminLoginResponseDto?> AdminLoginAsync(AdminLoginDto loginDto);
        Task<bool> CreateAdminAsync(CreateUserDto createUserDto);
        Task<bool> UpdateUserRoleAsync(string id, string role);
    }
} 