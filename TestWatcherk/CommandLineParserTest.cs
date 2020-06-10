using Moq;
using NUnit.Framework;
using Watcherk.Parser;

namespace TestWatcherk
{
  public class CommandLineParserTest
  {
    [Test]
    public void RightArgs()
    {
      string[] args = new string[] { "notepad", "5", "1" };
      var mockOptions = new Mock<Options>();
      var parser = new CommandLineParser(args);
      var result = parser.Parse(mockOptions.Object);
      Assert.IsTrue(result.Valid);
      Assert.AreEqual(mockOptions.Object.ProcessName, "notepad");
      Assert.AreEqual(mockOptions.Object.MaxLifeTime, 5);
      Assert.AreEqual(mockOptions.Object.Frequency, 1);
    }

    [Test]
    public void WrongArgsLess()
    {
      string[] args = new string[] { "notepad", "5" };
      var mockOptions = new Mock<Options>();
      var parser = new CommandLineParser(args);
      var result = parser.Parse(mockOptions.Object);
      Assert.IsFalse(result.Valid);
    }

    [Test]
    public void WrongArgsMore()
    {
      string[] args = new string[] { "notepad", "5", "1", "3" };
      var mockOptions = new Mock<Options>();
      var parser = new CommandLineParser(args);
      var result = parser.Parse(mockOptions.Object);
      Assert.IsFalse(result.Valid);
    }

    [Test]
    public void WrongArgsType()
    {
      string[] args = new string[] { "notepad", "crazy", "1" };
      var mockOptions = new Mock<Options>();
      var parser = new CommandLineParser(args);
      var result = parser.Parse(mockOptions.Object);
      Assert.IsFalse(result.Valid);

      string[] args1 = new string[] { "notepad", "crazy", "crazy" };
      var parser1 = new CommandLineParser(args);
      var result1 = parser.Parse(mockOptions.Object);
      Assert.IsFalse(result1.Valid);

      string[] args2 = new string[] { "notepad", "5", "crazy" };
      var parser2 = new CommandLineParser(args);
      var result2 = parser.Parse(mockOptions.Object);
      Assert.IsFalse(result2.Valid);
    }
  }
}