using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace Code_API_Mobile.Models
{
    public class MobileDbContext : DbContext
    {
        public MobileDbContext(DbContextOptions<MobileDbContext> options)
    : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
