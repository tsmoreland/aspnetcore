using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Contracts.Services.Weather;
using TennisByTheSea.MvcApp.Models.Configuration;
using TennisByTheSea.Shared.WeatherApi;

namespace TennisByTheSea.MvcApp.Pages;

public class IndexModel : PageModel
{
    private const string DefaultForecastSectionTitle = "How's the weather";
	private readonly IWeatherForecaster _weatherForecaster;
	private readonly ILogger<IndexModel> _logger;
	private readonly HomePageOptions _options;
    /*

	private readonly IGreetingService _greetingService;
	private readonly IProductsApiClient _productsApiClient;
    */

	public IndexModel(
		ILogger<IndexModel> logger,		
		//IGreetingService greetingService,
		IOptionsSnapshot<HomePageOptions> options,
		IWeatherForecaster weatherForecaster//,
		/*IProductsApiClient productsApiClient*/)
	{
		_logger = logger;
		//_greetingService = greetingService;
		_options = options.Value;
		_weatherForecaster = weatherForecaster;
		//_productsApiClient = productsApiClient;
	}

    public string WeatherDescription { get; private set; } =
            "We don't have the latest weather information right now, " +
            "please check again later.";

	public bool ShowWeatherForecast { get; private set; } = false;
    public string ForecastSectionTitle { get; private set; } =
        DefaultForecastSectionTitle;
    public bool ShowGreeting => !string.IsNullOrEmpty(Greeting);
    public string Greeting { get; private set; } = string.Empty;
    public string GreetingColour => "Blue";  // TODO: _greetingService.GreetingColour;
    /*
    public string GreetingColour => _greetingService.GreetingColour;
	public IReadOnlyCollection<Product> Products { get; private set; } = Array.Empty<Product>();
    */

	public async Task OnGet()
	{
        /*
		if (_homePageConfig.EnableGreeting)
		{
			Greeting = _greetingService.GetRandomGreeting();
		}
        */

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
                    "Cloud" => "It's cloudy at the moment and the outdoor courts are availale.",
                    "Rain" => "We're sorry but it's raining here. No outdoor courts are available.",
                    "Snow" => "It's snowing!! Outdoor courts will remain closed until the snow clears.",
                    _ => WeatherDescription
                };
            }
		}

		//var productsResult = await _productsApiClient.GetProducts();
		//Products = productsResult?.Products ?? Array.Empty<Product>();
	}
}
