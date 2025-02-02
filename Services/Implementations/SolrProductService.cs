using SolrNet.Commands.Parameters;
using SolrNet;
using TrendLine.Models;

namespace TrendLine.Services.Implementations
{
    public class SolrProductService
    {
        private readonly ISolrOperations<Product> _solr;

        public SolrProductService(ISolrOperations<Product> solr)
        {
            _solr = solr;
        }

        /// <summary>
        /// Adds or updates a product in the Solr index.
        /// </summary>
        public async Task AddOrUpdateProduct(Product product)
        {
            // Create a new product specifically for Solr indexing
            var solrProduct = new Product
            {
                Id = product.Id, // SolrNet will handle the conversion
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Gender = product.Gender,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ColorId = product.ColorId,
                SizeId = product.SizeId,
                DiscountId = product.DiscountId
            };

            await _solr.AddAsync(solrProduct);
            await _solr.CommitAsync();
        }

        /// <summary>
        /// Deletes a product from the Solr index using its ID.
        /// </summary>
        public async Task DeleteProduct(int productId)
        {
            await _solr.DeleteAsync(productId.ToString());
            await _solr.CommitAsync();
        }

        /// <summary>
        /// Searches for products in Solr using a query string.
        /// Supports full-text search and filtering.
        /// </summary>
        public async Task<List<Product>> SearchProducts(
    string query, int? categoryId = null, int? brandId = null,
    double? minPrice = null, double? maxPrice = null,
    string sortBy = "relevance", bool fuzzy = false)
        {
            string solrQueryString = fuzzy ? $"{query}~" : query; // Add `~` for fuzzy search
            var solrQuery = new SolrQuery(solrQueryString);

            var queryOptions = new QueryOptions
            {
                FilterQueries = new List<ISolrQuery>(),
                Rows = 20, // Limit results per page
                OrderBy = new List<SortOrder>()
            };

            // Apply category filter
            if (categoryId.HasValue)
            {
                queryOptions.FilterQueries.Add(new SolrQueryByField("category_id", categoryId.Value.ToString()));
            }

            // Apply brand filter
            if (brandId.HasValue)
            {
                queryOptions.FilterQueries.Add(new SolrQueryByField("brand_id", brandId.Value.ToString()));
            }

            // Apply price range filter
            if (minPrice.HasValue || maxPrice.HasValue)
            {
                double min = minPrice ?? 0; // Default min = 0
                double max = maxPrice ?? double.MaxValue; // Default max = unlimited
                queryOptions.FilterQueries.Add(new SolrQueryByRange<double>("price", min, max));
            }

            // Sorting Options
            switch (sortBy.ToLower())
            {
                case "price_asc":
                    queryOptions.OrderBy.Add(new SortOrder("price", SolrNet.Order.ASC));
                    break;
                case "price_desc":
                    queryOptions.OrderBy.Add(new SortOrder("price", SolrNet.Order.DESC));
                    break;
                case "relevance":
                default:
                    queryOptions.OrderBy.Add(SortOrder.Parse("score desc")); // Default sorting by relevance
                    break;
            }

            var results = await _solr.QueryAsync(solrQuery, queryOptions);
            return results;
        }

    }
}
