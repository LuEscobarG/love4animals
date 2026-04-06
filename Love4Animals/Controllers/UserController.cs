using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Love4AnimalsApi.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult<List<GetUserDto>> GetUsers()
        {
            return Ok(userService.GetUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<GetUserDto> GetUserById(int id)
        {
            var user = userService.GetUserById(id);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<GetUserDto> CreateUser([FromBody] CreateUserDto dto)
        {
            var createdUser = userService.CreateUser(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var updated = userService.UpdateUser(id, dto);

            if (updated == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var deleted = userService.DeleteUser(id);

            if (!deleted)
                return NotFound(new { message = "Usuario no encontrado" });

            return NoContent();
        }
    }
}