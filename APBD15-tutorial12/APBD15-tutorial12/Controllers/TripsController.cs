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

        return Ok(new
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = totalPages,
            trips = trips
        });
    }

}