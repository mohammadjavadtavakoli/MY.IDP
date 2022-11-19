﻿using Microsoft.EntityFrameworkCore;
using MY.WebApi.ImageGallery.DataLayer.Configuration;
using MY.WebApi.ImageGallery.DomainClasses;

namespace MY.WebApi.ImageGallery.DataLayer.Context

{
    public class ApplicationDbContext:DbContext,IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public virtual DbSet<Image> Images { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
            // modelBuilder.ApplyConfiguration(new ImageConfiguration());
        }
    }
}

