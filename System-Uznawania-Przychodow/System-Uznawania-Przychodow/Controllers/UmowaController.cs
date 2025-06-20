using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Exceptions;
using System_Uznawania_Przychodow.Requests;
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

    [HttpPost]
    public async Task<IActionResult> CreateUmowa(CreateUmowaRequest request)
    {
        await _umowaService.CreateUmowaAsync(request);
        return Created();
    }

    [HttpGet("{idUmowa}")]
    public async Task<IActionResult> BillingContract(int idUmowa)
    {
        var result = await _umowaService.BillingContract(idUmowa); 
        return Ok(result);
    }
}