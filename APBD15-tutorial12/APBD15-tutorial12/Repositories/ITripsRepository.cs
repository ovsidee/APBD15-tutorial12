using APBD15_tutorial12.DAL.DTO.Responses;

namespace APBD15_tutorial12.DAL.Repositories;

public interface ITripsRepository
{
    public Task<(List<TripDto> Trips, int TotalPages)> GetAllTripsDescQueryStringAsync(int page, int pageSize, CancellationToken cancellationToken);
}