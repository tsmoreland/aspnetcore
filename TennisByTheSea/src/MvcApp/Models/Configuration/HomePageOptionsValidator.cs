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

using Microsoft.Extensions.Options;
using TennisByTheSea.Domain.Contracts.Services.Profanity;

namespace TennisByTheSea.MvcApp.Models.Configuration;

public sealed class HomePageOptionsValidator : IValidateOptions<HomePageOptions>
{
    private readonly IProfanityChecker _profanityChecker;
    private readonly bool _checkForProfanity;
    private readonly WeatherForecastingOptions _weatherOptions;

    public HomePageOptionsValidator(
        IOptions<WeatherForecastingOptions> weatherOptions,
        IOptions<ContentOptions> contentOptions,
        IProfanityChecker profanityChecker)
    {
        _profanityChecker = profanityChecker;
        _weatherOptions = weatherOptions.Value;
        _checkForProfanity = contentOptions.Value.CheckForProfanity;
    }

    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, HomePageOptions options)
    {
        if (_weatherOptions.Enable && options.EnableWeatherForecast
                                   && string.IsNullOrEmpty(options.ForecastSectionTitle))
        {
            return ValidateOptionsResult.Fail("""
                A title is required, when the weather forecast is enabled.
                """);
        }

        if (_checkForProfanity && _profanityChecker
                .ContainsProfanity(options.ForecastSectionTitle))
        {
            return ValidateOptionsResult.Fail("The configured title contains " +
                                              "a blocked profanity word.");
        }

        return ValidateOptionsResult.Success;
    }
}
