using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameReview.Models;

namespace GameReview.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GameReview.Models.Game> Game { get; set; }
        public DbSet<GameReview.Models.Reviewer> Reviewer { get; set; }
        public DbSet<GameReview.Models.Review> Review { get; set; }
    }
}
