using System;
using LogsWatcher.Model;

namespace LogsWatcher.Database
{
    public interface ILogsInsertingMethodsFactory
    {
        ILogsInsertingMethodsFactory ForLogsType(LogTypeEnum logType);
        Action<Log> GetInsertingMethod();
    }

    public class LogsInsertingMethodsFactory : ILogsInsertingMethodsFactory
    {
        private readonly ILogsRepository _logsRepository;
        private readonly LogTypeEnum _logType;

        public LogsInsertingMethodsFactory(ILogsRepository logsRepository) // For initialization in Startup
        {
            _logsRepository = logsRepository;
        }

        private LogsInsertingMethodsFactory(ILogsRepository logsRepository, LogTypeEnum logType)
        {
            _logsRepository = logsRepository;
            _logType = logType;
        }

        public ILogsInsertingMethodsFactory ForLogsType(LogTypeEnum logType)
        {
            return new LogsInsertingMethodsFactory(_logsRepository, logType);
        }

        public Action<Log> GetInsertingMethod()
        {
            if (_logType == LogTypeEnum.Trace)
                return _logsRepository.InsertTrace;
            if (_logType == LogTypeEnum.Warning)
                return _logsRepository.InsertWarning;
            if(_logType == LogTypeEnum.Error)
                return _logsRepository.InsertError;

            throw new NotImplementedException("Could not find inserting method for depiced logs type number : " + _logType);
        }
    }
}
