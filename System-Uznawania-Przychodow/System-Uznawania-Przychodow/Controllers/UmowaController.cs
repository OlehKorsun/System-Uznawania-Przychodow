using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/[controller]")]
public class UmowaController : ControllerBase
{
    private readonly IUmowaService _umowaService;
    private readonly AppDbContext _context;

    public UmowaController(IUmowaService umowaService)
    {
        _umowaService = umowaService;
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