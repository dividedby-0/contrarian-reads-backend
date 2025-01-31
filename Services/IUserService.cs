using contrarian_reads_backend.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace contrarian_reads_backend.Services
{
    public interface IUserService
    {
        Task<ActionResult<UserDTO>> GetUser(string id);

        Task<ActionResult<List<UserDTO>>> GetUsers();

        Task<ActionResult<int>> GetUserCount();

        Task<ActionResult<UserDTO>> CreateUser(CreateUserDTO createUserDTO);

        Task<ActionResult<UserDTO>> UpdateUser(string id, CreateUserDTO createUserDTO);

        Task<ActionResult<UserDTO>> DeleteUser(string id);
    }
}