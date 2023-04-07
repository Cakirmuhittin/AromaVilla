using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Order:BaseEntity
    {
        public string BuyerId = null!;
        public Address ShippingAdress { get; set; } = null!;
        public DateTimeOffset OrderData { get; set; }=DateTimeOffset.Now;
        public List<OrderItem> Items { get; set; } = new();
    }
}
