using Microsoft.EntityFrameworkCore;
using WalkProject_WebApi.Models;
using WalkProject_WebApi.Models.Domain;

namespace WalkProject_WebApi.Data
{
    public class WalkProjectDbContext:DbContext
    {
        public WalkProjectDbContext(DbContextOptions<WalkProjectDbContext> dbContextOptions):base (dbContextOptions) 
        {               
        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
