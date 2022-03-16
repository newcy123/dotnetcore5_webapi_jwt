using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MYSQLStoreAPI.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>{

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){

        }

        public DbSet<Category> category {get; set;}

        public DbSet<Products> products {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        
    }
    
        
    
}