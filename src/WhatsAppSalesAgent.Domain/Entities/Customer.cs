namespace WhatsAppSalesAgent.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string WhatsAppNumber { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Address { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    private Customer() { }

    public static Customer Create(string whatsAppNumber, string name, string? email = null, string? address = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(whatsAppNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Customer
        {
            Id = Guid.NewGuid(),
            WhatsAppNumber = whatsAppNumber,
            Name = name,
            Email = email,
            Address = address,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? email, string? address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Email = email;
        Address = address;
    }
}
