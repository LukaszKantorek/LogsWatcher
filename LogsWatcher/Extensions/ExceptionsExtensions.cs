using System;

namespace LogsWatcher.Extensions
{
    public class ExceptionsExtensions
    {
        public static TReturn InvokeWithinTryCatch<TReturn>(Func<TReturn> action)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                // Some logging e.g. via Log4Net -> _log.LogError(e.Message); 
                throw; // Whis should be removed for production env. Don't want to watch stack.
            }
        }
    }
}
