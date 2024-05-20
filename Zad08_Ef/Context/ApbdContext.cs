using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zad08_Ef.Models;

namespace Zad08_Ef.Context;

public partial class ApbdContext : DbContext
{
    public ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions<ApbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(
                "Data Source=localhost;Initial Catalog=APBD;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False")
            .LogTo(Console.WriteLine, LogLevel.Information);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.IdGroup).HasName("Groups_pk");

            entity.ToTable("Groups", "cw5");

            entity.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.IdStudent).HasName("Students_pk");

            entity.ToTable("Students", "cw5");

            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => new { e.IdStudent, e.IdGroup }).HasName("Student_Group_pk");

            entity.ToTable("Student_Group", "cw5");

            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.IdGroup)
                .HasConstraintName("Student_Group_Group");

            entity.HasOne(d => d.IdStudentNavigation).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.IdStudent)
                .HasConstraintName("Student_Group_Student");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
