using System;
using System.Linq;
using LogsWatcher.Database;
using LogsWatcher.Extensions;
using LogsWatcher.Model;
using Microsoft.AspNetCore.Mvc;

namespace LogsWatcher.Controllers
{
    public class LoggerController : Controller
    {
        private readonly ILogsRepository _logsRepository;
        private readonly ILogsInsertingMethodsFactory _methodsFactory;

        public LoggerController(ILogsRepository logsRepository, ILogsInsertingMethodsFactory methodsFactory)
        {
            _logsRepository = logsRepository;
            _methodsFactory = methodsFactory;
        }

        [HttpGet]
        public ActionResult GetLogs()
        {
            var logs = ExceptionsExtensions.InvokeWithinTryCatch(() => 
                _logsRepository
                    .SelectAllLogs()
                    .ToList());

            return Json(logs);
        }

        [HttpGet]
        public void AddLog(LogTypeEnum logType)
        {
            ExceptionsExtensions.InvokeWithinTryCatch<object>(() =>
            {
                var assemblyPath = GetCurrentAssemblyPath();

                _methodsFactory
                    .ForLogsType(logType)
                    .GetInsertingMethod()(
                        new Log
                        {
                            InsertDate = DateTime.Now,
                            Value = string.Format("This is {0}", Enum.GetName(typeof(LogTypeEnum), logType)),
                            StackTrace = assemblyPath
                        });

                return null;
            });
        }

        private string  GetCurrentAssemblyPath()
        {
            return System.Reflection
                .Assembly
                .GetAssembly(typeof(LoggerController))
                .Location;
        }
    }
}
