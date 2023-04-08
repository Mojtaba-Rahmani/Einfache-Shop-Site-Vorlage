using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Walet;

namespace TopLearn.DataLayer.Context
{
   public class TopLearnContext:DbContext
    {

        public TopLearnContext(DbContextOptions<TopLearnContext> options):base(options)
        {
            
        }

        #region User

        public DbSet<Roles> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        #endregion

        #region Walet
        public DbSet<Walet> Wallets { get; set; }
        public DbSet<WaletType> WaletTypes { get; set; }
        #endregion

        // query Filter
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);
            base.OnModelCreating(modelBuilder);
        }
    }

    
}
