using APBD15_tutorial12.Data;
using Microsoft.EntityFrameworkCore;

namespace APBD15_tutorial12.DAL.Repositories;

public class ClientsRepository : IClientsRepository
{
    public TripsDbContext _DbContext { get; set; }
    
    public ClientsRepository(TripsDbContext dbContext)
    {
        _DbContext = dbContext;
    }

    public async Task<string> DeleteClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        var client = await _DbContext.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == id, cancellationToken);

        if (client == null)
            return "notFound";

        if (client.ClientTrips.Any())
            return "hasTrips";

        _DbContext.Clients.Remove(client);

        return "success";
    }
}