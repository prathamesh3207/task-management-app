using Task_API.DTO;

namespace Task_API.Services
{
    public interface IAuth
    {
        Task<LoginResponseDTO> CheckLogin(LoginDTO loginDTO);
        Task<(bool IsSuccess, string Message)> AddUsers(RegisterDTO registerDTO);
    }
}
