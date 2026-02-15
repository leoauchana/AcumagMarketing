using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AcumagContext : DbContext
{
    public AcumagContext(DbContextOptions<AcumagContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CustomerConfig(modelBuilder);
        EmployeeConfig(modelBuilder);
        UserConfig(modelBuilder);
        RoleConfig(modelBuilder);
        QuoteConfig(modelBuilder);
        QuoteOrderConfig(modelBuilder);
        DocumentConfig(modelBuilder);
    }

    private void CustomerConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .ToTable("Customers")
            .HasKey(a => a.Id);
        modelBuilder.Entity<Customer>()
            .Property(a => a.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(a => a.LastName)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(a => a.PhoneNumber)
            .HasMaxLength(15)
            .HasColumnType("varchar")
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(a => a.Dni)
            .HasConversion(
                dni => dni.Value,
                value => Dni.Create(value)
            )
            .HasColumnType("varchar")
            .IsRequired();
        modelBuilder.Entity<Customer>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.City)
                    .HasColumnType("varchar")
                    .HasMaxLength(30)
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Customer>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.Street)
                    .HasColumnType("varchar")
                    .HasMaxLength(30)
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Customer>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.Number)
                    .HasColumnType("int")
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Customer>()
            .Property(a => a.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value)
            )
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
    } 
    private void EmployeeConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .ToTable("Employees")
            .HasKey(a => a.Id);
        modelBuilder.Entity<Employee>()
            .Property(a => a.FirstName)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Employee>()
            .Property(a => a.LastName)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Employee>()
            .Property(a => a.Dni)
            .HasConversion(
                dni => dni.Value,
                value => Dni.Create(value)
            )
            .HasColumnType("varchar")
            .IsRequired();
        modelBuilder.Entity<Employee>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.City)
                    .HasColumnType("varchar")
                    .HasMaxLength(30)
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Employee>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.Street)
                    .HasColumnType("varchar")
                    .HasMaxLength(30)
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Employee>(a =>
        {
            a.OwnsOne(d => d.Domicilie, dom =>
            {
                dom.Property(d => d.Number)
                    .HasColumnType("int")
                    .IsRequired();
            });
        });
        modelBuilder.Entity<Employee>()
            .Property(a => a.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value)
            )
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();
    }
    private void UserConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("Users")
            .HasKey(a => a.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.Username)
            .HasMaxLength(20)
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property(u => u.Password)
            .HasMaxLength(20)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<Employee>(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    private void RoleConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>()
            .ToTable("Roles")
            .HasKey(r => r.Id);
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Role>()
            .Property(r => r.Description)
            .HasMaxLength(50)
            .HasColumnType("varchar")
            .IsRequired();
    }
    private void QuoteConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quote>()
            .ToTable("Quotes")
            .HasKey(q => q.Id);
        modelBuilder.Entity<Quote>()
            .Property(q => q.QuoteDate)
            .IsRequired();
        modelBuilder.Entity<Quote>()
            .Property(q => q.Observations)
            .HasMaxLength(100)
            .HasColumnType("varchar")
            .IsRequired();
        modelBuilder.Entity<Quote>()
            .Property(q => q.ExpirationDate)
            .HasColumnType("varchar")
            .IsRequired();
        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Employee)
            .WithMany()
            .HasForeignKey(q => q.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Quote>()
            .HasOne(q => q.QuoteOrder)
            .WithOne(qr => qr.Quote)
            .HasForeignKey<Quote>(q => q.QuoteOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    private void QuoteOrderConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuoteOrder>()
            .ToTable("Quote_order")
            .HasKey(q => q.Id);
        modelBuilder.Entity<QuoteOrder>()
            .Property(q => q.PresentationDate)
            .IsRequired();
        modelBuilder.Entity<QuoteOrder>()
            .Property(q => q.QuoteState)
            .IsRequired();
        modelBuilder.Entity<QuoteOrder>()
            .HasOne(qr => qr.Customer)
            .WithOne()
            .HasForeignKey<QuoteOrder>(qr => qr.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<QuoteOrder>()
            .HasOne(qr => qr.Employee)
            .WithOne()
            .HasForeignKey<QuoteOrder>(qr => qr.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    private void DocumentConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .ToTable("Document")
            .HasKey(d => d.Id);
        modelBuilder.Entity<Document>()
            .Property(d => d.Name)
            .HasMaxLength(50)
            .IsRequired();
        modelBuilder.Entity<Document>()
            .Property(d => d.Path)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<Document>()
            .Property(d => d.Size)
            .HasColumnType("float")
            .IsRequired();
        modelBuilder.Entity<Document>()
            .Property(d => d.Type)
            .HasMaxLength(10)
            .IsRequired();
        modelBuilder.Entity<Document>()
            .HasOne(d => d.QuoteOrder)
            .WithMany(qr => qr.Documents)
            .HasForeignKey(d => d.QuoteOrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}