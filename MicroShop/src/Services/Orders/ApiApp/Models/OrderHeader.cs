namespace MicroShop.Services.Orders.ApiApp.Models;

public sealed class OrderHeader
{
    public OrderHeader(string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, IEnumerable<OrderDetails> items)
        : this(userId, couponCode, discount, orderTotal, name, emailAddress, null, null, items)
    {
    }

    public OrderHeader(string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, string? paymentIntentId, string? stripeSessionId, IEnumerable<OrderDetails> items)
        : this(0, userId, couponCode, discount, orderTotal, name, emailAddress, DateTime.UtcNow, paymentIntentId, stripeSessionId)
    {
        Items = items.Distinct().ToHashSet();
        // TODO: validate parameters here
    }

    private OrderHeader(int id, string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, DateTime orderTime, string? paymentIntentId, string? stripeSessionId)
    {
        Id = id;
        UserId = userId;
        CouponCode = couponCode;
        Discount = discount;
        OrderTotal = orderTotal;
        Name = name;
        EmailAddress = emailAddress;
        OrderTime = orderTime;
        PaymentIntentId = paymentIntentId;
        StripeSessionId = stripeSessionId;
    }

    public int Id { get; private init; }
    public string? UserId { get; private init; }
    public string? CouponCode { get; init; }
    public double Discount { get; init; }
    public double OrderTotal { get; init; }
    public string Name { get; init; }
    public string EmailAddress { get; init; }
    public DateTime OrderTime { get; init; }
    
    public string? PaymentIntentId { get; private set; }
    public string? StripeSessionId { get; private set; }

    public HashSet<OrderDetails> Items { get; private set; } = [];

}
