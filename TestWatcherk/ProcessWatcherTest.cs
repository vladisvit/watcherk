using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Watcherk.Processing;

namespace TestWatcherk
{
  /// <summary>
  /// No good test, as it depends on the outside environment
  /// </summary>
  public class ProcessWatcherTest
  {
    private const string ProcessName = @"notepad";

    [SetUp]
    public void Setup()
    {
      Process.Start(Path.Combine(Environment.SystemDirectory, $"{ProcessName}.exe"));
    }

    [Test]
    public void FindProcesses()
    {
      var watcher = new ProcessWatcher();
      var processes = watcher.GetProcesses(ProcessName);
      Assert.IsTrue(processes.Select(e => e.ProcessName.Equals(ProcessName)).Count() > 0);
      Assert.AreEqual(processes[0].ProcessName, ProcessName);
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
      var processesForKill = Process.GetProcessesByName(ProcessName);
      foreach (var item in processesForKill)
      {
        item.Kill();
      }
    }
  }
}