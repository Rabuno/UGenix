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
        // 1. Seed Users
        if (!await context.Users.AnyAsync(u => u.Email == "Ahihi@gmail.com"))
        {
            var admin = User.Create(
                "Ahihi@gmail.com",
                passwordHasher.HashPassword("Admin@123"),
                Roles.Admin);
            
            context.Users.Add(admin);
        }

        if (!await context.Users.AnyAsync(u => u.Email == "user@gmail.com"))
        {
            var customer = User.Create(
                "user@gmail.com",
                passwordHasher.HashPassword("User@123"),
                Roles.Customer);
            
            context.Users.Add(customer);
        }

        // 2. Seed Restaurants
        if (!await context.Restaurants.AnyAsync())
        {
            var restaurants = new List<Restaurant>
            {
                new() {
                    Id = Guid.NewGuid(),
                    Name = "The Gourmet Kitchen",
                    Description = "A fine dining experience with exquisite flavors.",
                    Address = "123 Foodie St, District 1, HCMC",
                    Location = new Point(106.6948, 10.7765) { SRID = 4326 },
                    ThumbnailUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4",
                    AverageRating = 4.8,
                    ReviewCount = 5,
                    MerchantId = Guid.NewGuid()
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Street Food Haven",
                    Description = "The best street food in the city.",
                    Address = "456 Hawker Ln, District 3, HCMC",
                    Location = new Point(106.6821, 10.7812) { SRID = 4326 },
                    ThumbnailUrl = "https://images.unsplash.com/photo-1552566626-52f8b828add9",
                    AverageRating = 4.2,
                    ReviewCount = 0,
                    MerchantId = Guid.NewGuid()
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Saigon Sushi Bar",
                    Description = "Fresh sushi and sashimi in the heart of Saigon.",
                    Address = "78 Le Thanh Ton, District 1, HCMC",
                    Location = new Point(106.7011, 10.7785) { SRID = 4326 },
                    ThumbnailUrl = "https://images.unsplash.com/photo-1579871494447-9811cf80d66c",
                    AverageRating = 4.5,
                    ReviewCount = 0,
                    MerchantId = Guid.NewGuid()
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Pasta Paradise",
                    Description = "Authentic Italian pasta made with love.",
                    Address = "12 Vo Van Tan, District 3, HCMC",
                    Location = new Point(106.6890, 10.7740) { SRID = 4326 },
                    ThumbnailUrl = "https://images.unsplash.com/photo-1473093226795-af9932fe5856",
                    AverageRating = 4.6,
                    ReviewCount = 0,
                    MerchantId = Guid.NewGuid()
                },
                new() {
                    Id = Guid.NewGuid(),
                    Name = "Banh Mi Master",
                    Description = "Crispy baguettes with premium fillings.",
                    Address = "200 Nguyen Trai, District 1, HCMC",
                    Location = new Point(106.6885, 10.7680) { SRID = 4326 },
                    ThumbnailUrl = "https://images.unsplash.com/photo-1601050690597-df0568f70950",
                    AverageRating = 4.9,
                    ReviewCount = 0,
                    MerchantId = Guid.NewGuid()
                }
            };

            context.Restaurants.AddRange(restaurants);

            // 3. Seed Vouchers
            foreach (var restaurant in restaurants)
            {
                var voucher = Voucher.Create(
                    restaurant.Id,
                    $"{restaurant.Name.Replace(" ", "").ToUpper().Take(5)}50",
                    100000,
                    50000,
                    50,
                    DateTime.UtcNow.AddMonths(1));
                
                context.Vouchers.Add(voucher);
            }

            // 4. Seed Reviews for "The Gourmet Kitchen"
            var user = await context.Users.FirstAsync(u => u.Email == "user@gmail.com");
            var targetRestaurant = restaurants[0];
            
            var reviews = new List<Review>
            {
                Review.Create(targetRestaurant.Id, user.Id, 5, "Incredible food and service!", "Mozilla/5.0", "127.0.0.1"),
                Review.Create(targetRestaurant.Id, user.Id, 4, "Great atmosphere, but a bit pricey.", "Mozilla/5.0", "127.0.0.1"),
                Review.Create(targetRestaurant.Id, user.Id, 5, "Best steak in town.", "Mozilla/5.0", "127.0.0.1"),
                Review.Create(targetRestaurant.Id, user.Id, 5, "Highly recommend the tasting menu.", "Mozilla/5.0", "127.0.0.1"),
                Review.Create(targetRestaurant.Id, user.Id, 5, "Faultless experience.", "Mozilla/5.0", "127.0.0.1")
            };

            context.Reviews.AddRange(reviews);
        }

        await context.SaveChangesAsync();
    }
}
