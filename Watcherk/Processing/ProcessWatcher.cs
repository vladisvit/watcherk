using log4net;
using System.Diagnostics;

namespace Watcherk.Processing
{
  public class ProcessWatcher : IProcessWatcher
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(ProcessWatcher));

    //TODO: it could be improved with kind of the regex pattern
    // currently just by the process name
    public Process[] GetProcesses(string processName)
    {
      Process[] processlist = Process.GetProcessesByName(processName);

      foreach (Process theprocess in processlist)
      {
        log.Warn($"Finded process: {theprocess.ProcessName} ID: {theprocess.Id}");
      }

      return processlist;
    }
  }
}