namespace APBD15_tutorial12.Services;

public interface IClientsService
{
    public Task<string> DeleteClientByIdServiceAsync(int id, CancellationToken cancellationToken);
}