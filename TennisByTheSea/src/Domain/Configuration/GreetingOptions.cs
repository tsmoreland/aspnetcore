namespace TennisByTheSea.Domain.Configuration;

public sealed class GreetingOptions
{
    public string Colour { get; set; } = "black";

    public IReadOnlyList<string> Greetings { get; set; } = new List<string>();
    public IReadOnlyList<string> LoginGreetings { get; set; } = new List<string>();
}
