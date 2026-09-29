using Domain.ValueObjects;

namespace Domain.Entities;

public class Product
{
    public int Id { get; private set;}
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public Money Price { get; private set; } = null!;
    public int Stock{ get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    public static Product Create(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(name));
        if (price <= 0)
            throw new ArgumentNullException( "El precio tiene que ser mayor a 0.");
        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.", nameof(stock));

        return new Product
        {
            Name = name,
            Description = description,
            Price = Money.Create(price),
            Stock = stock,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Lógica de negocio dentro de la entidad
    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad a reducir debe ser mayor que cero.");
        if (Stock < quantity)
            throw new InvalidOperationException("No hay suficiente stock para reducir la cantidad solicitada.");

        Stock -= quantity;
    }

    public void RestoreStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad a restaurar debe ser mayor que cero.");

        Stock += quantity;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("El nuevo precio debe ser mayor que cero.", nameof(newPrice));

        Price = Money.Create(newPrice);
    }

    public void Desactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }

}