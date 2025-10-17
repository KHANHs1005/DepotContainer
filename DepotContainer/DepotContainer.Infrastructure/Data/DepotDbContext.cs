using Microsoft.EntityFrameworkCore;
using DepotContainer.Domain.Entities;
using DepotContainer.Domain.Enums;

namespace DepotContainer.Infrastructure.Data
{
    public class DepotDbContext : DbContext
    {
        public DepotDbContext(DbContextOptions<DepotDbContext> options)
            : base(options)
        {
        }

        // === DbSet cho toàn bộ entity trong hệ thống ===
        public DbSet<Container> Containers { get; set; } 
        public DbSet<EIR> Eirs { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Áp dụng tất cả file cấu hình (nếu có)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepotDbContext).Assembly);

            // Enum conversion cho StaffType (EF Core sẽ lưu enum dưới dạng string)
            modelBuilder.Entity<Staff>().Property(s => s.StaffType).HasConversion<string>();
            modelBuilder.Entity<EIR>().Property(e => e.Type).HasConversion<string>();

            // Optional: ràng buộc unique, length...
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.StaffName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.ToTable("Staff"); // mapping tới bảng có sẵn trong SQL
                entity.Property(e => e.StaffName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);
            });
        }

    }
}
