using LocumInvoiceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace LocumInvoiceTracker.Data;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Hospital> Hospitals => Set<Hospital>();

    public DbSet<WorkShift> WorkShifts => Set<WorkShift>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hospital>()
            .HasMany(hospital => hospital.WorkShifts)
            .WithOne(shift => shift.Hospital)
            .HasForeignKey(shift => shift.HospitalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkShift>()
            .Property(shift => shift.HourlyRate)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WorkShift>()
            .Property(shift => shift.HoursWorked)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Hospital>().HasData(
            new Hospital
            {
                Id = 1,
                Name = "Nova Vet",
                Province = "AB"
            },
            new Hospital
            {
                Id = 2,
                Name = "Calgary Animal Hospital",
                Province = "AB"
            }
        );
    }
}