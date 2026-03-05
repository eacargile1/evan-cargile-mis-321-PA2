using LTS.Core;
using Microsoft.EntityFrameworkCore;

namespace LTS.Infrastructure;

public static class DataSeeder
{
    private const string PlaceholderHash = "$2a$11$placeholder.hash.for.dev.only";

    public static async Task SeedAsync(LTSDbContext db)
    {
        if (await db.Users.AnyAsync()) return;

        // Users
        var admin = new User { Email = "admin@lts.local", PasswordHash = PlaceholderHash, Role = UserRole.Admin, FirstName = "Admin", LastName = "LTS", Phone = "205-555-0001", Address = "1 Training Blvd, Tuscaloosa, AL" };
        var janeUser = new User { Email = "jane@lts.local", PasswordHash = PlaceholderHash, Role = UserRole.Trainer, FirstName = "Jane", LastName = "Doe", Phone = "205-555-0100", Address = "42 Paw Lane, Tuscaloosa, AL" };
        var johnUser = new User { Email = "john@lts.local", PasswordHash = PlaceholderHash, Role = UserRole.Trainer, FirstName = "John", LastName = "Smith", Phone = "205-555-0200", Address = "7 Agility Ave, Tuscaloosa, AL" };
        var owner1 = new User { Email = "sarah@example.com", PasswordHash = PlaceholderHash, Role = UserRole.PetOwner, FirstName = "Sarah", LastName = "Johnson", Phone = "205-555-1001", Address = "123 Oak St, Tuscaloosa, AL" };
        var owner2 = new User { Email = "mike@example.com", PasswordHash = PlaceholderHash, Role = UserRole.PetOwner, FirstName = "Mike", LastName = "Williams", Phone = "205-555-1002", Address = "456 Elm Dr, Tuscaloosa, AL" };

        db.Users.AddRange(admin, janeUser, johnUser, owner1, owner2);
        await db.SaveChangesAsync();

        // Trainer profiles
        var janeProfile = new TrainerProfile
        {
            UserId = janeUser.Id,
            DisplayName = "Jane Doe",
            Bio = "Certified dog trainer with 10+ years experience in obedience, puppy training, and behavior modification.",
            Specialties = "Puppy, Obedience, Behavior",
            BreedSpecialties = "Labrador,Golden Retriever,Beagle,Mixed Breed"
        };
        var johnProfile = new TrainerProfile
        {
            UserId = johnUser.Id,
            DisplayName = "John Smith",
            Bio = "Agility and advanced obedience specialist. Former competitive handler with AKC titles.",
            Specialties = "Agility, Advanced Obedience, Competition",
            BreedSpecialties = "Border Collie,Australian Shepherd,German Shepherd,Belgian Malinois"
        };
        db.TrainerProfiles.AddRange(janeProfile, johnProfile);
        await db.SaveChangesAsync();

        // Pets
        var pets = new List<Pet>
        {
            new() { OwnerId = owner1.Id, Name = "Biscuit", Breed = "Labrador", IsNeutered = true, IsSpayed = false },
            new() { OwnerId = owner1.Id, Name = "Peanut", Breed = "Beagle", IsNeutered = false, IsSpayed = false },
            new() { OwnerId = owner2.Id, Name = "Shadow", Breed = "German Shepherd", IsNeutered = false, IsSpayed = false },
        };
        db.Pets.AddRange(pets);
        await db.SaveChangesAsync();

        // Offerings with realistic times
        var baseDate = DateTime.UtcNow.Date.AddDays(1 - (int)DateTime.UtcNow.DayOfWeek + 1); // Next Monday
        var offerings = new List<Offering>
        {
            // Classes - Jane
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Class, Title = "Puppy Basics (Morning)", StartDateTime = baseDate.AddHours(9), EndDateTime = baseDate.AddHours(10), Capacity = 8, MinEnrollment = 3, Price = 45m, Location = "LTS Training Center" },
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Class, Title = "Obedience 101", StartDateTime = baseDate.AddHours(18), EndDateTime = baseDate.AddHours(19), Capacity = 8, MinEnrollment = 3, Price = 50m, Location = "LTS Training Center" },
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Class, Title = "Puppy Basics (Evening)", StartDateTime = baseDate.AddDays(2).AddHours(17.5), EndDateTime = baseDate.AddDays(2).AddHours(18.5), Capacity = 8, MinEnrollment = 3, Price = 45m, Location = "LTS Training Center" },
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Class, Title = "Obedience Level 2", StartDateTime = baseDate.AddDays(5).AddHours(9), EndDateTime = baseDate.AddDays(5).AddHours(10), Capacity = 6, MinEnrollment = 2, Price = 55m, Location = "LTS Training Center" },

            // Classes - John
            new() { TrainerProfileId = johnProfile.Id, Type = OfferingType.Class, Title = "Agility Intro", StartDateTime = baseDate.AddDays(1).AddHours(9), EndDateTime = baseDate.AddDays(1).AddHours(10.5), Capacity = 6, MinEnrollment = 3, Price = 65m, Location = "LTS Agility Field", AllowedBreeds = "Border Collie,Australian Shepherd,German Shepherd,Belgian Malinois" },
            new() { TrainerProfileId = johnProfile.Id, Type = OfferingType.Class, Title = "Advanced Obedience", StartDateTime = baseDate.AddDays(3).AddHours(19), EndDateTime = baseDate.AddDays(3).AddHours(20), Capacity = 6, MinEnrollment = 2, Price = 60m, Location = "LTS Training Center" },
            new() { TrainerProfileId = johnProfile.Id, Type = OfferingType.Class, Title = "Saturday Agility", StartDateTime = baseDate.AddDays(5).AddHours(7), EndDateTime = baseDate.AddDays(5).AddHours(8.5), Capacity = 8, MinEnrollment = 3, Price = 65m, Location = "LTS Agility Field" },

            // Individual - Jane
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Individual, Title = "One-on-One Session", StartDateTime = baseDate.AddDays(1).AddHours(14), EndDateTime = baseDate.AddDays(1).AddHours(15), Capacity = 1, MinEnrollment = 1, Price = 85m },
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Individual, Title = "One-on-One Session", StartDateTime = baseDate.AddDays(2).AddHours(12), EndDateTime = baseDate.AddDays(2).AddHours(13), Capacity = 1, MinEnrollment = 1, Price = 85m },
            new() { TrainerProfileId = janeProfile.Id, Type = OfferingType.Individual, Title = "Behavior Consultation", StartDateTime = baseDate.AddDays(4).AddHours(10), EndDateTime = baseDate.AddDays(4).AddHours(11), Capacity = 1, MinEnrollment = 1, Price = 100m },

            // Individual - John
            new() { TrainerProfileId = johnProfile.Id, Type = OfferingType.Individual, Title = "Agility Private", StartDateTime = baseDate.AddDays(2).AddHours(7), EndDateTime = baseDate.AddDays(2).AddHours(8), Capacity = 1, MinEnrollment = 1, Price = 90m },
            new() { TrainerProfileId = johnProfile.Id, Type = OfferingType.Individual, Title = "Agility Private", StartDateTime = baseDate.AddDays(4).AddHours(17), EndDateTime = baseDate.AddDays(4).AddHours(18), Capacity = 1, MinEnrollment = 1, Price = 90m },
        };
        db.Offerings.AddRange(offerings);
        await db.SaveChangesAsync();

        // Packages
        var packages = new List<Package>
        {
            new() { TrainerProfileId = janeProfile.Id, Title = "Puppy Starter Pack", Description = "5 group classes to get your puppy started right.", NumSessions = 5, TotalPrice = 200m, IsActive = true },
            new() { TrainerProfileId = janeProfile.Id, Title = "Private Training Bundle", Description = "4 one-on-one sessions at a discounted rate.", NumSessions = 4, TotalPrice = 300m, IsActive = true },
            new() { TrainerProfileId = johnProfile.Id, Title = "Agility Foundations", Description = "6 group agility classes for a solid foundation.", NumSessions = 6, TotalPrice = 340m, IsActive = true },
        };
        db.Packages.AddRange(packages);

        // Sample bookings on first few offerings
        var booking1 = new Booking { OfferingId = offerings[0].Id, CustomerName = "Sarah Johnson", CustomerEmail = "sarah@example.com", CustomerPhone = "205-555-1001", PetId = pets[0].Id, Status = BookingStatus.Confirmed };
        var booking2 = new Booking { OfferingId = offerings[0].Id, CustomerName = "Mike Williams", CustomerEmail = "mike@example.com", CustomerPhone = "205-555-1002", PetId = pets[2].Id, Status = BookingStatus.Confirmed };
        var booking3 = new Booking { OfferingId = offerings[7].Id, CustomerName = "Sarah Johnson", CustomerEmail = "sarah@example.com", CustomerPhone = "205-555-1001", PetId = pets[0].Id, Status = BookingStatus.Confirmed };
        db.Bookings.AddRange(booking1, booking2, booking3);

        await db.SaveChangesAsync();
    }
}
