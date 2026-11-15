namespace Task_API.DTO
{
    public class LoginResponseDTO
    {
        public string? Username { get; set; }
        public string? Roles { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }

    }
}
