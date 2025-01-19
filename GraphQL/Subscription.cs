using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("ProductStockUpdated")]
        [GraphQLDescription("Notifies clients when a product's stock is updated.")]
        public ProductDTO OnProductStockUpdated([EventMessage] ProductDTO product)
        {
            return product;
        }
    }
}
