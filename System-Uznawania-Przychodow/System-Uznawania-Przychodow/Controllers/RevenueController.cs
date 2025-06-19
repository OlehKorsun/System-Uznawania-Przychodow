using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Requests;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[Authorize(Roles = "user")]
[ApiController]
[Route("api/[controller]")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [HttpGet]
    public async Task<IActionResult> CalculateRevenue([FromBody] PrzychodRequest przychodRequest)
    {
        try
        {
            var przychod = await _revenueService.GetPrzychod(przychodRequest);
            return Ok(przychod);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("anticipated")]
    public async Task<IActionResult> CalculateAnticipatedRevenue([FromBody] PrzychodRequest przychodRequest)
    {
        try
        {
            var przychod = await _revenueService.GetPrzychodPrzewidywalny(przychodRequest);
            return Ok(przychod);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}