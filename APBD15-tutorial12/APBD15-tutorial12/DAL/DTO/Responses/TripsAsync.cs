namespace APBD15_tutorial12.DAL.DTO.Responses;

public class TripsAsync
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public List<TripDto> Trips { get; set; }
}