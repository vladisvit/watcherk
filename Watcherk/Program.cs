using Watcherk.Parser;
using Watcherk.Processing;

namespace Watcherk
{
  class Program
  {
    static void Main(string[] args)
    {
      ICommandLineParser parser = new CommandLineParser(args);
      IProcessWatcher watcher = new ProcessWatcher();
      var processesKiller = new ProcessesKiller(parser, watcher);
      processesKiller.Run();
    }
  }
}
