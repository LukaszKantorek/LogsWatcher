using System;
using System.Collections.Generic;
using System.Linq;
using LogsWatcher.Database;
using LogsWatcher.Model;
using Moq;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace LogsWatcher.Tests
{
    [TestFixture]
    public class LogsRepositoryTests
    {
        private LogsRepository _repository;
        private Mock<LogsDatabaseContext> _mockedContext;
        private List<Log> _logs;
        private List<LogType> _logTypes;

        [SetUp]
        public void Setup()
        {
            _logs = new List<Log>();
            _logTypes = new List<LogType>();

            var logTypes = new List<LogType>
            {
                new LogType {TypeNumber = LogTypeEnum.Error, DisplayName = "Error"},
                new LogType {TypeNumber = LogTypeEnum.Warning, DisplayName = "Warning"},
                new LogType {TypeNumber = LogTypeEnum.Trace, DisplayName = "Trace"}
            };

            _mockedContext = new Mock<LogsDatabaseContext>();
            _mockedContext.Setup(c => c.LogTypes).ReturnsDbSet(logTypes, _logTypes);
            _mockedContext.Setup(c => c.Logs).ReturnsDbSet(_logs, _logs);

            _repository = new LogsRepository(_mockedContext.Object);
        }

        [Test]
        public void It_Should_Be_Able_to_Inset_Error()
        {
            var insertedEntity = new Log
            {
                InsertDate = DateTime.Now,
                StackTrace = "C://this/is/some/path/to/error",
            };

            _repository.InsertError(insertedEntity);

            RunInsertingAsserts(insertedEntity);
        }

        [Test]
        public void It_Should_Be_Able_to_Inset_Warning()
        {
            var insertedEntity = new Log
            {
                InsertDate = DateTime.Now,
                StackTrace = "C://this/is/some/path/to/warning",
            };

            _repository.InsertWarning(insertedEntity);

            RunInsertingAsserts(insertedEntity);
        }

        [Test]
        public void It_Should_Be_Able_to_Inset_Trace()
        {
            var insertedEntity = new Log
            {
                InsertDate = DateTime.Now,
                StackTrace = "C://this/is/some/path/to/warning",
            };

            _repository.InsertTrace(insertedEntity);

            RunInsertingAsserts(insertedEntity);
        }

        private void RunInsertingAsserts(Log requiredEntity)
        {
            Assert.IsNotEmpty(_logs);
            Assert.AreEqual(_logs.FirstOrDefault().InsertDate, requiredEntity.InsertDate);
            Assert.AreEqual(_logs.FirstOrDefault().StackTrace, requiredEntity.StackTrace);
            Assert.AreEqual(_logs.FirstOrDefault().Type.TypeNumber, requiredEntity.Type.TypeNumber);
            Assert.AreEqual(_logs.FirstOrDefault().Type.DisplayName, requiredEntity.Type.DisplayName);
        }
    }
}
