using Domain.Enums;

namespace Domain.Events;

public class OrderStatusChangedEvent
{
    public int OrderId { get; }
    public OrderStatus PreviousStatus { get; }
    public OrderStatus NewStatus { get; }
    public DateTime OccurredAt { get; }

    public OrderStatusChangedEvent(int orderId, OrderStatus previous, OrderStatus newStatus)
    {
        OrderId = orderId;
        PreviousStatus = previous;
        NewStatus = newStatus;
        OccurredAt = DateTime.UtcNow;
    }
}