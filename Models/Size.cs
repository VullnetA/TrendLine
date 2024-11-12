using System.ComponentModel.DataAnnotations;

namespace TrendLine.Models
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string Label { get; set; }
    } 
}
