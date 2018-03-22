using LogsWatcher.Model;
using Microsoft.EntityFrameworkCore;

namespace LogsWatcher.Database
{
    public class LogsDatabaseContext : DbContext
    {
        public LogsDatabaseContext()
        {
            // For Moq in unit tests
        }

        public LogsDatabaseContext(DbContextOptions<LogsDatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<LogType> LogTypes { get; set; }
    }
}
