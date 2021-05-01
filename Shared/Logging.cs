using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MfePoc.Shared
{
    public static class Logging
    {
        public static void SetupNLog<T>(Action run)
        {
            var config = new LoggingConfiguration();

            var logFolder = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "logs"));

            var fileTarget = new FileTarget("file")
            {
                FileName = Path.Combine(logFolder, "log.log"),
            };

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;

            var logger = LogManager.GetLogger(typeof(T).FullName);
            logger.Info("Process start");

            try
            {
                run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Process exception");
            }
            finally
            {
                logger.Info("Process end");
            }

            LogManager.Shutdown();
        }
    }
}
