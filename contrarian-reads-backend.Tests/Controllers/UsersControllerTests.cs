using contrarian_reads_backend.Controllers;
using contrarian_reads_backend.Services.DTOs;
using contrarian_reads_backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UsersController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetUsers_ReturnsListOfUsers()
    {
        // Arrange
        var expectedUsers = new List<UserDTO>
        {
            new UserDTO(
                Id: Guid.NewGuid(),
                Username: "testuser1",
                Email: "test1@example.com",
                ProfilePictureUrl: "https://example.com/pic1.jpg",
                CreatedAt: DateTime.UtcNow,
                Bio: "Test bio 1"
            ),
            new UserDTO(
                Id: Guid.NewGuid(),
                Username: "testuser2",
                Email: "test2@example.com",
                ProfilePictureUrl: "https://example.com/pic2.jpg",
                CreatedAt: DateTime.UtcNow,
                Bio: "Test bio 2"
            )
        };

        _userServiceMock.Setup(x => x.GetUsers())
            .ReturnsAsync(new OkObjectResult(expectedUsers));

        // Act
        var result = await _controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<ActionResult<List<UserDTO>>>(result);
        var userList = Assert.IsType<List<UserDTO>>(((OkObjectResult)okResult.Result).Value);
        Assert.Equal(2, userList.Count);
    }

    [Fact]
    public async Task GetUser_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var expectedUser = new UserDTO(
            Id: Guid.Parse(userId),
            Username: "testuser",
            Email: "test@example.com",
            ProfilePictureUrl: "https://example.com/pic.jpg",
            CreatedAt: DateTime.UtcNow,
            Bio: "Test bio"
        );

        _userServiceMock.Setup(x => x.GetUser(userId))
            .ReturnsAsync(new OkObjectResult(expectedUser));

        // Act
        var result = await _controller.GetUser(userId);

        // Assert
        var okResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var returnedUser = Assert.IsType<UserDTO>(((OkObjectResult)okResult.Result).Value);
        Assert.Equal(expectedUser.Username, returnedUser.Username);
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreatedUser()
    {
        // Arrange
        var createUserDto = new CreateUserDTO
        {
            Id = Guid.NewGuid(),
            Username = "newuser",
            Email = "new@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            ProfilePictureUrl = "https://example.com/pic.jpg",
            Bio = "New user bio"
        };

        var expectedUser = new UserDTO(
            Id: createUserDto.Id,
            Username: createUserDto.Username,
            Email: createUserDto.Email,
            ProfilePictureUrl: createUserDto.ProfilePictureUrl,
            CreatedAt: DateTime.UtcNow,
            Bio: createUserDto.Bio
        );

        _userServiceMock.Setup(x => x.CreateUser(createUserDto))
            .ReturnsAsync(new OkObjectResult(expectedUser));

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        var okResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var createdUser = Assert.IsType<UserDTO>(((OkObjectResult)okResult.Result).Value);
        Assert.Equal(createUserDto.Username, createdUser.Username);
        Assert.Equal(createUserDto.Email, createdUser.Email);
    }

    [Fact]
    public async Task GetUserProfile_WithValidId_ReturnsUserProfile()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var expectedProfile = new UserProfileDTO(
            User: new UserDTO(
                Id: Guid.Parse(userId),
                Username: "testuser",
                Email: "test@example.com",
                ProfilePictureUrl: "https://example.com/pic.jpg",
                CreatedAt: DateTime.UtcNow,
                Bio: "Test bio"
            ),
            AddedSuggestions: new List<SuggestionDTO>(),
            UpvotedSuggestions: new List<SuggestionDTO>(),
            Comments: new List<CommentDTO>()
        );

        _userServiceMock.Setup(x => x.GetUserProfile(Guid.Parse(userId)))
            .ReturnsAsync(new OkObjectResult(expectedProfile));

        // Act
        var result = await _controller.GetUserProfile(userId);

        // Assert
        var okResult = Assert.IsType<ActionResult<UserProfileDTO>>(result);
        var profile = Assert.IsType<UserProfileDTO>(((OkObjectResult)okResult.Result).Value);
        Assert.Equal(expectedProfile.User.Username, profile.User.Username);
    }

    [Fact]
    public async Task DeleteUser_WithValidId_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        _userServiceMock.Setup(x => x.DeleteUser(userId))
            .ReturnsAsync(new NoContentResult());

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_WithMissingUsername_ReturnsBadRequest()
    {
        // Arrange
        var createUserDto = new CreateUserDTO
        {
            Id = Guid.NewGuid(),
            // Username is missing
            Email = "test@example.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _controller.ModelState.AddModelError("Username", "Required");

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        var createUserDto = new CreateUserDTO
        {
            Id = Guid.NewGuid(),
            Username = "user",
            Email = "not-an-email",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };
        _controller.ModelState.AddModelError("Email", "Invalid email format");
        var result = await _controller.CreateUser(createUserDto);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUser_WithEmptyId_ReturnsBadRequest()
    {
        // Act
        var result = await _controller.GetUser(string.Empty);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task GetUser_WithNonExistentId_ReturnsNotFound()
    {
        var userId = Guid.NewGuid().ToString();
        _userServiceMock.Setup(x => x.GetUser(userId))
            .ReturnsAsync(new NotFoundResult());
        var result = await _controller.GetUser(userId);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetUserProfile_WithInvalidGuid_ReturnsBadRequest()
    {
        var result = await _controller.GetUserProfile("not-a-guid");
        Assert.IsType<BadRequestResult>(result.Result);
    }
}