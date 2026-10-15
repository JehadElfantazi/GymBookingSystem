using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GymBookingSystem.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Member> Members { get;  }
        DbSet<Domain.Entities.Instructor> Instructors { get;  }
        DbSet<Domain.Entities.GymClass> GymClasses { get;  }
        DbSet<Domain.Entities.ClassSchedule> ClassSchedules { get; }
        DbSet<Domain.Entities.Booking> Bookings { get; }
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;


        // method to save changes asynchronously with cancellation token
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}
