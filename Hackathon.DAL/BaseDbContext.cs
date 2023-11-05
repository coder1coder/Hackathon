using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.DAL.Entities.Interfaces;
using Hackathon.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL;

public abstract class BaseDbContext: DbContext
{
    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyGlobalFilters<ISoftDeletable>(e => !e.IsDeleted);
    }
    
    public override int SaveChanges()
    {
        SetDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetDates()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is IHasCreatedAt or IHasModifyAt 
                && e.State is EntityState.Added or EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            switch (entityEntry.State)
            {
                case EntityState.Added when entityEntry.Entity is IHasCreatedAt createdEntity:
                    createdEntity.CreatedAt = DateTime.UtcNow;
                    break;
                
                case EntityState.Modified when entityEntry.Entity is IHasModifyAt modifiedEntity:
                    modifiedEntity.ModifyAt = DateTime.UtcNow;
                    break;
                
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
