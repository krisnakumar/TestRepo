﻿using DataInterface.Database;
using Microsoft.EntityFrameworkCore;
namespace OnBoardLMS.WebAPI.Models
{
    public partial class DBEntity : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlServer(DatabaseWrapper._connectionString);
      
        /// <summary>
        /// DBset for to access the 
        /// 
        /// </summary>
        public  DbSet<Role> Role { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<UserCompany> UserCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Role");

            modelBuilder.Entity<Company>().ToTable("Company");

            modelBuilder.Entity<UserCompany>().ToTable("UserCompany")
                .HasKey(uc=> new { uc.CompanyId, uc.UserId });

            base.OnModelCreating(modelBuilder);

        }        
    }
}
