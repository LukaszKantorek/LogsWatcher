using System;
using System.Linq;
using LogsReader.Database;
using LogsReader.Model;
using Microsoft.AspNetCore.Mvc;

namespace LogsReader.Controllers
{
    //[Route("api/[controller]")]
    //[Authorize]
    public class DomainController : Controller
    {
        private readonly ILogsRepository _logsRepository;

        public DomainController(ILogsRepository logsRepository)
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
            _logsRepository.InsertTrace(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is warning",
            });
        }

        [HttpGet]
        public void AddWarning()
        {
            _logsRepository.InsertWarning(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is warning",
            });
        }

        [HttpGet]
        public void AddError()
        {
            _logsRepository.InsertError(new Log()
            {
                InsertDate = DateTime.Now,
                Value = "This is error",
            });
        }
    }
}
