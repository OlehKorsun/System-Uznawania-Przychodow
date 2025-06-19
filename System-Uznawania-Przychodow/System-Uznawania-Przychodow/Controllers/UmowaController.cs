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
        try
        {
            await _umowaService.CreateUmowaAsync(request);
            return Created();
        }
        catch (DateException e)
        {
            return BadRequest(e.Message);
        }
        catch (ClientHasContractException e)
        {
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{idUmowa}")]
    public async Task<IActionResult> BillingContract(int idUmowa)
    {
        try
        {
            var result = await _umowaService.BillingContract(idUmowa);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}