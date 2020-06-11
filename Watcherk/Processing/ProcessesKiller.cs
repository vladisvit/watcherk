using log4net;
using System;
using System.Diagnostics;
using System.Timers;
using Watcherk.Parser;

namespace Watcherk.Processing
{
  public class ProcessesKiller
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(ProcessesKiller));
    private bool _cancelled = false;
    private ICommandLineParser parser;
    private IProcessWatcher watcher;
    private IOptions options;

    public ProcessesKiller(ICommandLineParser commandParser, IProcessWatcher processWatcher)
    {
      parser = commandParser;
      watcher = processWatcher;
      options = new Options();
    }

    public void Run()
    {
      var result = parser.Parse(options);
      if (!result.Valid)
      {
        Console.WriteLine(result.InputValue);
        Console.WriteLine(result.HelpText);
        return;
      }

      Console.CancelKeyPress += Console_CancelKeyPress;

      KillProcesses();//first run check and kill process according to the params

      var timer = new Timer(TimeSpan.FromMinutes(options.Frequency).TotalMilliseconds);
      timer.AutoReset = true;
      timer.Elapsed += Timer_Elapsed;
      try
      {
        timer.Start();
        while (true)
        {
          if (_cancelled)
          {
            log.Info("The app was canceled");
            timer.Dispose();
            Console.CancelKeyPress -= Console_CancelKeyPress;
            return;
          }
        }
      }
      catch (Exception e)
      {
        log.Error(e.Message);
        log.Error(e.StackTrace);
      }
      finally
      {
        timer.Dispose();
        Console.CancelKeyPress -= Console_CancelKeyPress;
      }
    }

    private void KillProcesses()
    {
      var processes = watcher.GetProcesses(options.ProcessName);
      try
      {
        foreach (var process in processes)
        {
          var lifeTime = (DateTime.Now - process.StartTime).TotalMinutes;
          if (lifeTime > options.MaxLifeTime)
          {
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            log.Warn($"Try to kill process {process.ProcessName} ID: {process.Id}");
            process.Kill(true);
          }
        }
      }
      catch (Exception e)
      {
        log.Error(e.Message);
        log.Error(e.StackTrace);
        throw;
      }
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs eventArgs)
    {
      KillProcesses();
    }

    private void Process_Exited(object sender, System.EventArgs e)
    {
      if (sender is Process process)
      {
        log.Warn($"Process {process.ProcessName} Id: {process.Id} exited. Start time: {process.StartTime}");
      }
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      log.Info("Cancelling");
      if (e.SpecialKey == ConsoleSpecialKey.ControlC)
      {
        _cancelled = true;
        e.Cancel = true;
      }
    }
  }
}