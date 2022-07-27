using Microsoft.EntityFrameworkCore;
using DevTrust_Task.Models;

namespace DevTrust_Task.Data
{
    public class DevTrustContext : DbContext
    {
        public DevTrustContext(DbContextOptions<DevTrustContext> opt) : base(opt)
        {

        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Address { get; set; }

    }
}
