namespace Watcherk.Parser
{
  public interface IParserResult
  {
    string InputValue { get; set; }
    string HelpText { get; set; }
    bool Valid { get; set; }
  }
}
