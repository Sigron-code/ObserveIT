using Heartbeat_Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heartbeat_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AgentUser> Users { get; set; }
    }
}
