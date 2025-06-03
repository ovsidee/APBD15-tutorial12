using APBD15_tutorial12.DAL.DTO.Responses;

namespace APBD15_tutorial12.Services;

public interface ITripsService
{
    public Task<(List<TripDto> Trips, int TotalPages)> GetAllTripsDescQueryStringAsync(int page, int pageSize, CancellationToken cancellationToken);
}