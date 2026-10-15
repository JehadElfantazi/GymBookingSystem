using GymBookingSystem.Application.Interfaces;
using GymBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymBookingSystem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<ClassSchedule> ClassSchedules => Set<ClassSchedule>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GymClass>()
            .HasOne(g => g.Instructor)
            .WithMany(i => i.GymClasses)
            .HasForeignKey(g => g.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.GymClass)
            .WithMany(gc => gc.ClassSchedules)
            .HasForeignKey(cs => cs.GymClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Member)
            .WithMany(m => m.Bookings)
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.ClassSchedule)
            .WithMany(cs => cs.Bookings)
            .HasForeignKey(b => b.ClassScheduleId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.LastName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(m => m.Email).IsUnique();
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.Property(i => i.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(i => i.LastName).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(i => i.Email).IsUnique();
        });

        modelBuilder.Entity<GymClass>(entity =>
        {
            entity.Property(gc => gc.Name).IsRequired().HasMaxLength(200);
            entity.Property(gc => gc.Description).HasMaxLength(1000);
            entity.Property(gc => gc.Capacity).IsRequired();
        });

        modelBuilder.Entity<ClassSchedule>(entity =>
        {
            entity.Property(cs => cs.StartTime).IsRequired();
            entity.Property(cs => cs.EndTime).IsRequired();
            entity.Property(cs => cs.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(b => b.BookedAt).IsRequired();
            entity.HasIndex(b => new { b.MemberId, b.ClassScheduleId }).IsUnique();
        });
    }


}