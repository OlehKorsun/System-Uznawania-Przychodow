namespace System_Uznawania_Przychodow.Tests;

using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Services;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

public class AuthServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<IConfiguration> _configurationMock;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new AppDbContext(options);
        _configurationMock = new Mock<IConfiguration>();

        
        _configurationMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_1234567890jhgty");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("MyApp");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("MyAppUsers");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreCorrect()
    {
        
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var password = "test123";
        var hashedPassword = ComputeHash(password + salt);

        var role = new Rola { IdRola = 1, Nazwa = "user" };

        var user = new User
        {
            IdUser = 1,
            Login = "testuser",
            Password = hashedPassword,
            Salt = salt,
            Email = "test@example.com",
            IdRola = 1,
            IdRolaNavigation = role
        };

        await _context.Rolas.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var service = new AuthService(_context, _configurationMock.Object);

        var loginRequest = new LoginRequest
        {
            Login = "testuser",
            Password = password
        };

        
        var token = await service.LoginAsync(loginRequest);

        
        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenLoginIsInvalid()
    {
        
        var service = new AuthService(_context, _configurationMock.Object);
        var loginRequest = new LoginRequest { Login = "nonexistent", Password = "xxx" };

        
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsIncorrect()
    {
        
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var password = "correct";
        var hashedPassword = ComputeHash(password + salt);

        var role = new Rola { IdRola = 2, Nazwa = "admin" };

        var user = new User
        {
            IdUser = 2,
            Login = "admin",
            Password = hashedPassword,
            Salt = salt,
            Email = "admin@example.com",
            IdRola = 2,
            IdRolaNavigation = role
        };

        await _context.Rolas.AddAsync(role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var service = new AuthService(_context, _configurationMock.Object);
        var loginRequest = new LoginRequest { Login = "admin", Password = "wrong" };

        
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.LoginAsync(loginRequest));
    }

    private static string ComputeHash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
