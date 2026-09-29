using Domain.ValueObjects;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities;

// Order es el Aggregate Root del dominio de pedidos
// Toda modificación a OrderDetail pasa por aquí

public class Order
{
    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public Address ShippingAddress { get; private set; } = null!;
    public Money Total { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<OrderDetail> _details = new();
    public IReadOnlyCollection<OrderDetail> Details => _details.AsReadOnly();

    // Domain Events pendientes de publicar
    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    private Order() { }

    // Factory method — valida que el pedido sea creado correctamente

    public static Order Create(Customer customer, Address shippingAddress, string? notes = null)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));
        

        var order = new Order
        {
            CustomerId = customer.Id,
            Customer = customer,
            Status = OrderStatus.Pending,
            ShippingAddress = shippingAddress,
            Total = Money.Create(0), // Inicializa el total en 0 con la moneda del shippingAddress            
            CreatedAt = DateTime.UtcNow,
            Notes = notes
        };

        return order;

    }
        // Agregar un producto al pedido — lógica de negocio en el aggregate

    public void AddDetail(Product product, int quantity)
    {
        if(Status != OrderStatus.Pending)
            throw new InvalidOperationException("No se pueden agregar detalles a un pedido que no está pendiente.");

        // Verificar Stock del producto
        product.ReduceStock(quantity);

        var detail = OrderDetail.Create(product, quantity);
        _details.Add(detail);

        // Recalcular el total del pedido
        RecalculateTotal();
    }

    // Cambiar estado con reglas de negocio

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Solo se puede confirmar un pedido que está pendiente.");

        if (!_details.Any())
            throw new InvalidOperationException("el pedido no tiene productos.");

        var previousStatus = Status;
        Status = OrderStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;

        // Disparar domain event
        _domainEvents.Add(new OrderCreatedEvent(Id, CustomerId, Total.Amount));
        _domainEvents.Add(new OrderStatusChangedEvent(Id, previousStatus, Status));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Solo se puede enviar un pedido que está confirmado.");

        var previousStatus = Status;
        Status = OrderStatus.Shipped;
        UpdatedAt = DateTime.UtcNow;

        // Disparar domain event
        _domainEvents.Add(new OrderStatusChangedEvent(Id, previousStatus, Status));
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException("Solo se puede entregar un pedido que está enviado.");

        var previousStatus = Status;
        Status = OrderStatus.Delivered;
        UpdatedAt = DateTime.UtcNow;

        // Disparar domain event
        _domainEvents.Add(new OrderStatusChangedEvent(Id, previousStatus, Status));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("No se puede cancelar un pedido que ya ha sido entregado.");

        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("El pedido ya está cancelado.");

        // Restaurar stock de los productos
        foreach (var detail in _details)
        {
            detail.Product?.RestoreStock(detail.Quantity);
        }
         var previousStatus = Status;
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        

        // Disparar domain event
        _domainEvents.Add(new OrderStatusChangedEvent(Id, previousStatus, Status));
    }

    // Limpiar eventos después de publicarlos
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void RecalculateTotal()
    {
        var total = _details.Sum(d => d.Subtotal.Amount);
        Total = Money.Create(total);
    }
}