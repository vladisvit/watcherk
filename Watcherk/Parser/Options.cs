namespace Watcherk.Parser
{
  public class Options : IOptions
  {
    public string ProcessName { get; set; }
    public int MaxLifeTime { get; set; }
    public int Frequency { get; set; }
  }
}