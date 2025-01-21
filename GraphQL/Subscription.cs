using TrendLine.DTOs;

namespace TrendLine.GraphQL
{
    public class Subscription
    {
        [Subscribe]
        [Topic("ProductStockUpdated")]
        [GraphQLDescription("Notifies clients when a product's stock is updated.")]
        public ProductDTO OnProductStockUpdated([EventMessage] ProductDTO product)
        {
            if (product == null)
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Received null product data.")
                        .SetCode("INVALID_DATA")
                        .SetExtension("details", "The product payload is null or invalid.")
                        .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                        .Build()
                );

            }
            return product;
        }
    }
}
