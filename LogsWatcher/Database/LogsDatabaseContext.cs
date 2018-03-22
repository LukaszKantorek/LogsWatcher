using LogsReader.Model;
using Microsoft.EntityFrameworkCore;

namespace LogsReader.Database
{
    public class LogsDatabaseContext : DbContext
    {
        public LogsDatabaseContext(DbContextOptions<LogsDatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Log> Logs { get; set; }

        public DbSet<LogType> LogTypes { get; set; }
    }
}
