using Microsoft.EntityFrameworkCore;
using reservas.api.Models;

namespace reservas.api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReservaModel> Reservas { get; set; }
        public DbSet<UserModel> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source= DB_Reservas;");

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReservaModel>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservas) 
                .HasForeignKey(r => r.IdUser);
        }
    }
}
