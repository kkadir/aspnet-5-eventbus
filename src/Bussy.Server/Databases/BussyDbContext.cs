using System;
using System.Threading;
using System.Threading.Tasks;
using Bussy.Server.Domain;
using Bussy.Server.Domain.Orders;
using Bussy.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace Bussy.Server.Databases
{
    public class BussyDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public BussyDbContext(DbContextOptions<BussyDbContext> options, ICurrentUserService currentUserService) :
            base(options)
        {
            _currentUserService = currentUserService;
        }
        
        public DbSet<Order> Orders { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService?.UserId ?? "N/A";
                        entry.Entity.CreatedOn = now;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedBy = _currentUserService?.UserId ?? "N/A";
                        entry.Entity.DeletedOn = now;
                        entry.Entity.IsDeleted = true;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _currentUserService?.UserId ?? "N/A";
                        entry.Entity.ModifiedOn = now;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}