using Domain.ValueObjects;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public Address? ShippingAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    private Customer() { }

    // Factory method — la única forma de crear un Customer válido

    public static Customer Create(string name, string email, string phone, Address adress)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El correo electrónico no puede estar vacío.", nameof(email));
        
        return new Customer
        {
            Name = name,
            Email = email,
            Phone = phone,
            ShippingAddress = adress,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateContact(string name, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("El teléfono no puede estar vacío.", nameof(phone));

        Name = name;
        Phone = phone;
    }

    public void UpdateAddress(Address newAddress)
    {
        if (newAddress == null)
            throw new ArgumentNullException(nameof(newAddress), "La dirección no puede ser nula.");

        ShippingAddress = newAddress;
    }
}
    



