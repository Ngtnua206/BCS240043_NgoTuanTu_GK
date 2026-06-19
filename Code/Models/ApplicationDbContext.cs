using Microsoft.EntityFrameworkCore;

namespace BTKTGK_BCS240043_NgoTuanTu.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<RoomType_BCS240043> RoomTypes_BCS240043 { get; set; }
        public DbSet<Room_BCS240043> Rooms_BCS240043 { get; set; }
        public DbSet<RoomImage_BCS240043> RoomImages_BCS240043 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomType_BCS240043>(entity =>
            {
                entity.ToTable("RoomTypes_BCS240043");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();

                entity.HasMany(e => e.Rooms)
                      .WithOne(e => e.RoomType)
                      .HasForeignKey(e => e.RoomTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Room_BCS240043>(entity =>
            {
                entity.ToTable("Rooms_BCS240043");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Area).HasColumnType("decimal(18,2)");

                entity.HasIndex(e => new { e.Name, e.RoomTypeId }).IsUnique();

                entity.HasMany(e => e.RoomImages)
                      .WithOne(e => e.Room)
                      .HasForeignKey(e => e.RoomId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RoomImage_BCS240043>(entity =>
            {
                entity.ToTable("RoomImages_BCS240043");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired();
            });
        }
    }
}
