using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using moore.Models;

namespace moore.Data
{
    // This class inherits from IdentityDbContext to add user authentication and authorization to the data context.
    // It includes properties for the sets of orders, products, and order details in the database.
    public class mooreContext : IdentityDbContext<ApplicationUser>
    {
        public mooreContext (DbContextOptions<mooreContext> options)
            : base(options)
        {
        }
        // Set of orders

        public DbSet<moore.Models.Order> Order { get; set; } = default!;
        // Set of products

        public DbSet<moore.Models.Product>? Product { get; set; }
        // Set of order details

        public DbSet<moore.Models.OrderDetail>? OrderDetail { get; set; }
        


    }
}
