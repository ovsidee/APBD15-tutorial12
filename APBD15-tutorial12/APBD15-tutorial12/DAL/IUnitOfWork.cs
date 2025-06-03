namespace APBD15_tutorial12.DAL;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void RollBack();
}