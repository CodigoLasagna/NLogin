using Business.Contracts;
using Microsoft.AspNetCore.Mvc;
using Nlog.model;

namespace Nlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
        private readonly IUserService _userService;
    
        public UserController(IUserService activityService)
        {
            _userService = activityService;
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register(User entity)
        {
            string public_key = _userService.Register(entity);
            return Ok(public_key);
        }
        [HttpPost("login/{key}")]
        public async Task<ActionResult> Login(User entity, [FromRoute] string key)
        {
            string response = _userService.Login(entity, key);
            if (response.Equals("0")) return NotFound();
            if (response.Equals("-1")) return Unauthorized();
            if (response.Equals("-2")) return BadRequest();
            return Ok(response);
        }
        [HttpGet("read/{Id}")]
        public async Task<ActionResult> Read([FromRoute]int Id)
        {
            User entity = _userService.Read(Id);
            return Ok(entity);
        }

}