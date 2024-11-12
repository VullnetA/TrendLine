using System.ComponentModel.DataAnnotations;

namespace TrendLine.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    } 
}
