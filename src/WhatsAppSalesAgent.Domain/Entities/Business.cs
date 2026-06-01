


using WhatsAppSalesAgent.Domain.Common;
using WhatsAppSalesAgent.Domain.Events;
using WhatsAppSalesAgent.Domain.Exceptions;
using WhatsAppSalesAgent.Domain.ValueObjects;

namespace WhatsAppSalesAgent.Domain.Entities
{

    public class Business : BaseEntity
    {
        public string BusinessName { get; private set; }
        public string WhatsAppNumber { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string LegalName { get; private set; }
        public string PasswordHash { get; private set; }
        public string Address { get; private set; }
        public bool IsVerified { get; private set; }
        public decimal WalletBalance { get; private set; }


        // Business Settings
        public BusinessStatus Status { get; private set; }
        public BusinessType BusinessType { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }
        public DateTime? VerifiedAt { get; private set; }



        // Settings
        public BusinessSettings Settings { get; private set; }


        // Navigation Properties
        private readonly List<Product> _products = new();
        private readonly List<Customer> _customers = new();
        private readonly List<Order> _orders = new();
        //private readonly List<Delivery> _deliveries = new();
        private readonly List<Payment> _payments = new();

        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();
        public IReadOnlyCollection<Customer> Customers => _customers.AsReadOnly();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        //public IReadOnlyCollection<Delivery> Deliveries => _deliveries.AsReadOnly();
        public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

        private Business() { } 

        public Business(
            string businessName,
            string whatsAppNumber, 
            string email, 
            string passwordHash,
            BusinessType businessType = BusinessType.Individual
            )
        {
           
            BusinessName = businessName ?? throw new ArgumentNullException(nameof(businessName));
            WhatsAppNumber = whatsAppNumber ?? throw new ArgumentNullException(nameof(whatsAppNumber));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            BusinessType = businessType;
            Status = BusinessStatus.Pending;
            IsEmailVerified = false;
            IsPhoneVerified = false;
            IsVerified = false;
            WalletBalance = 0;
            CreatedAt = DateTime.UtcNow;
            Settings = new BusinessSettings();
        }

        public void UpdateProfile(
            string businessName, 
            string whatsAppNumber, 
            string email,
            string address,
            string legalName,
            string phoneNumber)
        {
            BusinessName = businessName;
            LegalName = legalName;
            WhatsAppNumber = whatsAppNumber;
            Email = email;
            Address = address;
            PhoneNumber = phoneNumber;
            LastModifiedAt = DateTime.UtcNow;
            
        }

        public void Verify() => IsVerified = true;

        public void AddToWallet(decimal amount)
        {
            if (amount <= 0) throw new DomainException("Amount must be positive");
            WalletBalance += amount;
        }
        public void VerifyBusiness()
        {
            Status = BusinessStatus.Active;
            VerifiedAt = DateTime.UtcNow;
            IsEmailVerified = true;
            AddDomainEvent(new BusinessVerifiedEvent(Id, BusinessName));
        }
        public void Suspend()
        {
            Status = BusinessStatus.Suspended;
            LastModifiedAt = DateTime.UtcNow;
            AddDomainEvent(new BusinessSuspendedEvent(Id, BusinessName));
        }

        public void Activate()
        {
            Status = BusinessStatus.Active;
            LastModifiedAt = DateTime.UtcNow;
        }
        public void VerifyWhatsApp()
        {
            IsPhoneVerified = true;
            LastModifiedAt = DateTime.UtcNow;
        }
        public void AddToWallet(decimal amount, string description = null)
        {
            if (amount <= 0) throw new DomainException("Amount must be positive");
            WalletBalance += amount;
            LastModifiedAt = DateTime.UtcNow;

            AddDomainEvent(new WalletCreditedEvent(Id, amount, WalletBalance, description));
        }

        public void DeductFromWallet(decimal amount, string description = null)
        {
            if (amount <= 0) throw new DomainException("Amount must be positive");
            if (WalletBalance < amount) throw new DomainException("Insufficient wallet balance");

            WalletBalance -= amount;
            LastModifiedAt = DateTime.UtcNow;

            AddDomainEvent(new WalletDebitedEvent(Id, amount, WalletBalance, description));
        }


        public void UpdateWhatsAppNumber(string whatsAppNumber)
        {
            WhatsAppNumber = whatsAppNumber;
            IsPhoneVerified = false;
            LastModifiedAt = DateTime.UtcNow;
        }
        public void UpdateSettings(BusinessSettings settings)
        {
            Settings = settings;
            LastModifiedAt = DateTime.UtcNow;
        }

    }

    public enum BusinessStatus
    {
        Pending,
        Active,
        Suspended,
        Banned
    }

    public enum BusinessType
    {
        Individual,
        SoleProprietorship,
        Partnership,
        LimitedLiabilityCompany,
        Corporation
    }

}
