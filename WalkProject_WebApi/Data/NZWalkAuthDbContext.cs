using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WalkProject_WebApi.Data
{
    public class NZWalkAuthDbContext:IdentityDbContext
    {
        public NZWalkAuthDbContext(DbContextOptions<NZWalkAuthDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleID = "783dacb8-0083-47a8-8eff-f39e36eb46be";
            var WriterRoleID = "7ff697ea-83b3-4bec-8ece-3465f327fbf0";

            var rols = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleID,
                    ConcurrencyStamp = readerRoleID,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()
                },
                    new IdentityRole
                    {
                        Id = WriterRoleID,
                        ConcurrencyStamp = WriterRoleID,
                        Name = "Writer",
                        NormalizedName = "Writer".ToUpper()
                    }
            };

            builder.Entity<IdentityRole>().HasData(rols);

        }
    }
    
}
