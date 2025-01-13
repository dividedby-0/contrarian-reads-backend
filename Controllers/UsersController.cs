using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.DTOs;
using contrarian_reads_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userDTOs = _mapper.Map<List<UserDTO>>(users);
            return Ok(userDTOs);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var user = await _context.Users.FindAsync(guidId);

            if (user == null)
            {
                return NotFound();
            }

            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDTO)
        {
            if (await _context.Users.AnyAsync(u => u.Email == createUserDTO.Email))
            {
                return BadRequest("A user with this email already exists.");
            }

            var user = _mapper.Map<User>(createUserDTO);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);

            user.Id = Guid.NewGuid();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, _mapper.Map<UserDTO>(user));
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserDTO userDTO)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var existingUser = await _context.Users.FindAsync(guidId);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = userDTO.Username;
            existingUser.Email = userDTO.Email;
            existingUser.ProfilePictureUrl = userDTO.ProfilePictureUrl;
            existingUser.Bio = userDTO.Bio;

            await _context.SaveChangesAsync();

            var updatedUserDTO = _mapper.Map<UserDTO>(existingUser);
            return Ok(updatedUserDTO);
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return BadRequest("Invalid GUID format.");
            }

            var user = await _context.Users.FindAsync(guidId);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
