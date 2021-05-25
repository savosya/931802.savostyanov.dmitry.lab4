using Microsoft.EntityFrameworkCore;

namespace lab4.Models
{
    public class UserContext : DbContext
    {

        public UserContext(DbContextOptions<UserContext> options)
           : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
