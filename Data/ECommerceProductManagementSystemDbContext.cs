using E_CommerceProductManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceProductManagementSystem.Data;

public class ECommerceProductManagementSystemDbContext : DbContext
{
    public ECommerceProductManagementSystemDbContext(
        DbContextOptions<ECommerceProductManagementSystemDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Defining the Relationship beween Order and OrderItems with Cascade Delete Behavior

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed category
        modelBuilder.Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, Name = "Books", Description = "Various genres of books and literature" },
                new Category { Id = 3, Name = "Clothing", Description = "Men's and women's apparel" }
            );

        // Seed products
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1, Name = "Laptop", Price = 1500m, CategoryId = 1, Description = "High performance laptop"
            },
            new Product
            {
                Id = 2, Name = "Smartphone", Price = 800m, CategoryId = 1,
                Description = "Latest model smartphone"
            },
            new Product
            {
                Id = 3, Name = "ASP.NET Core Book", Price = 40m, CategoryId = 2,
                Description = "Comprehensive guide to ASP.NET Core"
            }
        );

        // Seed customer
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "John Doe", Email = "john@example.com" },
            new Customer { Id = 2, FullName = "Jane Smith", Email = "jane@example.com" }
        );

        // Seed order
        modelBuilder.Entity<Order>().Property(o => o.Date).HasColumnType("timestamptz");
        
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1, CustomerId = 1,
                Date = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero),
                Amount = 1540.00m
            },
            new Order
            {
                Id = 2, CustomerId = 2,
                Date = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero),
                Amount = 840.00m
            }
        );

        // Seed OrderItems
        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 1500.00m },
            new OrderItem { Id = 2, OrderId = 1, ProductId = 3, Quantity = 1, UnitPrice = 40.00m },
            new OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, UnitPrice = 800.00m },
            new OrderItem { Id = 4, OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 40.00m }
        );
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }
}