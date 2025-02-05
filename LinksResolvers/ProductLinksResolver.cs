using AutoMapper;
using TrendLine.DTOs;
using TrendLine.Models;
using System.Security.Claims;

namespace TrendLine.LinksResolvers
{
    public class ProductLinksResolver : IValueResolver<Product, ProductDTO, List<Link>>
    {
        private readonly LinkHelper _linkHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductLinksResolver(LinkHelper linkHelper, IHttpContextAccessor httpContextAccessor)
        {
            _linkHelper = linkHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Link> Resolve(Product source, ProductDTO destination, List<Link> destMember, ResolutionContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext is null.");
            }

            // Default behavior: do NOT include links.
            bool includeLinks = false;
            if (context.TryGetItems != null &&
                context.Items.TryGetValue("IncludeLinks", out var includeLinksObj) &&
                includeLinksObj is bool flag)
            {
                includeLinks = flag;
            }

            // If links are not explicitly enabled, return an empty list.
            if (!includeLinks)
            {
                return new List<Link>();
            }

            // Extract user roles from the HTTP context.
            var userRoles = httpContext.User
                .FindAll(ClaimTypes.Role)
                .Select(role => role.Value)
                .ToList();

            // Check for a single-product context using the "IsSingleProduct" flag.
            if (context.TryGetItems != null &&
                context.Items.TryGetValue("IsSingleProduct", out var isSingleProductObj) &&
                isSingleProductObj is bool singleProduct && singleProduct)
            {
                // Generate links for a single product.
                return _linkHelper.GenerateProductLinksForSingleProduct(
                    httpContext,
                    source.Id,
                    source.Category.Name,
                    source.Brand.Name,
                    source.Gender.ToString(),
                    source.Size.Label,
                    source.Color.Name,
                    userRoles
                );
            }

            // Generate links for the all-products context.
            return _linkHelper.GenerateProductLinksForAllProducts(
                httpContext,
                source.Id,
                source.Category.Name,
                userRoles
            );
        }
    }
}
