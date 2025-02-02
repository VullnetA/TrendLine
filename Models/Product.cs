using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SolrNet;
using SolrNet.Attributes;
using TrendLine.Enums;

namespace TrendLine.Models
{
    public class Product
    {
        [Key]
        [SolrUniqueKey("id")]
        public int Id { get; set; }

        [SolrField("name")]
        public string Name { get; set; }

        [SolrField("description")]
        public string Description { get; set; }

        [SolrField("price")]
        public double Price { get; set; }

        [SolrField("quantity")]
        public int Quantity { get; set; }

        [SolrField("gender")]
        public Gender Gender { get; set; }

        [SolrField("brand_id")]
        public int BrandId { get; set; }

        [SolrField("category_id")]
        public int CategoryId { get; set; }

        [SolrField("color_id")]
        public int ColorId { get; set; }

        [SolrField("size_id")]
        public int SizeId { get; set; }

        [SolrField("discount_id")]
        public int? DiscountId { get; set; }

        // Navigation properties (Not indexed by Solr)
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
        public Discount Discount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public double GetFinalPrice()
        {
            if (Discount == null || (Discount.ExpirationDate.HasValue && Discount.ExpirationDate < DateTime.UtcNow))
            {
                return Price;
            }

            if (Discount.DiscountPercentage.HasValue)
            {
                return Price * (1 - (Discount.DiscountPercentage.Value / 100));
            }
            else if (Discount.DiscountAmount > 0)
            {
                return Price - (double)Discount.DiscountAmount;
            }

            return Price;
        }
    }
}
