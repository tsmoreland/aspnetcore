//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Microsoft.EntityFrameworkCore;
using SunDoeCoffeeShop.Shared.AuthPersistence;

namespace SunDoeCoffeeShop.Admin.FrontEnd.App;

internal static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        return app;
    }

    public static async Task MigrateIfProduction(this WebApplication app, Serilog.ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            return;
        }

        logger.Information("Performing database migration.");
        using IServiceScope scope = app.Services.CreateAsyncScope();
        AuthDbContext dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        await WaitUntilMigrationComplete(dbContext, logger);
    }


    private static async Task WaitUntilMigrationComplete(DbContext dbContext, Serilog.ILogger logger)
    {
        bool ready = false;
        int failures = 0;
        const int maxFailures = 5;
        while (!ready)
        {
            try
            {
                logger.Information("Attempting to connect to database");
                await dbContext.Database.MigrateAsync();
                ready = true;
            }
            catch (Exception)
            {
                failures++;
            }

            if (!ready && failures >= maxFailures)
            {
                logger.Warning("Unable to connect to database, attempt #{Attempt} out of {MaxFailures}", failures, maxFailures);
                await Task.Delay(TimeSpan.FromSeconds(2.5));
            }
            else if (failures > maxFailures)
            {
                throw new ApplicationException("Unable to start due to database not being available");
            }
        }
    }
}
