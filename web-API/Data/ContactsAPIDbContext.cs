using Microsoft.EntityFrameworkCore;
using web_API.Models;

namespace web_API.Data
{
    public class ContactsAPIDbContext : DbContext
    {
        public ContactsAPIDbContext(DbContextOptions<ContactsAPIDbContext> options)
             : base(options)
        {
        }

        public DbSet<Files1> Files1 { get; set; }

    }
}