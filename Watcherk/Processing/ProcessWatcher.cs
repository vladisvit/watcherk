using System;
using System.Diagnostics;

namespace Watcherk.Processing
{
  public class ProcessWatcher : IProcessWatcher
  {
    //TODO: it could be improved with kind of the regex pattern
    // currently just by the process name
    public Process[] GetProcesses(string processName)
    {
      Process[] processlist = Process.GetProcessesByName(processName);

      foreach (Process theprocess in processlist)
      {
        Console.WriteLine("Finded process: {0} ID: {1} StartTime: {2}", theprocess.ProcessName, theprocess.Id, theprocess.StartTime.ToString("o"));
      }

      return processlist;
    }
  }
}
