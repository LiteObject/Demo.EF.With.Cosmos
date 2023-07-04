namespace Demo.EF.With.Cosmos.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? TrackingNumber { get; set; }
        public string PartitionKey { get; set; }
        public StreetAddress ShippingAddress { get; set; }
    }
}
