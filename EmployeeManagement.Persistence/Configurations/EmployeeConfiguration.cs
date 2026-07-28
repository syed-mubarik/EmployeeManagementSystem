using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Table Name
            builder.ToTable("Employees");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties
            builder.Property(e => e.EmployeeCode)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(e => e.Salary)
                   .HasPrecision(18, 2);

            // Relationships
            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Designation)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DesignationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}