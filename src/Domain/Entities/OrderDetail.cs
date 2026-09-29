using Domain.ValueObjects;

namespace Domain.Entities;


// OrderDetail no es un Aggregate Root — se accede siempre a través de Order
public class OrderDetail
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public string? ProductName { get; private set; } // snapshot del nombre al momento de comprar
    public Money UnitPrice { get; private set; } = null!; // snapshot del precio al momento de comprar
    public int Quantity { get; private set; }
    public Money Subtotal { get; private set; } = null!;
    public Product? Product { get; private set; } = null!; // navegación opcional al producto, si aún existe

    private OrderDetail() { }

    // Factory method — la única forma de crear un OrderDetail válido
    public static OrderDetail Create(Product product, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor que cero.");
        
        if (!product.IsActive)
            throw new InvalidOperationException($"El producto '{product.Name}' no está Disponible.");

        var unitPrice = product.Price;
        var subtotal = unitPrice.Multiply(quantity);

        return new OrderDetail
        {
            ProductId = product.Id,
            ProductName = product.Name,  //Guarda el nombre actual
            UnitPrice = unitPrice,       //Guarda el precio actual
            Quantity = quantity,
            Subtotal = subtotal
        };
    }
}