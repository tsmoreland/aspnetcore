namespace MicroShop.Services.Orders.App.Models;

public sealed class OrderHeader
{
    public OrderHeader(string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, IEnumerable<OrderDetails> items)
        : this(userId, couponCode, discount, orderTotal, name, emailAddress, null, null, items)
    {
    }

    public OrderHeader(string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, string? paymentIntentId, string? stripeSessionId, IEnumerable<OrderDetails> items)
        : this(0, userId, OrderStatus.Pending, couponCode, discount, orderTotal, name, emailAddress, DateTime.UtcNow, paymentIntentId, stripeSessionId)
    {
        Items = items.Distinct().ToHashSet();
        // TODO: validate parameters here
    }

    private OrderHeader(int id, string userId, OrderStatus status, string? couponCode, double discount, double orderTotal, string name, string emailAddress, DateTime orderTime, string? paymentIntentId, string? stripeSessionId)
    {
        Id = id;
        UserId = userId;
        Status = status;
        CouponCode = couponCode;
        Discount = discount;
        OrderTotal = orderTotal;
        Name = name;
        EmailAddress = emailAddress;
        OrderTime = orderTime;
        PaymentIntentId = paymentIntentId;
        StripeSessionId = stripeSessionId;
    }

    public static OrderHeader CreateOrder<T>(string userId, string? couponCode, double discount, double orderTotal, string name, string emailAddress, IEnumerable<T> items, Func<OrderHeader, T, OrderDetails> builder)
    {
        OrderHeader header = new(userId, couponCode, discount, orderTotal, name, emailAddress, []);
        header.Items = items.Select(i => builder(header, i)).ToHashSet();
        return header;
    }


    public int Id { get; private init; }
    public string? UserId { get; private init; }
    public string? CouponCode { get; private init; }
    public double Discount { get; private init; }
    public double OrderTotal { get; private init; }
    public string Name { get; private init; }
    public string EmailAddress { get; private init; }
    public OrderStatus Status { get; private set; }
    public DateTime OrderTime { get; init; }

    public string? PaymentIntentId { get; private set; }
    public string? StripeSessionId { get; private set; }

    public HashSet<OrderDetails> Items { get; private set; } = [];

    public bool CanUpdateStatus(OrderStatus status)
    {
        return CanUpdateStatus(status, Status);
    }

    public static bool CanUpdateStatus(OrderStatus newStatus, OrderStatus currentStatus)
    {
        // consider state pattern here

        return newStatus switch
        {
            OrderStatus.Cancelled => currentStatus != OrderStatus.WaitingToShip &&
                                     currentStatus != OrderStatus.Shipping && currentStatus != OrderStatus.Complete &&
                                     currentStatus != OrderStatus.Cancelled,
            OrderStatus.Approved => currentStatus == OrderStatus.Pending,
            OrderStatus.Processing => currentStatus == OrderStatus.Approved,
            OrderStatus.WaitingToShip => currentStatus == OrderStatus.Processing,
            OrderStatus.Shipping => currentStatus == OrderStatus.WaitingToShip,
            OrderStatus.Complete => currentStatus == OrderStatus.Shipping,
            OrderStatus.Pending => false,
            _ => false
        };
    }

    public bool TrySetStripeDetails(string stripeSessionId, string paymentIntentId)
    {
        // can't set to empty and can't set if already set
        if (stripeSessionId is not { Length: > 0 } || StripeSessionId is not { Length: > 0 })
        {
            return false;
        }

        StripeSessionId = stripeSessionId;
        PaymentIntentId = paymentIntentId;
        if (CanUpdateStatus(OrderStatus.Approved))
        {
            Status = OrderStatus.Approved;
        }
        return true;
    }

    public bool TryUpdateOrderStatus(OrderStatus status)
    {
        if (!CanUpdateStatus(status))
        {
            return false;
        }

        Status = status;
        return true;
    }


    public bool RefundRequiredOnCancel()
    {
        return (int)Status >= (int)OrderStatus.Approved;
    }

    public void SetPaymentIntentIdIfPending(string paymentIntentId)
    {
        if (Status != OrderStatus.Pending || PaymentIntentId is { Length: > 0 } || paymentIntentId is not { Length: > 0 })
        {
            return;
        }

        PaymentIntentId = paymentIntentId;
        Status = OrderStatus.Approved;
    }
}
