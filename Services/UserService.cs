using AutoMapper;
using contrarian_reads_backend.Data;
using contrarian_reads_backend.Models;
using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace contrarian_reads_backend.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext context, IMapper mapper)

    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDTO)
    {
        if (await _context.Users.AnyAsync(u => u.Email == createUserDTO.Email))
            return new BadRequestObjectResult("A user with this email already exists.");

        var user = _mapper.Map<User>(createUserDTO);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password);

        user.Id = Guid.NewGuid();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new OkObjectResult(_mapper.Map<UserDTO>(user));
    }

    public async Task<ActionResult<UserDTO>> DeleteUser(string id)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var user = await _context.Users.FindAsync(guidId);

        if (user == null) return new NotFoundObjectResult("User not found.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return new NoContentResult();
    }

    public async Task<ActionResult<UserDTO>> GetUser(string id)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var user = await _context.Users.FindAsync(guidId);

        if (user == null) return new NotFoundObjectResult("User not found.");

        var userDTO = _mapper.Map<UserDTO>(user);
        return new OkObjectResult(userDTO);
    }

    public Task<ActionResult<int>> GetUserCount()
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResult<List<UserDTO>>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        var userDTOs = _mapper.Map<List<UserDTO>>(users);
        return new OkObjectResult(userDTOs);
    }

    public async Task<ActionResult<UserDTO>> UpdateUser(string id, CreateUserDTO createUserDTO)
    {
        if (!Guid.TryParse(id, out var guidId)) return new BadRequestObjectResult("Invalid GUID format.");

        var existingUser = await _context.Users.FindAsync(guidId);

        if (existingUser == null) return new NotFoundObjectResult("User not found.");

        existingUser.Username = createUserDTO.Username;
        existingUser.Email = createUserDTO.Email;
        existingUser.ProfilePictureUrl = createUserDTO.ProfilePictureUrl;
        existingUser.Bio = createUserDTO.Bio;

        await _context.SaveChangesAsync();

        var updatedUserDTO = _mapper.Map<UserDTO>(existingUser);
        return new OkObjectResult(updatedUserDTO);
    }
}