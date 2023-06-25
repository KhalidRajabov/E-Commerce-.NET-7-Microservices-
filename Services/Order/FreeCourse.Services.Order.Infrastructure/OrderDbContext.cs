using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext:DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options):base(options) { }

        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //code below asks for table name of entity and schema name
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x=>x.Price).HasColumnType("decimal(18,2)");



            //owned means, there will not be created another table named "Address"
            //Instead, properties of "Address" will be added to Order class as columns
            //It can be done when there is a class with only a few properties
            //we can make the code below in a different way, like writing [Owned] on the class name of Address
            //But then, Domain will understand that it is working with efcore, which is wrong
            //Because Domain shoul not know what ORM we are using. That is why we write the code below,
            //meaning that the "Address" class is owned by  Order class
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(x => x.Address).WithOwner();


            base.OnModelCreating(modelBuilder);
        }
    }
}
