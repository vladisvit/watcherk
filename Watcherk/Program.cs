using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using System;
using System.Reflection;
using Watcherk.Parser;
using Watcherk.Processing;

namespace Watcherk
{
  internal class Program
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(Program));

    private static void Main(string[] args)
    {
      ILoggerRepository repository = log4net.LogManager.GetRepository(Assembly.GetCallingAssembly());
      IAppender fileAppender = CreateFileAppender();
      IAppender consoleAppender = CreateConsoleAppender();
      BasicConfigurator.Configure(repository, fileAppender, consoleAppender);

      log.Info($"Watching the processes by name:{args[0]}");
      ICommandLineParser parser = new CommandLineParser(args);
      IProcessWatcher watcher = new ProcessWatcher();
      var processesKiller = new ProcessesKiller(parser, watcher);
      processesKiller.Run();
      log.Info("DONE");
    }

    /// <summary>
    /// To avoid any additional config files the appender will be create programmatically
    /// </summary>
    /// <returns></returns>
    private static IAppender CreateFileAppender()
    {
      var fileAppender = new RollingFileAppender
      {
        Name = "watcherk_filelog",
        File = "watcherk.log",
        LockingModel = new FileAppender.MinimalLock(),
        AppendToFile = true,
        RollingStyle = RollingFileAppender.RollingMode.Size,
        MaxSizeRollBackups = 2,
        MaximumFileSize = "1MB",
        StaticLogFileName = true,
        Layout = new log4net.Layout.PatternLayout("%d [%t] %-5p %c %m%n")
      };
      fileAppender.ActivateOptions();

      return fileAppender;
    }

    private static IAppender CreateConsoleAppender()
    {
      var consoleAppender = new ManagedColoredConsoleAppender();
      consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors
      {
        Level = Level.Info,
        ForeColor = ConsoleColor.Green
      });
      consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors
      {
        Level = Level.Warn,
        ForeColor = ConsoleColor.Yellow
      });
      consoleAppender.AddMapping(new ManagedColoredConsoleAppender.LevelColors
      {
        Level = Level.Error,
        ForeColor = ConsoleColor.Red
      });
      consoleAppender.Layout = new log4net.Layout.PatternLayout("%d [%t] %-5p %c %m%n");
      consoleAppender.ActivateOptions();

      return consoleAppender;
    }
  }
}