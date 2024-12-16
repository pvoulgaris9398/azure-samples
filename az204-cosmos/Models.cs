using System.Text.Json;

namespace az204cosmos
{
    public class Order
    {
        public string? Id { get; set; }
        public string? OrderNumber { get; set; }
        public Address? OrderAddress { get; set; }
        public Customer? OrderCustomer { get; set; }
        public OrderItem[]? OrderItems { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class Product
    {
        public string? ProductName { get; set; }
    }

    public class Address
    {
        public string? State { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
    }
    public class OrderItem
    {
        public Product? ProductItem { get; set; }
        public int Count { get; set; }
    }
}
