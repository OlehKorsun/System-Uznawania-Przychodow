using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Exceptions;
using System_Uznawania_Przychodow.Models;
using System_Uznawania_Przychodow.Requests;
using LoginRequest = System_Uznawania_Przychodow.Requests.LoginRequest;

namespace System_Uznawania_Przychodow.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    
    public async Task<String> LoginAsync(LoginRequest dto)
    {
        var user = await _context.Users
            .Include(u => u.IdRolaNavigation)
            .FirstOrDefaultAsync(u => u.Login == dto.Login);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var hash = ComputeHash(dto.Password + user.Salt);
        if (user.Password != hash)
        {
           throw new UnauthorizedAccessException("Invalid password.");
        }

        var token = GenerateJwt(user);
        return token;
    }

    private string GenerateJwt(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.IdRolaNavigation.Nazwa)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }



    public async Task RegisterAsync(RegisterUserRequest dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(a => a.Login == dto.Login);
        if (user != null)
        {
            throw new UserExistsException("Użytkownik o podanym loginie już istnieje!");
        }

        var hashSold = CreateSold();
        var hashPassword = ComputeHash(dto.Password);

        var rola = await _context.Rolas.FirstOrDefaultAsync(r => r.Nazwa == dto.Rola);
        int rolaId;
        if (rola == null)
        {
            rolaId = 2;  // domyślnia rola - user
        }
        else
        {
            rolaId = rola.IdRola;
        }

        var maxId = await _context.Users.MaxAsync(a => a.IdUser);
        
        user = new User()
        {
            Login = dto.Login,
            Password = hashPassword,
            Salt = hashSold,
            Email = dto.Email,
            IdRola = rolaId,
            IdUser = maxId+1
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }


    private static string CreateSold()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string saltBase64 = Convert.ToBase64String(salt);
        return saltBase64;
    }
    
    
}