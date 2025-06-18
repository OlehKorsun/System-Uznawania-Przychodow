using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Services;

namespace System_Uznawania_Przychodow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;
    private readonly ApbdContext _context;

    public RevenueController(IRevenueService revenueService, ApbdContext context)
    {
        _revenueService = revenueService;
        _context = context;
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