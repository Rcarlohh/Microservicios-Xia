using System.Security.Cryptography;
using System.Text;
using MicroservicioXia.Core.DTOs;
using MicroservicioXia.Core.Entities;
using MicroservicioXia.Core.Interfaces;
using MicroservicioXia.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MicroservicioXia.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityQuestionService _securityQuestionService;
        private readonly string _jwtSecret = "TuClaveSecretaSuperSeguraParaJWT2024"; // En producción usar configuración

        public UserService(IUserRepository userRepository, ISecurityQuestionService securityQuestionService)
        {
            _userRepository = userRepository;
            _securityQuestionService = securityQuestionService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.ExistsAsync(createUserDto.Email, createUserDto.Username))
            {
                throw new InvalidOperationException("El usuario ya existe con ese email o username.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Password = HashPassword(createUserDto.Password),
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Role = "User", // Usuario normal por defecto
                SecurityQuestions = createUserDto.SecurityQuestions.Select(q => new SecurityQuestion
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Answer = HashPassword(q.Answer.ToLower())
                }).ToList()
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return MapToDto(createdUser);
        }

        public async Task<bool> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<PasswordRecoveryDto?> GetPasswordRecoveryQuestionsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            return new PasswordRecoveryDto
            {
                Email = user.Email,
                SecurityQuestions = user.SecurityQuestions.Select(q => new SecurityQuestionDto
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Answer = string.Empty // No enviamos la respuesta por seguridad
                }).ToList()
            };
        }

        public async Task<bool> ResetPasswordAsync(PasswordResetDto passwordResetDto)
        {
            var user = await _userRepository.GetByEmailAsync(passwordResetDto.Email);
            if (user == null)
                return false;

            // Validar las respuestas de seguridad
            var providedQuestions = passwordResetDto.SecurityQuestions.Select(q => new SecurityQuestion
            {
                QuestionId = q.QuestionId,
                Question = q.Question,
                Answer = HashPassword(q.Answer.ToLower())
            }).ToList();

            if (!await _securityQuestionService.ValidateSecurityQuestionsAsync(user.Email, providedQuestions))
                return false;

            // Actualizar la contraseña
            user.Password = HashPassword(passwordResetDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(user);
        }

        // Métodos para administrador
        public async Task<AdminLoginResponseDto?> AdminLoginAsync(AdminLoginDto loginDto)
        {
            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || user.Role != "Admin" || !user.IsActive)
                return null;

            if (user.Password != HashPassword(loginDto.Password))
                return null;

            var token = GenerateJwtToken(user);
            return new AdminLoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<bool> CreateAdminAsync(CreateUserDto createUserDto)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.ExistsAsync(createUserDto.Email, createUserDto.Username))
            {
                throw new InvalidOperationException("El usuario ya existe con ese email o username.");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                Password = HashPassword(createUserDto.Password),
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Role = "Admin",
                SecurityQuestions = createUserDto.SecurityQuestions.Select(q => new SecurityQuestion
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Answer = HashPassword(q.Answer.ToLower())
                }).ToList()
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return createdUser != null;
        }

        public async Task<bool> UpdateUserRoleAsync(string id, string role)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            user.Role = role;
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                SecurityQuestions = user.SecurityQuestions.Select(q => new SecurityQuestionDto
                {
                    QuestionId = q.QuestionId,
                    Question = q.Question,
                    Answer = string.Empty // No enviamos las respuestas por seguridad
                }).ToList()
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
} 