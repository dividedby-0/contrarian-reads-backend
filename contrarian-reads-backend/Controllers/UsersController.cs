using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    // GET: api/Users/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        return await _userService.GetUser(id);
    }

    // GET: api/Users/profile/{userId}
    [Authorize]
    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<UserProfileDTO>> GetUserProfile(string userId)
    {
        if (!Guid.TryParse(userId, out var guid))
            return BadRequest();

        return await _userService.GetUserProfile(Guid.Parse(userId));
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return await _userService.CreateUser(createUserDTO);
    }

    // PUT: api/Users/{id}
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDTO>> UpdateUser(string id, CreateUserDTO createUserDTO)
    {
        return await _userService.UpdateUser(id, createUserDTO);
    }

    // DELETE: api/Users/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDTO>> DeleteUser(string id)
    {
        return await _userService.DeleteUser(id);
    }
}