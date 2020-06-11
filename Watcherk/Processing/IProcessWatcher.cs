using System.Diagnostics;

namespace Watcherk.Processing
{
  public interface IProcessWatcher
  {
    Process[] GetProcesses(string patternProcessName);
  }
}