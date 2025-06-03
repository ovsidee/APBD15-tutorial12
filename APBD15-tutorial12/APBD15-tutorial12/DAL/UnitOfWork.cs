using APBD15_tutorial12.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace APBD15_tutorial12.DAL;

public class UnitOfWork : IUnitOfWork
{
    public TripsDbContext TripsDbContext { get; set; }
    public IDbContextTransaction _transaction { get; set; }
    
    public UnitOfWork(TripsDbContext tripsDbContext)
    {
        TripsDbContext = tripsDbContext;
    }
    
    public void BeginTransaction()
    {
        _transaction = TripsDbContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            TripsDbContext.SaveChanges();
            _transaction.Commit();
        } 
        finally
        {
            _transaction.Dispose();
        }
    }

    public void RollBack()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
        }
    }
}