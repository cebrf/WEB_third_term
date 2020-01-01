using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Task_3
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Field_1 { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationContext()
        {
            //    Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=T_6_1;username=postgres;Password=pswrd");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Add-Migration Name
             * Update-Database
             */
        }
    }
}