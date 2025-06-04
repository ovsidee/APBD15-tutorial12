using APBD15_tutorial12.DAL.DTO.Requests;
using APBD15_tutorial12.DAL.DTO.Responses;
using APBD15_tutorial12.DAL.Repositories;
using APBD15_tutorial12.Data;
using APBD15_tutorial12.Models;
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

        var trips = await _context
            .Trips
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
        
    public async Task<string> AssignClientToTripAsync(int tripId, ClientDTORequest requestClient, CancellationToken cancellationToken)
    {
        //1. searching client by pesel
        var resultOfSearchingByPesel =  await _context
                                            .Clients
                                            .FirstOrDefaultAsync(c => c.Pesel == requestClient.Pesel, cancellationToken);
        
        if (resultOfSearchingByPesel != null)
            return "clientAlreadyExistsPesel";
        
        //inserting client
        var newClientToBeInserted = new Client 
        {
            FirstName = requestClient.FirstName,
            LastName = requestClient.LastName,
            Email = requestClient.Email,
            Telephone = requestClient.Telephone,
            Pesel = requestClient.Pesel 
        };
        
        await _context.Clients.AddAsync(newClientToBeInserted, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        //2. searching trip by id
        var resultOfSearchingById = await _context
                                        .Trips
                                        .FirstOrDefaultAsync(t => t.IdTrip == tripId, cancellationToken);
        
        if (resultOfSearchingById == null)
            return "tripNotFound";
        
        //checking DateFrom
        if (resultOfSearchingById.DateFrom < DateTime.Now)
            return "tripDateFromIsInPast";
        
        //3. inserting client to trip
        var newClientTrip = new ClientTrip
        {
            IdClient = newClientToBeInserted.IdClient,
            IdTrip = resultOfSearchingById.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = requestClient.PaymentDate,
        };
        
        await _context.ClientTrips.AddAsync(newClientTrip, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return "success";
    }
}