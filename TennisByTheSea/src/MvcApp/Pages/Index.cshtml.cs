using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Contracts.Greetings.Queries;
using TennisByTheSea.Domain.Contracts.Products.Queries;
using TennisByTheSea.Domain.Contracts.Services.Weather;
using TennisByTheSea.MvcApp.Models.Configuration;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.MvcApp.Pages;

public class IndexModel : PageModel
{
    private const string DefaultForecastSectionTitle = "How's the weather";
    private readonly IWeatherForecaster _weatherForecaster;
    private readonly IMediator _mediator;
    private readonly HomePageOptions _options;

    public IndexModel(
        IMediator mediator,
        IOptionsSnapshot<HomePageOptions> options,
        IWeatherForecaster weatherForecaster)
    {
        _mediator = mediator;
        _options = options.Value;
        _weatherForecaster = weatherForecaster;
    }

    public string WeatherDescription { get; private set; } =
            "We don't have the latest weather information right now, " +
            "please check again later.";

    public bool ShowWeatherForecast { get; private set; } = false;
    public string ForecastSectionTitle { get; private set; } =
        DefaultForecastSectionTitle;
    public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
    public string Greeting { get; private set; } = string.Empty;

    public string GreetingColour { get; private set; } = "black";

    public IReadOnlyList<Product> Products { get; private set; } = Array.Empty<Product>();

    public async Task OnGet()
    {
        if (_options.EnableGreeting)
        {
            (Greeting, GreetingColour) = await _mediator.Send(new GetRandomGreetingQuery());
        }

        ShowWeatherForecast = _options.EnableWeatherForecast
            && _weatherForecaster.ForecastEnabled;

        if (ShowWeatherForecast)
        {
            string title = _options.ForecastSectionTitle;
            ForecastSectionTitle = string.IsNullOrEmpty(title)
                ? DefaultForecastSectionTitle : title;

            WeatherResult? currentWeather = await _weatherForecaster
                .GetCurrentWeatherAsync("Eastbourne");

            if (currentWeather.Weather is not null)
            {
                WeatherDescription = currentWeather.Weather.Summary switch
                {
                    "Sun" => "It's sunny right now. A great day for tennis!",
                    "Cloud" => "It's cloudy at the moment and the outdoor courts are available.",
                    "Rain" => "We're sorry but it's raining here. No outdoor courts are available.",
                    "Snow" => "It's snowing!! Outdoor courts will remain closed until the snow clears.",
                    _ => WeatherDescription
                };
            }
        }

        Products = await _mediator.CreateStream(new GetProductsQuery()).ToListAsync();
    }
}
