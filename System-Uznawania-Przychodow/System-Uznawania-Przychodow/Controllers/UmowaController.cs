using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UmowaController : ControllerBase
{
    private readonly IUmowaService _umowaService;
    private readonly ApbdContext _context;

    public UmowaController(IUmowaService umowaService, ApbdContext context)
    {
        _umowaService = umowaService;
        _context = context;
    }


    public async Task<IActionResult> CreateUmowa()
    {
        return Ok();
    }

    public async Task<IActionResult> BillingContract()
    {
        return Ok();
    }
}