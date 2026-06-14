using DemoApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasKey(x => x.Id);

            builder.Entity<User>()
                .Property(x => x.Name)
                .HasMaxLength(100);

            builder.Entity<User>()
                .Property(x => x.City)
                .HasMaxLength(100);

            builder.Entity<User>()
                .Property(x => x.State)
                .HasMaxLength(100);

            builder.Entity<User>()
                .Property(x => x.Pincode)
                .HasMaxLength(10);
        }
    }
}