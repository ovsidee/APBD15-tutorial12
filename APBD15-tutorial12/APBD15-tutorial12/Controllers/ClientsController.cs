using APBD15_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD15_tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    public IClientsService _ClientsService { get; set; }
    
    public ClientsController(IClientsService ClientsService)
    {
        _ClientsService = ClientsService;
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        var resultOfDelete = await _ClientsService.DeleteClientByIdServiceAsync(id, cancellationToken);
        
        return resultOfDelete switch
        {
            "success" => Ok("Client deleted"),
            "notFound" => NotFound("Client not found"),
            "hasTrips" => Conflict("Client has trips"),
            "error" => StatusCode(500, "Error"),
            _ => StatusCode(500, "Error")
        };
    }
}