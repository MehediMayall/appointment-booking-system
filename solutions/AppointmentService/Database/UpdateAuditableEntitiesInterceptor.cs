using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AppointmentService;

public sealed class UpdateAuditableEntitiesInterceptor() : SaveChangesInterceptor
{
    
    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        // Get logged user
        // SessionUserDto? user = userSession.GetSessionUser();


        IEnumerable<EntityEntry<EntityBase<Guid>>> changesEntities = dbContext.ChangeTracker.Entries<EntityBase<Guid>>();

        // foreach (EntityEntry<EntityBase<Guid>> entityEntry in changesEntities)
        // {
        //     // WHEN ADDED
        //     if (entityEntry.State == EntityState.Added){
        //         entityEntry.Property(e=> e.CreatedOn).CurrentValue = DateTime.UtcNow;
        //         if (user != null)
        //             entityEntry.Property(e=> e.CreatedById).CurrentValue = user.UserId;
        //     }

        //     // WHEN MODIFIED
        //     if (entityEntry.State == EntityState.Modified){
        //         entityEntry.Property(e=> e.UpdatedOn).CurrentValue = DateTime.UtcNow;
        //         if (user != null)
        //             entityEntry.Property(e=> e.UpdatedById).CurrentValue = user.UserId;
        //     }
        // }


        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}