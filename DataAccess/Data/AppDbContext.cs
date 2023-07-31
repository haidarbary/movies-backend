
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() 
        { 
            
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Movie>? Movies { get; set; }
    }

}
