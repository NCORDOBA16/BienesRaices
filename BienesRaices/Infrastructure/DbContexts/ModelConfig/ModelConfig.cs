using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts.ModelConfig
{
    public static class ModelsConfig
    {
        public static void ApplyEntitiesConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VersionControl>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("VERSION_CONTROL");

                entity.Property(e => e.CurrentVersion).HasMaxLength(50);
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.IdOwner);
                entity.Property(e => e.IdOwner).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Address).HasMaxLength(300);
                entity.Property(e => e.Photo);
                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.IdProperty);
                entity.Property(e => e.IdProperty).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CodeInternal).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.CodeInternal).IsUnique();

                entity.Property(e => e.Year);
                entity.Property(e => e.IdOwner).IsRequired();

                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Properties)
                      .HasForeignKey(e => e.IdOwner)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.IdPropertyImage);
                entity.Property(e => e.IdPropertyImage).ValueGeneratedOnAdd();
                entity.Property(e => e.File).HasMaxLength(500);
                entity.Property(e => e.Enabled).IsRequired();

                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyImages)
                      .HasForeignKey(e => e.IdProperty)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PropertyTrace>(entity =>
            {
                entity.HasKey(e => e.IdPropertyTrace);
                entity.Property(e => e.IdPropertyTrace).ValueGeneratedOnAdd();

                entity.Property(e => e.DateSale).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Tax).HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime2");
                entity.Property(e => e.UpdatedBy).HasMaxLength(100);
                entity.Property(e => e.IsActive).IsRequired();

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyTraces)
                      .HasForeignKey(e => e.IdProperty)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
