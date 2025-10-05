using Application.Statics.Configurations;
using Domain.Entities;
using Domain.Entities.Common;
using Infrastructure.DbContexts.ModelConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DbContexts
{
    public partial class ApplicationDbContext : DbContext
    {
        private readonly string UserName;

        private const string UserAt = nameof(BaseEntity.CreatedBy);
        private const string CreationAt = nameof(BaseEntity.CreatedAt);
        private const string UpdateUser = nameof(BaseEntity.UpdatedBy);
        private const string UpdatedAt = nameof(BaseEntity.UpdatedAt);
        private const string IsActive = nameof(BaseEntity.IsActive);
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            UserName = "System";
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (base.Database.IsRelational())
            {
                base.Database.SetCommandTimeout(TimeSpan.FromMinutes(DbSettings.TimeoutInMinutes));
            }

        }

        public virtual DbSet<VersionControl> VersionControl { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEntitiesConfiguration();
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        HandleModifiedState(entry);
                        break;
                    case EntityState.Added:
                        HandleAddedState(entry);
                        break;
                }
            }
        }

        private void HandleModifiedState(EntityEntry entry)
        {
            entry.Property(UserAt).IsModified = false;
            entry.Property(CreationAt).IsModified = false;
            entry.CurrentValues[UpdateUser] = GetUserNameOrDefault(entry, UpdateUser);
            entry.CurrentValues[UpdatedAt] = DateTime.UtcNow;
        }

        private void HandleAddedState(EntityEntry entry)
        {
            entry.CurrentValues[UserAt] = GetUserNameOrDefault(entry, UserAt);
            entry.CurrentValues[UpdateUser] = GetUserNameOrDefault(entry, UpdateUser);
            entry.CurrentValues[CreationAt] = GetCreationDateOrDefault(entry);
            entry.CurrentValues[UpdatedAt] = DateTime.UtcNow;
            entry.CurrentValues[IsActive] = true;
        }

        private string GetUserNameOrDefault(EntityEntry entry, string propertyName)
        {
            return entry.CurrentValues[propertyName] is null || entry.CurrentValues[propertyName]?.ToString()?.Length <= 0
                ? UserName
                : entry.CurrentValues[propertyName]?.ToString()!;
        }

        private static DateTime GetCreationDateOrDefault(EntityEntry entry)
        {
            DateTime def = default;
            return entry.CurrentValues[CreationAt] is null || (DateTime)entry.CurrentValues[CreationAt]! == def
                ? DateTime.Now
                : (DateTime)entry.CurrentValues[CreationAt]!;
        }
    }
}
