using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shift_System.Domain.Entities;
using System.Reflection;

namespace Shift_System_UI.Models
{
    public class ApplicationDbContextWeb : IdentityDbContext<AppUserWeb, AppRoleWeb, int>
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MYPC\\SQLEXPRESS;Database=Shift_Db33;Trusted_Connection=true;TrustServerCertificate=True;Integrated_Security=True");
            //optionsBuilder.UseSqlServer("Server=MYPC\\SQLEXPRESS;Database=Shift_Db33;Trusted_Connection=true;Integrated_Security=true;");
        }


        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<ShiftList> ShiftLists => Set<ShiftList>();
        public DbSet<AssignList> AssignLists => Set<AssignList>();
        public DbSet<TeamEmployee> TeamEmployees => Set<TeamEmployee>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            //modelBuilder.Entity<AssignList>(entity =>
            //{
            //    entity.Property(e => e.ShiftId).HasColumnName("Shift_Id");

            //    entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            //    entity.HasOne(d => d.Shift)
            //        .WithMany(p => p._Assignments)
            //        .HasForeignKey(d => d.ShiftId)
            //        .HasConstraintName("FK_Assignments_Shifts");

            //    entity.HasOne(d => d.Team)
            //        .WithMany(p => p._Assignments)
            //        .HasForeignKey(d => d.TeamId)
            //        .HasConstraintName("FK_Assignments_Teams");
            //});

            //modelBuilder.Entity<Employee>(entity =>
            //{
            //    entity.Property(e => e.Address).IsRequired();

            //    entity.Property(e => e.Mail).IsRequired();

            //    entity.Property(e => e.Name).IsRequired();

            //    entity.Property(e => e.Phone).IsRequired();

            //    entity.Property(e => e.SurName).IsRequired();

            //    entity.Property(e => e.Title).IsRequired();

            //    entity.Property(e => e.UserName).IsRequired();
            //});

            //modelBuilder.Entity<ShiftList>(entity =>
            //{
            //    entity.Property(e => e.Shift_Name)
            //        .IsRequired()
            //        .HasColumnName("Shift_Name");
            //});

            //modelBuilder.Entity<Team>(entity =>
            //{
            //    entity.Property(e => e.TeamName).IsRequired();
            //});

            //modelBuilder.Entity<TeamEmployee>(entity =>
            //{
            //    entity.Property(e => e.EmployeeId).HasColumnName("Employee_Id");

            //    entity.Property(e => e.IsLeader).HasColumnName("isLeader");

            //    entity.Property(e => e.TeamId).HasColumnName("Team_Id");

            //    entity.HasOne(d => d.Employee)
            //        .WithMany(p => p._TeamEmployees)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .HasConstraintName("FK_TeamEmployees_Employees");

            //    entity.HasOne(d => d.Team)
            //        .WithMany(p => p._TeamEmployees)
            //        .HasForeignKey(d => d.TeamId)
            //        .HasConstraintName("FK_TeamEmployees_Teams");
            //});


            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


    }
}