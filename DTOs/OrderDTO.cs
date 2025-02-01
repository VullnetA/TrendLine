using TrendLine.Models;

namespace TrendLine.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public CustomerDTO Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
