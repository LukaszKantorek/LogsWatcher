using System;
using System.Linq;
using LogsWatcher.Database;
using LogsWatcher.Model;
using Microsoft.AspNetCore.Mvc;

namespace LogsWatcher.Controllers
{
    public class LoggerController : Controller
    {
        private readonly ILogsRepository _logsRepository;

        public LoggerController(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        [HttpGet]
        public ActionResult GetLogs()
        {
            var logs = _logsRepository
                .SelectAllLogs()
                .ToList();

            return Json(logs);
        }

        [HttpGet]
        public void AddTrace()
        {
            var assemblyPath = GetCurrentAssemblyPath();

            _logsRepository.InsertTrace(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is warning",
                StackTrace = assemblyPath
            });
        }

        [HttpGet]
        public void AddWarning()
        {
            var assemblyPath = GetCurrentAssemblyPath();

            _logsRepository.InsertWarning(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is warning",
                StackTrace = assemblyPath
            });
        }

        [HttpGet]
        public void AddError()
        {
            var assemblyPath = GetCurrentAssemblyPath();

            _logsRepository.InsertError(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is error",
                StackTrace = assemblyPath
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
