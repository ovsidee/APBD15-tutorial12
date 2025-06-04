using APBD15_tutorial12.DAL;
using APBD15_tutorial12.DAL.DTO.Requests;
using APBD15_tutorial12.DAL.DTO.Responses;
using APBD15_tutorial12.DAL.Repositories;

namespace APBD15_tutorial12.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TripsService(ITripsRepository tripsRepository, IUnitOfWork unitOfWork)
    {
        _tripsRepository = tripsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(List<TripDto> Trips, int TotalPages)> GetAllTripsDescQueryStringAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            var trips = await _tripsRepository.GetAllTripsDescQueryStringAsync(page, pageSize, cancellationToken);

            _unitOfWork.Commit();
            return trips;
        }
        catch (Exception)
        {
            _unitOfWork.RollBack();
            return (null, 0);
        }
    }

    public async Task<string> AddClientToClientTripAsync(int tripId, ClientDTORequest requestClient, CancellationToken cancellationToken)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            
            var resultOfAssigningToTripAsync = await _tripsRepository.AssignClientToTripAsync(tripId, requestClient, cancellationToken);

            if (resultOfAssigningToTripAsync.Equals("success"))
                _unitOfWork.Commit();
            else
                _unitOfWork.RollBack();
            
            return resultOfAssigningToTripAsync; 
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return "error";
        }
    }
}