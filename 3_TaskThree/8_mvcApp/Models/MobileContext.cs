using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _8_mvcApp.Models;
using Microsoft.EntityFrameworkCore;

namespace _8_mvcApp.Models
{
    public class MobileContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }

        public MobileContext(DbContextOptions<MobileContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}