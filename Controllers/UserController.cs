using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Repository;
using System.Collections.Generic;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IJWTManagerRepository jWTManagerRepository;

    public UserController(IJWTManagerRepository jWTManagerRepository)
    {
        this.jWTManagerRepository = jWTManagerRepository;
    }

    [HttpGet]
    [Authorize]
    [Route("userlist")]
    public List<string> Get()
    {
        var users = new List<string>
        {
            "Utku Erkoç",
            "Bir şey",
            "İki şey"
        };
        return users;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("authenticate")]
    public IActionResult Authenticate(Users userdata)
    {
        var token = jWTManagerRepository.Authenticate(userdata);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public IActionResult Register([FromBody] Users userdata)
    {
        if (userdata == null || string.IsNullOrWhiteSpace(userdata.Name) || string.IsNullOrWhiteSpace(userdata.Password))
        {
            return BadRequest("Invalid user data.");
        }

        bool isRegistered = jWTManagerRepository.RegisterUser(userdata.Name, userdata.Password);

        if (isRegistered)
        {
            return Ok(new { message = "User registered successfully.", user = new { Name = userdata.Name } });
        }

        return Conflict("Username is already taken. Please choose a different username.");
    }
}
