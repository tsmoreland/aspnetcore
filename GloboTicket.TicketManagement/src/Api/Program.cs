using GloboTicket.TicketManagement.Api;

WebApplication app = WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigurePipeline();

#if DEBUG
await app.ResetDatabaseAsync();
#endif

app.Run();
