namespace APBD15_tutorial12.DAL.Repositories;

public interface IClientsRepository
{
    public Task<string> DeleteClientByIdAsync(int id, CancellationToken cancellationToken);
}