using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shift_System.Domain.Common;
using Shift_System.Domain.Common.Interfaces;
using Shift_System.Domain.Entities;
using System.Reflection;

namespace Shift_System.Persistence.Contexts
{
   public class ApplicationDbContext : IdentityDbContext<AppUser>
   {
      private readonly IDomainEventDispatcher _dispatcher;

      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher dispatcher)
          : base(options)
      {
         _dispatcher = dispatcher;
      }


      public DbSet<Employee> Employees => Set<Employee>();
      public DbSet<Team> Teams => Set<Team>();
      public DbSet<ShiftList> ShiftLists => Set<ShiftList>();
      public DbSet<AssignList> AssignLists => Set<AssignList>();
      public DbSet<TeamEmployee> TeamEmployees => Set<TeamEmployee>();


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

         modelBuilder.Entity<AssignList>(entity =>
         {
            entity.Property(e => e.ShiftId).HasColumnName("Shift_Id");

            entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            entity.HasOne(d => d.Shift)
                .WithMany(p => p._Assignments)
                .HasForeignKey(d => d.ShiftId)
                .HasConstraintName("FK_Assignments_Shifts");

            entity.HasOne(d => d.Team)
                .WithMany(p => p._Assignments)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Assignments_Teams");
         });

         modelBuilder.Entity<Employee>(entity =>
         {
            entity.Property(e => e.Address).IsRequired();

            entity.Property(e => e.Mail).IsRequired();

            entity.Property(e => e.Name).IsRequired();

            entity.Property(e => e.Phone).IsRequired();

            entity.Property(e => e.SurName).IsRequired();

            entity.Property(e => e.Title).IsRequired();

            entity.Property(e => e.UserName).IsRequired();
         });

         modelBuilder.Entity<ShiftList>(entity =>
         {
            entity.Property(e => e.Shift_Name)
                .IsRequired()
                .HasColumnName("Shift_Name");
         });

         modelBuilder.Entity<Team>(entity =>
         {
            entity.Property(e => e.TeamName).IsRequired();
         });

         modelBuilder.Entity<TeamEmployee>(entity =>
         {
            entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

            entity.Property(e => e.IsLeader).HasColumnName("isLeader");

            entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p._TeamEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_TeamEmployees_Employees");

            entity.HasOne(d => d.Team)
                .WithMany(p => p._TeamEmployees)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_TeamEmployees_Teams");
         });


         base.OnModelCreating(modelBuilder);
         modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      }

      public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
      {
         int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         // ignore events if no dispatcher provided
         if (_dispatcher == null) return result;

         // dispatch events only if save was successful
         var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
             .Select(e => e.Entity)
             .Where(e => e.DomainEvents.Any())
             .ToArray();

         await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

         return result;
      }

      public override int SaveChanges()
      {
         return SaveChangesAsync().GetAwaiter().GetResult();
      }
   }
}
