using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtService jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            this.userService = userService;
            this.jwtService = jwtService;
        }

        [HttpGet]
        [OutputCache(PolicyName = "Default", Tags = ["users"])]
        [EndpointSummary("Obtener todos los usuarios")]
        [ProducesResponseType<List<GetUserDto>>(200)]
        public ActionResult<List<GetUserDto>> GetUsers()
        {
            return Ok(userService.GetUsers());
        }

        [HttpGet("profile/{id}")]
        [OutputCache(PolicyName = "Default", Tags = ["users"])]
        [EndpointSummary("Obtener perfil de usuario por ID")]
        [ProducesResponseType<GetUserDto>(200)]
        [ProducesResponseType(404)]
        public ActionResult<GetUserDto> GetUserById(int id)
        {
            var user = userService.GetUserById(id);
            if (user == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [EnableRateLimiting("Public")]
        [EndpointSummary("Registrar nuevo usuario")]
        [Consumes("application/json")]
        [ProducesResponseType<GetUserDto>(201)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(429)]
        public async Task<ActionResult<GetUserDto>> Register([FromBody] CreateUserDto dto, [FromServices] IOutputCacheStore cache, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                var createdUser = userService.CreateUser(dto);
                await cache.EvictByTagAsync("users", ct);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("Public")]
        [EndpointSummary("Iniciar sesión")]
        [Consumes("application/json")]
        [ProducesResponseType<LoginResponseDto>(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(429)]
        public ActionResult<LoginResponseDto> Login([FromBody] LoginDto dto)
        {
            var result = userService.Login(dto);
            if (result == null) return Unauthorized(new { message = "Email o contraseña incorrectos" });
            return Ok(result);
        }

        [HttpPut("profile/{id}")]
        [EndpointSummary("Modificar perfil de usuario")]
        [Consumes("application/json")]
        [ProducesResponseType<GetUserDto>(200)]
        [ProducesResponseType<ValidationProblemDetails>(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProfile(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = userService.UpdateUser(id, dto);
            if (updated == null) return NotFound(new { message = "Usuario no encontrado" });
            return Ok(updated);
        }

        [HttpDelete("profile/{id}")]
        [EndpointSummary("Eliminar usuario")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int id)
        {
            var deleted = userService.DeleteUser(id);
            if (!deleted) return NotFound(new { message = "Usuario no encontrado" });
            return NoContent();
        }

        [HttpPost("logout")]
        [EndpointSummary("Cerrar sesión")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout()
        {

            var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

            if (jti == null || expClaim == null)
                return Unauthorized(new { message = "Token inválido" });

 
            var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime;
            var remaining = expiry - DateTime.UtcNow;

            if (remaining > TimeSpan.Zero)
                await jwtService.RevokeTokenAsync(jti, remaining);

            return NoContent();
        }
    }
}
