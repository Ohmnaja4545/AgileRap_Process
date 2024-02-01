using Microsoft.EntityFrameworkCore;
using AgileRap_Process.Models;

namespace AgileRap_Process.Data
{
    public class AgileContext : DbContext
    {
        public DbSet<Provider> providers { get; set; }  
        public DbSet<ProviderLog> providersLog { get; set; }
        public DbSet<Status> statuses { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Work> works { get; set; }  
        public DbSet<WorkLog> workLog { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=AgileRap;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }


    }
}
