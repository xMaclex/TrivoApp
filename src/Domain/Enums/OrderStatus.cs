namespace Domain.Enums;

public enum OrderStatus
{
    Pending,    // Pedido creado, esperando confirmación
    Confirmed,  // Pedido confirmado
    Shipped,    // Pedido enviado
    Delivered,  // Pedido entregado
    Cancelled  // Pedido cancelado
}