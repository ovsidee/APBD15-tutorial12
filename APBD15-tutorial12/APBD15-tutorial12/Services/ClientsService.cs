using APBD15_tutorial12.DAL;
using APBD15_tutorial12.DAL.Repositories;

namespace APBD15_tutorial12.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;
    
    private IUnitOfWork UnitOfWork;
    
    public ClientsService(IUnitOfWork unitOfWork, IClientsRepository clientsRepository)
    {
        UnitOfWork = unitOfWork;
        _clientsRepository = clientsRepository;
    }
    
    public async Task<string> DeleteClientByIdServiceAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            UnitOfWork.BeginTransaction();
            
            var resultOfDeleting = await _clientsRepository.DeleteClientByIdAsync(id, cancellationToken);

            if (resultOfDeleting.Equals("success")) 
                UnitOfWork.Commit();
            else 
                UnitOfWork.RollBack();
            
            return resultOfDeleting;
        }
        catch (Exception e)
        {
            UnitOfWork.RollBack();
            return "error";
        }
    }
}