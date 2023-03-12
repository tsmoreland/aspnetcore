using System.Runtime.CompilerServices;
using CarvedRock.Domain.Entities;
using CarvedRock.Domain.ValueObjects;
using CarvedRock.Infrastructure.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CarvedRock.Infrastructure;

public sealed class CarvedRockDbContext : DbContext
{
    private readonly IModelConfiguration<ModelBuilder, DbContextOptionsBuilder, ModelConfigurationBuilder> _configuration;

    /// <inheritdoc />
    public CarvedRockDbContext(DbContextOptions options, IModelConfiguration<ModelBuilder, DbContextOptionsBuilder, ModelConfigurationBuilder> configuration)
        : base(options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public DbSet<Order> Orders { get; init; } = default!;

    public DbSet<Customer> Customers { get; init; } = default!;

    public DbSet<Item> Items { get; init; } = default!;

    public DbSet<Dictionary<string, object?>> Tags => Set<Dictionary<string, object?>>("Tag");

    public DbSet<CustomerOrder> CustomerOrders { get; init; } = default!;

    public IQueryable<CustomerOrder> GetCustomerOrdersById(int customerId)
    {
        return FromExpression(() => GetCustomerOrdersById(customerId));
    }

    public IQueryable<StatusOverview> GetStatusOverview()
    {
        return FromExpression(() => GetStatusOverview());
    }

    /// <summary>
    /// Configuration will tie this method to the db function so this implementation should never be called
    /// </summary>
    /// <param name="orderId">order id to retrieve total invoice amount for</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">
    /// thrown if misconfigured or database doesn't support functions
    /// </exception>
    public int GetTotalInvoceAmountByOrder(int orderId)
    {
        throw new NotSupportedException();
    }

    public async IAsyncEnumerable<Order> GetOrders([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ConfiguredCancelableAsyncEnumerable<Order> orders = Orders
            .FromSqlInterpolated($"GetOrders")
            .AsAsyncEnumerable()
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false);
        await foreach (Order order in orders)
        {
            yield return order;
        }
    }

    public (IAsyncEnumerable<Order> Orders, int Count) GetOrdersByCustomerId(int customerId, CancellationToken cancellationToken)
    {
        SqlParameter parameter = new() { ParameterName = "customer_order_count", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

        IQueryable<Order> orders = Orders.FromSqlInterpolated($@"GetOrdersBy @customer_id = {customerId}, @customer_order_count = {parameter} OUTPUT");

        return (GetOrdersFromQueryable(orders, cancellationToken), (int)parameter.Value);

        static async IAsyncEnumerable<Order> GetOrdersFromQueryable(IQueryable<Order> orders, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            ConfiguredCancelableAsyncEnumerable<Order> ordersEnumerable = orders
                .AsAsyncEnumerable()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false);
            await foreach (Order order in ordersEnumerable)
            {
                yield return order;
            }
        }
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {_configuration.ConfigureModel(modelBuilder);
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {_configuration.ConfigureContext(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {_configuration.ConfigureConventions(configurationBuilder);
        base.ConfigureConventions(configurationBuilder);
    }
}
