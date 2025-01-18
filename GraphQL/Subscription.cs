using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("ProductStockUpdated")]
        public ProductDTO OnProductStockUpdated([EventMessage] ProductDTO product)
        {
            return product;
        }
    }
}
