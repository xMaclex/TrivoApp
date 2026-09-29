namespace Domain.Events;

// Un Domain Event es algo que ocurrió en el pasado
// Siempre se nombra en pasado: OrderCreated, UserRegistered, etc.

public class OrderCreatedEvent
{
    public int OrderId { get; }
    public int CostumerId { get; }
    public decimal TotalAmount { get; }
    public DateTime OccurredAt { get; }


public OrderCreatedEvent(int orderId, int costumerId, decimal totalAmount)

    {
        OrderId = orderId;
        CostumerId = costumerId;
        TotalAmount = totalAmount;
        OccurredAt = DateTime.UtcNow;
    }
}

