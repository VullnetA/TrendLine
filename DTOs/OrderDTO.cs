namespace TrendLine.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }
}
