using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.DTO;
using Task_API.Services;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuth auth) : ControllerBase
    {

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.Username) || string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                return BadRequest(new { Message = "Username and password are required." });
            }

            var result = await auth.CheckLogin(loginDTO);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (registerDTO == null || string.IsNullOrWhiteSpace(registerDTO.Username) || string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                return BadRequest(new { Message = "Invalid registration data." });
            }

            var result = await auth.AddUsers(registerDTO);
            if (result.IsSuccess)
            {
                return Ok(new { Message = result.Message });
            }

            // Use BadRequest (400) instead of Unauthorized (401) for registration failures
            return BadRequest(new { Message = result.Message });
        }

    }
}
