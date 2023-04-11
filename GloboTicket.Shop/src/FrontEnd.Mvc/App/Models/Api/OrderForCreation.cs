namespace GloboTicket.FrontEnd.Mvc.App.Models.Api;

public class OrderForCreation
{
    public required DateTimeOffset Date { get; set; }
    public required CustomerDetails CustomerDetails { get; set; } 
    public required List<OrderLine> Lines { get; set; } 
}
