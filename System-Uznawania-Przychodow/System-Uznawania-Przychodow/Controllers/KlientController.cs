using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodow.Data;
using System_Uznawania_Przychodow.Exceptions;
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

    [HttpPost("individual")]
    public async Task<IActionResult> AddClientOsobaFizyczna([FromBody]CreateIndividualRequest request)
    {
        await _klientService.CreateIndividualAsync(request);
        return Created();
    }

    [HttpPost("firma")]
    public async Task<IActionResult> AddClientFirma(CreateFirmaRequest request)
    {
        await _klientService.CreateFirmaAsync(request);
        return Created();
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        await _klientService.DeleteClient(idClient);
        return Ok();
    }

    [HttpPut("{idClient}")]
    public async Task<IActionResult> UpdateClient([FromBody]UpdateClientRequest request, int idClient)
    {
        await _klientService.UpdateClientAsync(request, idClient);
        return Ok();
    }
    
    
}