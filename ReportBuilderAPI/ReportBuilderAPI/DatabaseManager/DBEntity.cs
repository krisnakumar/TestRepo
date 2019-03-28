using Amazon.Lambda.Core;
using DataInterface.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace OnBoardLMS.WebAPI.Models
{
    public partial class DBEntity : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DatabaseWrapper._connectionString);
        }

        /// <summary>
        /// DBset for to access the 
        /// 
        /// </summary>
        public DbSet<Role> Role { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<UserCompany> UserCompany { get; set; }

        public DbSet<CompanyClient> CompanyClient { get; set; }

        public DbSet<User> User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<Role>().ToTable("Role");

                modelBuilder.Entity<Company>().ToTable("Company");

                modelBuilder.Entity<UserCompany>().ToTable("UserCompany")
                    .HasKey(uc => new { uc.CompanyId, uc.UserId });


                modelBuilder.Entity<CompanyClient>().ToTable("CompanyClient")
                   .HasKey(cc => new { cc.OwnerCompany, cc.ClientCompany });

                modelBuilder.Entity<User>().ToTable("User");


                base.OnModelCreating(modelBuilder);
            }
            catch(Exception modelException)
            {
                LambdaLogger.Log(modelException.ToString());
            }        
        }
    }
}
