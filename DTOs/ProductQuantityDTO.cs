namespace TrendLine.DTOs
{
    public class ProductQuantityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InitialQuantity { get; set; }
        public int SoldQuantity { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
