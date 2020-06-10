namespace Watcherk.Parser
{
  public interface IOptions
  {
    int Frequency { get; set; }
    int MaxLifeTime { get; set; }
    string ProcessName { get; set; }
  }
}