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
    public IActionResult Login([FromBody] Login login)
    {
        var user = userRepository.GetOne(login.Email);

        if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password)) return Empty;
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetEnvironment("JWT_SECRET")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "your_issuer",
            audience: "your_audience",
            claims: new List<Claim>(),
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);
        
        return Ok(token.EncodedPayload);
    }

    [HttpGet]
    [Route(template: "test")]
    public IActionResult Test()
    {
        return Ok();
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        return claims;
    }
}