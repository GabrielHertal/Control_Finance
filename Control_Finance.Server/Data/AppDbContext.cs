using Control_Finance.Server.Enums;
using Control_Finance.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Control_Finance.Server.Data
{
    public class AppDbContext : IdentityDbContext <AppUsers, IdentityRole<int>,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Lancamentos> Lancamentos { get; set; }
        public DbSet<Contas> Contas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUsers>()
                .HasIndex(u => u.Email)
                .IsUnique(true);
            builder.Entity<Categorias>()
                .HasIndex(c => new { c.Titulo })
                .IsUnique(true);
        }
    }
}