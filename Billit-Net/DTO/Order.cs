using System;
using System.Collections.Generic;

namespace Billit_Net.DTO
{
    public class OrderPDF
    {
        public string FileID { get; set; }
    }

    public class CounterParty
    {
        public int PartyID { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public string PartyType { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return $"{DisplayName} {PartyType}";
        }
    }

    public class Order
    {
        public int OrderID { get; set; }
        public OrderPDF OrderPDF { get; set; }
        public CounterParty CounterParty { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string OrderType { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }
        public string OrderDirection { get; set; }
        public double TotalExcl { get; set; }
        public double TotalIncl { get; set; }
        public double TotalVAT { get; set; }
        public string PaymentReference { get; set; }
        public bool Paid { get; set; }
        public int RemindersSent { get; set; }
        public double ToPay { get; set; }
        public string OrderStatus { get; set; }
        public bool Overdue { get; set; }
        public int DaysOverdue { get; set; }
        public double FXRateToForeign { get; set; }
        public DateTime? LastReminder { get; set; }
        public string OrderTitle { get; set; }
        public string Reference { get; set; }

        public override string ToString()
        {
            return $"{OrderID} {OrderType} {OrderDirection}";
        }
    }

    public class OrderCollection
    {
        public List<Order> Items { get; set; }
        public string NextPageLink { get; set; }
    }
}