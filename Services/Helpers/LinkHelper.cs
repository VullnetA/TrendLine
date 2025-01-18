using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TrendLine.Models;

public class LinkHelper
{
    private readonly LinkGenerator _linkGenerator;

    public LinkHelper(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public List<Link> GenerateProductLinks(HttpContext httpContext, int productId)
    {
        return new List<Link>
        {
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductById", "Product", new { id = productId }),
                rel: "self",
                method: "GET"
            ),
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetAllProducts", "Product"),
                rel: "all-products",
                method: "GET"
            )
        };
    }
}
