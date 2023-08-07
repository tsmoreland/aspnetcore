using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FlightPlan.Application.Contracts.Persistence;
using FlightPlan.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FlightPlan.Persistence.Repositories;

public sealed class FlightPlanAsyncRepository : IFlightPlanAsyncRepository
{
    private readonly NoSqlContext _context;
    private const string CollectionName = "flight_plans";

    public FlightPlanAsyncRepository(NoSqlContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<FlightPlanEntity> GetAll([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        IMongoCollection<BsonDocument> collection = _context.GetCollection(CollectionName);
        List<BsonDocument> documents = await collection
            .Find(_ => true)
            .ToListAsync(cancellationToken);

        foreach (BsonDocument document in documents)
        {
            yield return ToFlightPlanOrNull(document);
        }
    }

    /// <inheritdoc />
    public async ValueTask<FlightPlanEntity?> GetById(string id, CancellationToken cancellationToken)
    {
        IMongoCollection<BsonDocument> collection = _context.GetCollection(CollectionName);
        FilterDefinition<BsonDocument> filter = GetFilterForId(id);
        BsonDocument? document = (await collection.FindAsync(filter, cancellationToken: cancellationToken)).FirstOrDefault(cancellationToken);
        return document is not null
            ? ToFlightPlanOrNull(document)
            : null;
    }

    /// <inheritdoc />
    public async ValueTask<(string Id, TransactionResult Result)> Add(FlightPlanEntity entity, CancellationToken cancellationToken)
    {
        IMongoCollection<BsonDocument> collection = _context.GetCollection(CollectionName);
        string id = Guid.NewGuid().ToString("N");
        BsonDocument document = new()
        {
            { "flight_plan_id", id },
            { "altitude", entity.Altitude },
            { "airspeed", entity.Airspeed },
            { "aircraft_identification", entity.AircraftIdentification },
            { "aircraft_type", entity.AircraftType },
            { "arrival_airport", entity.ArrivalAirport },
            { "flight_type", entity.FlightType },
            { "departing_airport", entity.DepartureAirport },
            { "departure_time", entity.DepartureTime },
            { "estimated_arrival_time", entity.ArrivalTime },
            { "route", entity.Route },
            { "remarks", entity.Remarks },
            { "fuel_hours", entity.FuelHours },
            { "fuel_minutes", entity.FuelMinutes },
            { "number_onboard", entity.NumberOnBoard },
        };

        try
        {
            await collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            return document["_id"].IsObjectId
                ? (id, TransactionResult.Success)
                : (string.Empty, TransactionResult.BadRequest);
        }
        catch (Exception)
        {
            return (string.Empty, TransactionResult.ServerError);
        }
    }

    /// <inheritdoc />
    public async ValueTask<TransactionResult> Update(string id, FlightPlanEntity entity, CancellationToken cancellationToken)
    {
        IMongoCollection<BsonDocument> collection = _context.GetCollection(CollectionName);
        FilterDefinition<BsonDocument> filter = GetFilterForId(id);
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update
            .Set("flight_plan_id", entity.FlightPlanId)
            .Set("altitude", entity.Altitude)
            .Set("airspeed", entity.Airspeed)
            .Set("aircraft_identification", entity.AircraftIdentification)
            .Set("aircraft_type", entity.AircraftType)
            .Set("arrival_airport", entity.ArrivalAirport)
            .Set("flight_type", entity.FlightType)
            .Set("departing_airport", entity.DepartureAirport)
            .Set("departure_time", entity.DepartureTime)
            .Set("estimated_arrival_time", entity.ArrivalTime)
            .Set("route", entity.Route)
            .Set("remarks", entity.Remarks)
            .Set("fuel_hours", entity.FuelHours)
            .Set("fuel_minutes", entity.FuelMinutes)
            .Set("number_onboard", entity.NumberOnBoard);

        UpdateResult result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if (result.MatchedCount == 0)
        {
            return TransactionResult.NotFound;
        }

        return result.ModifiedCount > 0
            ? TransactionResult.Success
            : TransactionResult.ServerError;
    }

    /// <inheritdoc />
    public async ValueTask<TransactionResult> Delete(string id, CancellationToken cancellationToken)
    {
        IMongoCollection<BsonDocument> collection = _context.GetCollection(CollectionName);
        FilterDefinition<BsonDocument> filter = GetFilterForId(id);

        DeleteResult result = await collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
        return result.DeletedCount > 0
            ? TransactionResult.Success
            : TransactionResult.NotFound;
    }

    /// <inheritdoc />
    public ValueTask<TransactionResult> Delete(FlightPlanEntity entity, CancellationToken cancellationToken)
    {
        return Delete(entity.FlightPlanId, cancellationToken);
    }

    private static FilterDefinition<BsonDocument> GetFilterForId(string id)
    {
        return Builders<BsonDocument>.Filter.Eq("flight_plan_id", id);
    }

    [return: NotNullIfNotNull("document")]
    private static FlightPlanEntity? ToFlightPlanOrNull(BsonDocument? document)
    {
        if (document == null)
        {
            return null;
        }

        return new FlightPlanEntity
        {
            FlightPlanId = document["flight_plan_id"].AsString,
            Altitude = document["altitude"].AsInt32,
            Airspeed = document["airspeed"].AsInt32,
            AircraftIdentification = document["aircraft_identification"].AsString,
            AircraftType = document["aircraft_type"].AsString,
            ArrivalAirport = document["arrival_airport"].AsString,
            FlightType = document["flight_type"].AsString,
            DepartureAirport = document["departing_airport"].AsString,
            DepartureTime = document["departure_time"].AsBsonDateTime.ToUniversalTime(),
            ArrivalTime = document["estimated_arrival_time"].AsBsonDateTime.ToUniversalTime(),
            Route = document["route"].AsString,
            Remarks = document["remarks"].AsString,
            FuelHours = document["fuel_hours"].AsInt32,
            FuelMinutes = document["fuel_minutes"].AsInt32,
            NumberOnBoard = document["number_onboard"].AsInt32
        };
    }
}
