using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myfirstrestapi.Dto;
using myfirstrestapi.GenericResponse;
using myfirstrestapi.IServices;
namespace myfirstrestapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserDto user)
        {
            try
            {
                var result = await authService.LoginUser(user);
                if (result.Item1 == 0)
                {
                    return NotFound(Response<TokenDto>.failure(null, result.Item2.Message));
                }

                return Ok(Response<TokenDto>.success(result.Item2, result.Item2.Message));
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto user)
        {
            try
            {
                var result = await authService.register(user);
                if (result.Item1 == 0)
                {
                    return BadRequest(Response<UserDto>.failure(null, result.Item2));
                }
                return Ok(Response<UserDto>.success(user, "Register successful"));
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
