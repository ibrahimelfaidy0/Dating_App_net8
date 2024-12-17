
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AppDbcontext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser>? Users { get; set; }
        
    }
}