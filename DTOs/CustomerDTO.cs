namespace TrendLine.DTOs
{
    public class CustomerDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Other properties as needed
        public ICollection<OrderDTO> Orders { get; set; }
    }
}
