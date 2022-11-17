using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MY.IDP.DataLayer.Configuration;
using MY.IDP.DomainClass;
using MY.IDP.Services;

namespace MY.IDP.DataLayer.Context
{
    public class ApplicationDbContext:DbContext,IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
        }
        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<UserClaim> UserClaims { set; get; }
        public virtual DbSet<UserLogin> UserLogins { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        }
    }
    
}

