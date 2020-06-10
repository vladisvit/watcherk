using System;
using System.Timers;
using Watcherk.Parser;

namespace Watcherk.Processing
{
  public class ProcessesKiller
  {
    const int WaitingTime = 500;
    bool _cancelled = false;
    ICommandLineParser parser;
    IProcessWatcher watcher;
    IOptions options;
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
      var timer = new Timer(options.Frequency);
      timer.AutoReset = true;
      timer.Elapsed += Timer_Elapsed;
      try
      {
        timer.Start();
        while (true)
        {
          if (_cancelled)
          {
            Console.WriteLine("Finish");
            timer.Dispose();
            Console.CancelKeyPress -= Console_CancelKeyPress;
            return;
          }
        }
      }
      finally
      {
        timer.Dispose();
      }
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      var processes = watcher.GetProcesses(options.ProcessName);
      foreach (var process in processes)
      {
        if (DateTime.Now - process.StartTime > TimeSpan.FromMinutes(options.MaxLifeTime))
        {
          process.EnableRaisingEvents = true;
          process.Exited += Process_Exited;
          process.Kill();
        }
      }
    }

    private void Process_Exited(object sender, System.EventArgs e)
    {
      Console.WriteLine("process exited");
    }

    private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
      Console.WriteLine("Cancelling");
      if (e.SpecialKey == ConsoleSpecialKey.ControlC)
      {
        _cancelled = true;
        e.Cancel = true;
      }
    }
  }
}
