namespace Watcherk.Parser
{
  public class ParserResult : IParserResult
  {
    public string InputValue { get; set; }
    public string HelpText { get; set; }
    public bool Valid { get; set; }
  }
}