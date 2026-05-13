using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using UGenix.Domain.Entities;
using UGenix.Shared.Abstractions;
using UGenix.Shared.Constants;

namespace UGenix.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        // 1. Seed Admin User
        if (!await context.Users.AnyAsync(u => u.Email == "Ahihi@gmail.com"))
        {
            var admin = User.Create(
                "Ahihi@gmail.com",
                passwordHasher.HashPassword("Admin@123"),
                Roles.Admin);
            
            context.Users.Add(admin);
        }

        // 2. Seed some sample data if needed (e.g. Restaurants)
        if (!await context.Restaurants.AnyAsync())
        {
            var restaurant1 = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "The Gourmet Kitchen",
                Description = "A fine dining experience with exquisite flavors.",
                Address = "123 Foodie St, Flavor Town",
                Location = new Point(106.660172, 10.762622) { SRID = 4326 }, // Example coordinates for Saigon
                ThumbnailUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4",
                AverageRating = 4.5,
                ReviewCount = 10,
                MerchantId = Guid.NewGuid()
            };

            var restaurant2 = new Restaurant
            {
                Id = Guid.NewGuid(),
                Name = "Street Food Haven",
                Description = "The best street food in the city.",
                Address = "456 Hawker Ln, Spice City",
                Location = new Point(106.700172, 10.772622) { SRID = 4326 },
                ThumbnailUrl = "https://images.unsplash.com/photo-1552566626-52f8b828add9",
                AverageRating = 4.2,
                ReviewCount = 25,
                MerchantId = Guid.NewGuid()
            };

            context.Restaurants.AddRange(restaurant1, restaurant2);

            // 3. Seed Vouchers for these restaurants
            var voucher1 = Voucher.Create(
                restaurant1.Id,
                "GOURMET50",
                100000,
                50000,
                100,
                DateTime.UtcNow.AddMonths(1));

            var voucher2 = Voucher.Create(
                restaurant2.Id,
                "STREET20",
                50000,
                10000,
                200,
                DateTime.UtcNow.AddMonths(2));

            context.Vouchers.AddRange(voucher1, voucher2);
        }

        await context.SaveChangesAsync();
    }
}
