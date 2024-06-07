using Microsoft.EntityFrameworkCore;
using DapperProject.Models;

namespace MobileDAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Mobiles> Mobiles { get; set; }

        // Add other DbSet properties for your entities

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Demo;Integrated Security=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mobiles>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.slno).HasColumnName("slno");
                entity.Property(e => e.Quantity).HasColumnName("Quantity");
                entity.Property(e => e.Price).HasColumnName("Price");
                entity.Property(e => e.Ordertime).HasColumnType("datetime");

                //entity.HasOne(d => d.Customer).WithMany(p => p.AgriculturalOrders)
                //    .HasForeignKey(d => d.CustomerID)
                //    .HasConstraintName("FK_Agricultural_Order_Customer");

                //entity.HasOne(d => d.Employee).WithMany(p => p.AgriculturalOrders)
                //    .HasForeignKey(d => d.EmployeeID)
                //    .OnDelete(DeleteBehavior.SetNull)
                //    .HasConstraintName("FK_Agricultural_Order_Employee");
            });
        }
        }
}
