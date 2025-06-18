using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
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

    public async Task<IActionResult> CalculateRevenue()
    {
        return Ok();
    }


    public async Task<IActionResult> CalculateAnticipatedRevenue()
    {
        return Ok();
    }
}