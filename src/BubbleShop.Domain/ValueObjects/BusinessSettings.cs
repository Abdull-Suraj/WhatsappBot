using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleShop.Domain.ValueObjects
{
    public class BusinessSettings
    {
        public bool AutoConfirmOrders { get; private set; }
        //public int DefaultDeliveryTimeInHours { get; private set; }
        //public decimal FreeDeliveryThreshold { get; private set; }
        //public decimal DeliveryFee { get; private set; }
        public bool EnableWhatsAppNotifications { get; private set; }
        public string WelcomeMessage { get; private set; }
        public string OrderConfirmationMessage { get; private set; }
        public WorkingHours WorkingHours { get; private set; }

        public BusinessSettings()
        {
            AutoConfirmOrders = true;
            //DefaultDeliveryTimeInHours = 48;
            //FreeDeliveryThreshold = 50;
            //DeliveryFee = 5;
            EnableWhatsAppNotifications = true;
            WelcomeMessage = "Welcome to our store! How can we help you today?";
            OrderConfirmationMessage = "Thank you for your order! Your order number is {OrderNumber}";
            WorkingHours = new WorkingHours();
        }

        public void UpdateSettings(
            bool? autoConfirmOrders = null,
            int? defaultDeliveryTimeInHours = null,
            decimal? freeDeliveryThreshold = null,
            decimal? deliveryFee = null,
            bool? enableWhatsAppNotifications = null,
            string welcomeMessage = null,
            string orderConfirmationMessage = null)
        {
            AutoConfirmOrders = autoConfirmOrders ?? AutoConfirmOrders;
            //DefaultDeliveryTimeInHours = defaultDeliveryTimeInHours ?? DefaultDeliveryTimeInHours;
            //FreeDeliveryThreshold = freeDeliveryThreshold ?? FreeDeliveryThreshold;
            //DeliveryFee = deliveryFee ?? DeliveryFee;
            EnableWhatsAppNotifications = enableWhatsAppNotifications ?? EnableWhatsAppNotifications;
            WelcomeMessage = welcomeMessage ?? WelcomeMessage;
            OrderConfirmationMessage = orderConfirmationMessage ?? OrderConfirmationMessage;
        }
    }

    public class WorkingHours
    {
        public bool Is24Hours { get; private set; }
        public TimeOnly OpenTime { get; private set; }
        public TimeOnly CloseTime { get; private set; }
        public List<DayOfWeek> WorkingDays { get; private set; }

        public WorkingHours()
        {
            Is24Hours = true;
            OpenTime = new TimeOnly(9, 0);
            CloseTime = new TimeOnly(21, 0);
            WorkingDays = new List<DayOfWeek>
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday
        };
        }

        public bool IsOpen(DateTime time)
        {
            if (Is24Hours) return true;
            if (!WorkingDays.Contains(time.DayOfWeek)) return false;

            var timeOnly = TimeOnly.FromDateTime(time);
            return timeOnly >= OpenTime && timeOnly <= CloseTime;
        }
    }
}
