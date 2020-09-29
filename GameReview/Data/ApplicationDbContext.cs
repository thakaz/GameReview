using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameReview.Models;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Threading;

namespace GameReview.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<GameReview.Models.Game> Game { get; set; }
        public DbSet<GameReview.Models.Reviewer> Reviewer { get; set; }
        public DbSet<GameReview.Models.Review> Review { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        private static Dictionary<Type, bool> _entityHasCreatedAtDic = new Dictionary<Type, bool>();
        private static Dictionary<Type, bool> _entityHasUpdatedAtDic = new Dictionary<Type, bool>();


    }
}
