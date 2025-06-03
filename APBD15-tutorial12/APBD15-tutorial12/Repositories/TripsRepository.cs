using APBD15_tutorial12.DAL.DTO.Responses;
using APBD15_tutorial12.DAL.Repositories;
using APBD15_tutorial12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBD15_tutorial12.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly TripsDbContext _context;

    public TripsRepository(TripsDbContext context)
    {
        _context = context;
    }

    public async Task<(List<TripDto> Trips, int TotalPages)> GetAllTripsDescQueryStringAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var countOfTotalTrips = await _context.Trips.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(countOfTotalTrips / (double)pageSize);

        var trips = await _context.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDto
                {
                    Name = c.Name
                }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return (trips, totalPages);
    }
}