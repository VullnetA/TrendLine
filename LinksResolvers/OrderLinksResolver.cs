using AutoMapper;
using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.LinksResolvers
{
    public class OrderLinksResolver : IValueResolver<Order, OrderDTO, List<Link>>
    {
        private readonly LinkHelper _linkHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderLinksResolver(LinkHelper linkHelper, IHttpContextAccessor httpContextAccessor)
        {
            _linkHelper = linkHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Link> Resolve(Order source, OrderDTO destination, List<Link> destMember, ResolutionContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
                throw new InvalidOperationException("HttpContext is null.");

            // Extract user roles
            var userRoles = httpContext.User.FindAll(System.Security.Claims.ClaimTypes.Role)
                                .Select(role => role.Value)
                                .ToList();

            return _linkHelper.GenerateOrderLinks(httpContext, source.Id, userRoles);
        }
    }
}
