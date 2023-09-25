using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;

public interface IJWTManagerRepository
{
    Tokens Authenticate(Users user);
    bool RegisterUser(string username, string password);
    Dictionary<string, string> GetUserRecords();
}

public class JWTManagerRepository : IJWTManagerRepository
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string> UserRecords;

    public JWTManagerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        UserRecords = new Dictionary<string, string>
        {
            {"User1", "Password1"},
            {"User2", "Password2"},
            {"User3", "Password3"}
        };
    }

    public Tokens Authenticate(Users user)
    {
        if (!UserRecords.Any(x => x.Key == user.Name && x.Value == user.Password))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Name) }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Tokens { Token = tokenHandler.WriteToken(token) };
    }

    public bool RegisterUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        // Check if the username already exists
        if (UserRecords.ContainsKey(username))
        {
            return false;
        }

        UserRecords[username] = password;
        return true;
    }

    public Dictionary<string, string> GetUserRecords()
    {
        return UserRecords;
    }
}
