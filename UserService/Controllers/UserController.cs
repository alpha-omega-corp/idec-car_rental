using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarRental.Database;
using CarRental.Domain;
using CarRental.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.Controllers;

[ApiController]
[Route(template: "/users")]
public class UserController(
    ILogger<UserController> logger,
    UserRepository userRepository
) : ControllerBase
{

    [HttpPost]
    [Route(template: "register")]
    public IActionResult Register([FromBody] User user)
    {
        logger.LogInformation("Registering {@user}", user);
        
        userRepository.Create(user);
        return Ok(user.Email);
    }
    
    
    [HttpPost]
    [Route(template: "login")]
    public IActionResult Login([FromBody] Login user)
    {
        var email = userRepository.GetOne(user.Email).Email;
        logger.LogInformation("email {@email}", email);
        
        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Email),
        };
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration.GetEnvironment("JWT_SECRET"))
                ),
                SecurityAlgorithms.HmacSha256Signature)
        );
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwtToken));

    }
}