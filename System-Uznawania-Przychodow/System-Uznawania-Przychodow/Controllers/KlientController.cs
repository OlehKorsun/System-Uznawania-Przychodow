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
        try
        {
            await _klientService.CreateIndividualAsync(request);
            return Created();
        }
        catch (ClientHasExistsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("firma")]
    public async Task<IActionResult> AddClientFirma(CreateFirmaRequest request)
    {
        try
        {
            await _klientService.CreateFirmaAsync(request);
            return Created();

        }
        catch (ClientHasExistsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("klient/{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        try
        {
            await _klientService.DeleteClient(idClient);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("client/{idClient}")]
    public async Task<IActionResult> UpdateClient([FromBody]UpdateClientRequest request, int idClient)
    {
        try
        {
            await _klientService.UpdateClientAsync(request, idClient);
            return Ok();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UpdateClientException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    
}