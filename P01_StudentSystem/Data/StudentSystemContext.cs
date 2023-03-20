﻿using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> Homeworks { get; set; }
        public DbSet<StudentCourse> StudentsCourses { get; set; }

        // Validation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(e =>
            {
                e.HasKey(s => s.StudentId);
                e.Property(s => s.Name).HasMaxLength(100).IsUnicode();
                e.Property(s => s.PhoneNumber).HasMaxLength(10).IsUnicode(false).IsRequired(false);
                e.Property(s => s.BirthDay).IsRequired(false);
            });

            modelBuilder.Entity<Course>(e =>
            {
                e.HasKey(c => c.CourseId);
                e.Property(c => c.Name).HasMaxLength(80).IsUnicode();
                e.Property(c => c.Description).IsUnicode().IsRequired(false);
            });

            modelBuilder.Entity<Resource>(e =>
            {
                e.HasKey(r => r.ResourceId);
                e.Property(r => r.Name).HasMaxLength(50).IsUnicode();
                e.Property(r => r.Url).IsUnicode(false);

                e.HasOne(r => r.Course).WithMany(c => c.Resources).HasForeignKey(r => r.CourseId);
            });

            modelBuilder.Entity<Homework>(e =>
            {
                e.HasKey(h => h.HomeworkId);
                e.Property(h => h.Content).IsUnicode(false);

                e.HasOne(h => h.Course).WithMany(c => c.Homeworks).HasForeignKey(h => h.CourseId);
                e.HasOne(h => h.Student).WithMany(h => h.Homeworks).HasForeignKey(h => h.StudentId);
            });

            modelBuilder.Entity<StudentCourse>(e =>
            {
                e.HasKey(sc => new { sc.StudentId, sc.CourseId });

                e.HasOne(sc => sc.Student).WithMany(s => s.StudentCourses).HasForeignKey(sc => sc.StudentId);
                e.HasOne(sc => sc.Course).WithMany(c => c.StudentsCourses).HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}
