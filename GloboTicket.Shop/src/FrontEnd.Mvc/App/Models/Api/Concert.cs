namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public class Concert
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Artist { get; init; }

    public required DateTime Date { get; init; }

    public required int Price { get; init; }

    public required string Description { get; init; } = string.Empty;
    public string? ImageUrl { get; init; } 
}
