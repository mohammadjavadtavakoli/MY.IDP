using Microsoft.EntityFrameworkCore;
using MY.IDP.DataLayer.Configuration;
using MY.IDP.DataLayer.Context;
using MY.IDP.DomainClass;

namespace MY.IDP.DataLayer
{
    public class MyApplicationDbContext : DbContext, IUnitOfWork
    {
        public MyApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

         public virtual DbSet<User> User { get; set; }
         
         public virtual DbSet<UserClaim> UserClaims { get; set; }
         public virtual DbSet<UserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.ApplyConfiguration(new UserConfiguration());
            // modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
        }
    }
}