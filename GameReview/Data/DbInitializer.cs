using GameReview.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameReview.Data
{
    public class DbInitializer
    {
        public static async Task SeedingAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();
            if( await context.Reviewer.AnyAsync())
            {
                return;
            }

            await context.Reviewer.AddAsync(
                new Reviewer { Name = "test" }
                );

            await context.SaveChangesAsync();
        }

    }
}
