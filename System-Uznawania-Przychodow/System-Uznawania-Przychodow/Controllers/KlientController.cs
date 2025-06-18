using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class KlientController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IKlientService _klientService;
    private readonly ApbdContext _context;

    public KlientController(IKlientService klientService, ApbdContext context, IConfiguration configuration)
    {
        _klientService = klientService;
        _context = context;
        _configuration = configuration;
    }

    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        try
        {
            await _klientService.RegisterUserAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            await _klientService.LoginAsync(request);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    
    public async Task<IActionResult> AddClientOsobaFizyczna()
    {
        return Ok();
    }

    public async Task<IActionResult> AddClientFirma()
    {
        return Ok();
    }

    public async Task<IActionResult> DeleteClientOsobaFizyczna()
    {
        return Ok();
    }

    public async Task<IActionResult> UpdateClientOsobaFizyczna()
    {
        return Ok();
    }

    public async Task<IActionResult> UpdateClientFirma()
    {
        return Ok();
    }
    
}