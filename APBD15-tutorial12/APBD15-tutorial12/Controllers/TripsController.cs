using APBD15_tutorial12.DAL.DTO.Requests;
using APBD15_tutorial12.DAL.DTO.Responses;
using APBD15_tutorial12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD15_tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    public ITripsService _tripsService { get; set; }
    
    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var (trips, totalPages) = await _tripsService.GetAllTripsDescQueryStringAsync(page, pageSize, cancellationToken);
    
        if (!trips.Any())
            return NotFound("No trips found");

        return Ok(new TripsAsync
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = totalPages,
            Trips = trips
        });
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> CreateClientTripAsync(int idTrip, [FromBody] ClientDTORequest requestClient, CancellationToken cancellationToken)
    {
        var resultOfAdding = await _tripsService.AddClientToClientTripAsync(idTrip, requestClient, cancellationToken);
        
        return resultOfAdding switch
        {
            "clientAlreadyExistsPesel" => Conflict("Client with this PESEL already exists."),
            "tripNotFound" => NotFound("Trip not found."),
            "tripDateFromIsInPast" => BadRequest("Cannot assign to a past trip."),
            "success" => Ok("Client successfully assigned to the trip."),
            "error" => StatusCode(500, "Unknown error occurred."),
            _ => StatusCode(500, "Unknown error occurred.")
        };
    }
}