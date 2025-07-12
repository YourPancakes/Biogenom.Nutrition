using Biogenom.Nutrition.Domain.Entities;
using Biogenom.Nutrition.Domain.Enums;

namespace Biogenom.Nutrition.Persistence.Context;

public static class DbSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedNutrientsAsync(context);
        await SeedSupplementsAsync(context);
    }

    private static async Task SeedNutrientsAsync(ApplicationDbContext context)
    {
        if (!context.Nutrients.Any())
        {
            var nutrients = new List<Nutrient>
            {
                new() { Name = "Vitamin D", Unit = "mcg", DailyNormMin = 10, DailyNormMax = 20 },
                new() { Name = "Omega-3", Unit = "g", DailyNormMin = 1, DailyNormMax = 2 },
                new() { Name = "Vitamin C", Unit = "mg", DailyNormMin = 75, DailyNormMax = 90 },
                new() { Name = "Calcium", Unit = "mg", DailyNormMin = 1000, DailyNormMax = 1200 },
                new() { Name = "Iron", Unit = "mg", DailyNormMin = 8, DailyNormMax = 18 },
                new() { Name = "Magnesium", Unit = "mg", DailyNormMin = 310, DailyNormMax = 420 },
                new() { Name = "Zinc", Unit = "mg", DailyNormMin = 8, DailyNormMax = 11 },
                new() { Name = "Vitamin B12", Unit = "mcg", DailyNormMin = 2.4m, DailyNormMax = 2.8m }
            };

            context.Nutrients.AddRange(nutrients);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedSupplementsAsync(ApplicationDbContext context)
    {
        if (!context.Supplements.Any())
        {
            var supplements = new List<Supplement>
            {
                new() 
                { 
                    Name = "Vitamin D3", 
                    Description = "Supports immune system and bone health", 
                    ImageUrl = "/images/vitamin-d3.jpg", 
                    Dosage = "1000 IU daily", 
                    WhenToTake = "Morning with meals" 
                },
                new() 
                { 
                    Name = "Omega-3", 
                    Description = "Supports heart and brain health", 
                    ImageUrl = "/images/omega-3.jpg", 
                    Dosage = "1000 mg daily", 
                    WhenToTake = "With meals" 
                },
                new() 
                { 
                    Name = "Vitamin C", 
                    Description = "Strengthens immunity and antioxidant protection", 
                    ImageUrl = "/images/vitamin-c.jpg", 
                    Dosage = "500 mg daily", 
                    WhenToTake = "Morning or afternoon" 
                },
                new() 
                { 
                    Name = "Calcium + D3", 
                    Description = "Supports bone and dental health", 
                    ImageUrl = "/images/calcium.jpg", 
                    Dosage = "1000 mg daily", 
                    WhenToTake = "Evening with meals" 
                },
                new() 
                { 
                    Name = "Magnesium", 
                    Description = "Supports nervous system and sleep quality", 
                    ImageUrl = "/images/magnesium.jpg", 
                    Dosage = "400 mg daily", 
                    WhenToTake = "Evening before bed" 
                },
                new() 
                { 
                    Name = "Zinc", 
                    Description = "Supports immunity and muscle recovery", 
                    ImageUrl = "/images/zinc.jpg", 
                    Dosage = "15 mg daily", 
                    WhenToTake = "Morning on empty stomach" 
                },
                new() 
                { 
                    Name = "Probiotic", 
                    Description = "Supports gut health", 
                    ImageUrl = "/images/probiotic.jpg", 
                    Dosage = "1 capsule daily", 
                    WhenToTake = "Morning on empty stomach" 
                }
            };

            context.Supplements.AddRange(supplements);
            await context.SaveChangesAsync();
        }
    }
} 