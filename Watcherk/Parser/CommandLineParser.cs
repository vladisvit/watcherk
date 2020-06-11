namespace Watcherk.Parser
{
  public class CommandLineParser : ICommandLineParser
  {
    private const string Usage = "Usage: Watcherk <process name> <maximum life time of the process in minutes (int)> <checking frequency in minutes (int)>";
    private string[] args;

    public CommandLineParser(string[] commandLineArguments)
    {
      args = commandLineArguments;
    }

    public IParserResult Parse(IOptions options)
    {
      IParserResult result = new ParserResult();
      result.Valid = false;

      // Test if input arguments were supplied.
      if (args.Length == 0 || args.Length != 3)
      {
        result.InputValue = "Please enter arguments according to the usage describe.";
        result.HelpText = Usage;
        return result;
      }

      options.ProcessName = args[0].Trim();

      // parse second argument MaxLifeTime
      int max;
      if (!int.TryParse(args[1], out max))
      {
        result.InputValue = $"Wrong argument {args[1]} Please enter arguments according to the usage describe.";
        result.HelpText = Usage;
        return result;
      }

      options.MaxLifeTime = max;

      // parse third argument Frequency
      int frequency;
      if (!int.TryParse(args[2], out frequency))
      {
        result.InputValue = $"Wrong argument {args[2]} Please enter arguments according to the usage describe.";
        result.HelpText = Usage;
        return result;
      }

      options.Frequency = frequency;

      result.Valid = true;

      return result;
    }
  }
}