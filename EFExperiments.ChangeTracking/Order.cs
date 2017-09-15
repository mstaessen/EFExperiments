using System;
using System.Collections.Generic;
using System.Linq;

namespace EFExperiments.ChangeTracking
{
    public class Order
    {
        public Guid Id { get; protected set; }

        public DateTime Date { get; protected set; }

        public string ShippingAddress { get; protected set; }

        public virtual ICollection<OrderLine> Lines { get; protected set; }

        public virtual ICollection<OrderLine2> Lines2 { get; protected set; }

        protected Order() { }

        public Order(string shippingAddress)
        {
            Id = Guid.NewGuid();
            Date = DateTime.UtcNow;
            ShippingAddress = shippingAddress;
        }

        public void UpdateShippingAddress(string address)
        {
            ShippingAddress = address;
        }

        public void AddLine(Product product, int amount)
        {
            var nextLineId = Lines != null && Lines.Any() ? Lines.Max(x => x.LineNumber) + 1 : 1;
            if (Lines == null) {
                Lines = new HashSet<OrderLine>();
            }
            Lines.Add(new OrderLine(Id, nextLineId, product.Id, product.Name, product.Price, amount));
        }

        public void RemoveLine(OrderLine line)
        {
            Lines?.Remove(line);
        }

        public void AddLine2(Product product, int amount)
        {
            var nextLineId = Lines2 != null && Lines2.Any() ? Lines2.Max(x => x.LineNumber) + 1 : 1;
            if (Lines2 == null) {
                Lines2 = new HashSet<OrderLine2>();
            }
            Lines2.Add(new OrderLine2(Id, nextLineId, product.Id, product.Name, product.Price, amount));
        }

        public void RemoveLine2(OrderLine2 line)
        {
            Lines2?.Remove(line);
        }
    }

    public class OrderLine
    {
        private decimal unitPrice;
        private int amount;

        public Guid OrderId { get; set; }

        public int LineNumber { get; set; }

        public string Description { get; set; }

        public Guid ProductId { get; set; }

        public decimal UnitPrice {
            get => unitPrice;
            set {
                unitPrice = value;
                LinePrice = unitPrice * Amount;
            }
        }

        public int Amount {
            get => amount;
            set {
                amount = value;
                LinePrice = UnitPrice * amount;
            }
        }

        public decimal LinePrice { get; protected set; }

        protected OrderLine() { }

        public OrderLine(Guid orderId, int lineNumber, Guid productId, string description, decimal unitPrice, int amount)
        {
            OrderId = orderId;
            LineNumber = lineNumber;
            ProductId = productId;
            Description = description;
            UnitPrice = unitPrice;
            Amount = amount;
        }
    }

    public class OrderLine2
    {
        private decimal unitPrice;
        private int amount;

        public Guid Id { get; protected set; }

        public Guid OrderId { get; protected set; }

        public int LineNumber { get; protected set; }

        public string Description { get; protected set; }

        public Guid ProductId { get; protected set; }

        public decimal UnitPrice {
            get => unitPrice;
            set {
                unitPrice = value;
                LinePrice = unitPrice * Amount;
            }
        }

        public int Amount {
            get => amount;
            set {
                amount = value;
                LinePrice = UnitPrice * amount;
            }
        }

        public decimal LinePrice { get; protected set; }

        protected OrderLine2() { }

        public OrderLine2(Guid orderId, int lineNumber, Guid productId, string description, decimal unitPrice, int amount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            LineNumber = lineNumber;
            ProductId = productId;
            Description = description;
            UnitPrice = unitPrice;
            Amount = amount;
        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        protected Product() { }

        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }
    }
}