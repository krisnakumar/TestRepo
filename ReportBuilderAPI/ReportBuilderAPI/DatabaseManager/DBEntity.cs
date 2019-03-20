using DataInterface.Database;
using Microsoft.EntityFrameworkCore;
namespace OnBoardLMS.WebAPI.Models
{
    public partial class DBEntity : DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
           => optionsBuilder.UseSqlServer(DatabaseWrapper._connectionString);


      
        /// <summary>
        /// 
        /// </summary>
        public  DbSet<Role> Role { get; set; }
       
        
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().ToTable("Role");
            base.OnModelCreating(modelBuilder);
        }        
    }
}
