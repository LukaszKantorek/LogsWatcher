namespace LogsReader.Model
{
    public static class AllowedLogTypes
    {
        public static bool CheckIfTrace(this Log logType)
        {
            return logType.Type.TypeNumber.CheckType(LogTypeEnum.Trace);
        }

        public static bool CheckIfWarning(this Log logType)
        {
            return logType.Type.TypeNumber.CheckType(LogTypeEnum.Warning);
        }

        public static bool CheckIfError(this Log logType)
        {
            return logType.Type.TypeNumber.CheckType(LogTypeEnum.Error);
        }

        private static LogTypeEnum GetAllowedLogTypes()
        {
            return LogTypeEnum.Error | LogTypeEnum.Trace | LogTypeEnum.Warning;
        }

        private static bool CheckType(this LogTypeEnum checkingType, LogTypeEnum requiredType)
        {
            return (GetAllowedLogTypes() & requiredType) == checkingType;
        }
    }
}
