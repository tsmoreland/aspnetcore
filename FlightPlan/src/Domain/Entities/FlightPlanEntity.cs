using System.Text.Json.Serialization;

namespace FlightPlan.Domain.Entities;

public class FlightPlanEntity
{
    [JsonPropertyName("flight_plan_id")]
    public string FlightPlanId { get; set; } = string.Empty;

    [JsonPropertyName("aircraft_identification")]
    public string AircraftIdentification { get; set; } = string.Empty;

    [JsonPropertyName("aircraft_type")]
    public string AircraftType { get; set; } = string.Empty;

    [JsonPropertyName("airspeed")]
    public int Airspeed { get; set; }

    [JsonPropertyName("altitude")]
    public int Altitude { get; set; }

    [JsonPropertyName("flight_type")]
    public string FlightType { get; set; } = string.Empty;

    [JsonPropertyName("fuel_hours")]
    public int FuelHours { get; set; }

    [JsonPropertyName("fuel_minutes")]
    public int FuelMinutes { get; set; }

    [JsonPropertyName("departure_time")]
    public DateTime DepartureTime { get; set; }

    [JsonPropertyName("estimated_arrival_time")]
    public DateTime ArrivalTime { get; set; }

    [JsonPropertyName("departing_airport")]
    public string DepartureAirport { get; set; } = string.Empty;

    [JsonPropertyName("arrival_airport")]
    public string ArrivalAirport { get; set; } = string.Empty;

    [JsonPropertyName("route")]
    public string Route { get; set; } = string.Empty;

    [JsonPropertyName("remarks")]
    public string Remarks { get; set; } = string.Empty;

    [JsonPropertyName("number_onboard")]
    public int NumberOnBoard { get; set; }
}
