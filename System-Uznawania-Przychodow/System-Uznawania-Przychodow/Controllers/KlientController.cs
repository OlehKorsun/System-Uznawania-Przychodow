using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[Authorize(Roles="admin")]
[ApiController]
[Route("api/[controller]")]
public class KlientController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IKlientService _klientService;

    public KlientController(IKlientService klientService, IConfiguration configuration)
    {
        _klientService = klientService;
        _configuration = configuration;
    }
    
    
    public async Task<IActionResult> AddClientOsobaFizyczna()
    {
        return Ok();
    }

    public async Task<IActionResult> AddClientFirma()
    {
        return Ok();
    }

    public async Task<IActionResult> DeleteClient()
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