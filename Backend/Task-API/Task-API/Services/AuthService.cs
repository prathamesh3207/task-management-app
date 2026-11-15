using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Task_API.Data;
using Task_API.DTO;
using Task_API.Models;

namespace Task_API.Services
{
    public class AuthService : IAuth
    {
        private readonly TaskDbContext context;

        public AuthService(TaskDbContext context)
        {
            this.context = context;
        }

        public async Task<(bool IsSuccess, string Message)> AddUsers(RegisterDTO registerDTO)
        {
            // 1. Validate that Password and ConfirmPassword match before touching the DB
            if (registerDTO.Password != registerDTO.ConfirmPassword)
            {
                return (false, "Passwords do not match.");
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var passwordHasher = new PasswordHasher<Users>();

                var newUser = new Users
                {
                    Username = registerDTO.Username,
                    Roles = registerDTO.Roles
                };

                // 2. Hash the plain-text password and store it
                newUser.PasswordHash = passwordHasher.HashPassword(newUser, registerDTO.Password);

                context.users.Add(newUser);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "User Added Successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error(ex, "Error occurred while adding user {Username}", registerDTO.Username);
                return (false, ex.Message);
            }
        }


        public async Task<LoginResponseDTO> CheckLogin(LoginDTO loginDTO)
        {
            try
            {
                var user = await context.users.FirstOrDefaultAsync(x => x.Username == loginDTO.Username);
                if (user == null)
                {
                    return new LoginResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password."
                    };
                }

                var passwordHasher = new PasswordHasher<Users>();
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return new LoginResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password."
                    };
                }

                if (result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, loginDTO.Password);
                    await context.SaveChangesAsync();
                }

                return new LoginResponseDTO
                {
                    IsSuccess = true,
                    Message = "Login Successful",
                    Username = user.Username,
                    Roles = user.Roles
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during login attempt for {Username}", loginDTO.Username);
                return new LoginResponseDTO
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }


    }
}
