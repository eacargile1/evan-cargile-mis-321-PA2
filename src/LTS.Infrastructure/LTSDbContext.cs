using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public class LTSDbContext : DbContext
{
    public LTSDbContext(DbContextOptions<LTSDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TrainerProfile> TrainerProfiles => Set<TrainerProfile>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Offering> Offerings => Set<Offering>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageBooking> PackageBookings => Set<PackageBooking>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
        });

        mb.Entity<TrainerProfile>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User)
                .WithOne(x => x.TrainerProfile)
                .HasForeignKey<TrainerProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        mb.Entity<Pet>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Owner)
                .WithMany(x => x.Pets)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        mb.Entity<Offering>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Price).HasColumnType("decimal(10,2)");
            e.HasOne(x => x.TrainerProfile)
                .WithMany(x => x.Offerings)
                .HasForeignKey(x => x.TrainerProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        mb.Entity<Booking>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Offering)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.OfferingId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Pet)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.PetId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        mb.Entity<Review>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.BookingId).IsUnique();
            e.HasOne(x => x.TrainerProfile)
                .WithMany(x => x.Reviews)
                .HasForeignKey(x => x.TrainerProfileId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Booking)
                .WithOne(x => x.Review)
                .HasForeignKey<Review>(x => x.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        mb.Entity<Package>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.TotalPrice).HasColumnType("decimal(10,2)");
            e.HasOne(x => x.TrainerProfile)
                .WithMany(x => x.Packages)
                .HasForeignKey(x => x.TrainerProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        mb.Entity<PackageBooking>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Package)
                .WithMany(x => x.PackageBookings)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Pet)
                .WithMany()
                .HasForeignKey(x => x.PetId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
