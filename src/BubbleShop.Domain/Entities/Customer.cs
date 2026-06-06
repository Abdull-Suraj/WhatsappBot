using BubbleShop.Domain.Common;

namespace BubbleShop.Domain.Entities;

public class Customer : BaseEntity
{
    public Guid BusinessId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; }
    public string WhatsAppNumber { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string? Address { get; private set; }
    public CustomerStatus Status { get; private set; }
    public int TotalOrders { get; private set; }
    public decimal TotalSpent { get; private set; }
    public DateTime? LastOrderDate { get; private set; }
    public string Notes { get; private set; }
    //public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    // Navigation Properties
    public Business Business { get; private set; }
    private readonly List<Order> _orders = new();
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();


    private Customer() { }

    public  Customer (
        Guid businessId,
        string name,
        string whatsAppNumber,
        string phoneNumber = null
        )
    {
        BusinessId = businessId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        WhatsAppNumber = whatsAppNumber ?? throw new ArgumentNullException(nameof(whatsAppNumber));
        PhoneNumber = phoneNumber ?? whatsAppNumber;
        Status = CustomerStatus.Active;
        TotalOrders = 0;
        TotalSpent = 0;
    }

    public void Update(
        string name, 
        string? email, 
        string? address,
        string city,
        string state,
        string phoneNumber       
        )
    {
        
        Name = name;
        Email = email;
        Address = address;
        City = city;
                State = state;  
        PhoneNumber = phoneNumber;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void RecordOrder(decimal orderAmount)
    {
        TotalOrders++;
        TotalSpent += orderAmount;
        LastOrderDate = DateTime.UtcNow;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Block()
    {
        Status = CustomerStatus.Blocked;
        LastModifiedAt = DateTime.UtcNow;
    }

    public void Unblock()
    {
        Status = CustomerStatus.Active;
        LastModifiedAt = DateTime.UtcNow;
    }

}
public enum CustomerStatus
{
    Active,
    Inactive,
    Blocked
}