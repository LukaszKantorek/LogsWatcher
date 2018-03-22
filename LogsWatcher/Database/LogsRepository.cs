using System;
using System.Collections.Generic;
using System.Linq;
using LogsReader.Model;
using Microsoft.EntityFrameworkCore;

namespace LogsReader.Database
{
    public interface ILogsRepository
    {
        void InsertError(Log log);
        void InsertWarning(Log log);
        void InsertTrace(Log log);
        IEnumerable<Log> SelectAllLogs();
        IEnumerable<Log> SelectLogsForType(LogTypeEnum type);
    }

    public class LogsRepository : ILogsRepository
    {
        private readonly LogsDatabaseContext _databaseContext;

        public LogsRepository(LogsDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public void InsertWarning(Log log)
        {
            InsertLogOfType(log, LogTypeEnum.Warning);
        }
        
        public void InsertError(Log log)
        {
            InsertLogOfType(log, LogTypeEnum.Error);
        }

        public void InsertTrace(Log log)
        {
            InsertLogOfType(log, LogTypeEnum.Trace);
        }

        public IEnumerable<Log> SelectAllLogs()
        {
            return GetLogsWithTypes()
                .ToList();
        }

        public IEnumerable<Log> SelectLogsForType(LogTypeEnum type)
        {
            if (type == LogTypeEnum.Error)
                return GetLogsWithTypes()
                    .Where(x => x.CheckIfError())
                    .ToList();

            if (type == LogTypeEnum.Warning)
                return GetLogsWithTypes()
                    .Where(x => x.CheckIfWarning())
                    .ToList();

            if (type == LogTypeEnum.Trace)
                return GetLogsWithTypes()
                    .Where(x => x.CheckIfTrace())
                    .ToList();

            return _databaseContext
                .Logs
                .ToList();
        }

        private void InsertLogOfType(Log log, LogTypeEnum typeEnum)
        {
            if (log == null)
                throw new Exception("log object is not instantiated");

            log.Type = _databaseContext
                .LogTypes
                .FirstOrDefault(x => x.TypeNumber == typeEnum);

            _databaseContext.Logs.Add(log);
            _databaseContext.SaveChanges();
        }

        private IEnumerable<Log>  GetLogsWithTypes()
        {
            var logsWithTypes = _databaseContext
                .Logs
                .Include(x => x.Type).ToList();
            return logsWithTypes;
        }
    }
}
