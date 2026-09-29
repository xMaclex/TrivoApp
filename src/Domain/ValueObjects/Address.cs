namespace Domain.ValueObjects;

public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    private Address()
    {
        Street = string.Empty;
        City = string.Empty;
        State = string.Empty;
        ZipCode = string.Empty;
    }

    public static Address Create(string street, string city, string state, string zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("calle no puede estar vacía.", nameof(street));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("ciudad no puede estar vacía.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("estado no puede estar vacío.", nameof(state));
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException("código postal no puede estar vacío.", nameof(zipCode));

        return new Address
        {
            Street = street,
            City = city,
            State = state,
            ZipCode = zipCode
        };
    }

    //No se permite registrar dos direcciones iguales para un mismo usuario
    public override bool Equals(object? obj)
    {
        if (obj is not Address other)
            return false;

        return Street == other.Street &&
               City == other.City &&
               State == other.State &&
               ZipCode == other.ZipCode;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Street, City, State, ZipCode);
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State}, {ZipCode}";
    }
}