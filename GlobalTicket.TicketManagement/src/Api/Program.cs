using GlobalTicket.TicketManagement.Api;

WebApplication app = WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigurePipeline();

#if DEBUG
await app.RestDatabaseAsync();
#endif

app.Run();
