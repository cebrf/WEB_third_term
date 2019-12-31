using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Task_1
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=T_6;Username=postgres;Password=pswrd");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                User user1 = new User { Name = "Tom", Age = 33 };
                User user2 = new User { Name = "Alice", Age = 26 };

                // добавляем их в бд
                if (!db.Users.Any())
                {
                    db.Users.Add(user1);
                    db.Users.Add(user2);
                    db.SaveChanges();
                }

                // получаем объекты из бд и выводим на консоль
                var users = db.Users.ToList();
                Console.WriteLine("Users list:");
                foreach (User u in users)
                {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                }
            }
        }
    }
}
