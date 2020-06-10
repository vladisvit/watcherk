namespace Watcherk.Parser
{
  public interface ICommandLineParser
  {
    IParserResult Parse(IOptions options);
  }
}
