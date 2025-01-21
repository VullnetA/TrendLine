﻿using TrendLine.Models;

using TrendLine.Models;

public class LinkHelper
{
    private readonly LinkGenerator _linkGenerator;

    public LinkHelper(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public List<Link> GenerateProductLinksForAllProducts(HttpContext httpContext, int productId, string category, IEnumerable<string> userRoles)
    {
        if (httpContext == null)
        {
            Console.WriteLine("HttpContext is null in GenerateProductLinks");
            throw new InvalidOperationException("HttpContext is null.");
        }

        var links = new List<Link>
        {
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductById", "Product", new { id = productId }),
                rel: "self",
                method: "GET"
            ),
        };

        // Add link for viewing products in the same category if the user is a Customer
        if (userRoles.Contains("Customer"))
        {
            links.Add(new Link(
            href: _linkGenerator.GetUriByAction(httpContext, "CreateOrder", "Order"),
            rel: "create-order",
            method: "POST"
        ));
        }

        Console.WriteLine($"Links generated for product ID {productId}: {links.Count}");
        return links;
    }

    public List<Link> GenerateProductLinksForSingleProduct(HttpContext httpContext, int productId, string category, string brand, string gender, string size,
        string color,IEnumerable<string> userRoles)
    {
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null.");

        var links = new List<Link>
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

        // Admin: Add link to delete the product
        if (userRoles.Contains("Admin") || userRoles.Contains("Advanced User"))
        {
            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "DeleteProduct", "Product", new { id = productId }),
                rel: "delete",
                method: "DELETE"
            ));

            links.Add(new Link(
            href: _linkGenerator.GetUriByAction(httpContext, "UpdateProduct", "Product", new { id = productId }),
            rel: "update",
            method: "PUT"
            ));
        }

        // Customer: Add link to view products in the same category
        if (userRoles.Contains("Customer"))
        {
            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductsByCategory", "Product", new { category }),
                rel: "category-products",
                method: "GET"
            ));

            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductsByBrand", "Product", new { brand }),
                rel: "brand-products",
                method: "GET"
            ));

            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductsByGender", "Product", new { gender }),
                rel: "gender-products",
                method: "GET"
            ));

            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductsBySize", "Product", new { size }),
                rel: "size-products",
                method: "GET"
            ));

            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetProductsByColor", "Product", new { color }),
                rel: "color-products",
                method: "GET"
            ));
        }

        return links;
    }

    public List<Link> GenerateOrderLinks(HttpContext httpContext, int orderId, IEnumerable<string> userRoles)
    {
        if (httpContext == null)
            throw new InvalidOperationException("HttpContext is null.");

        var links = new List<Link>
        {
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetOrderById", "Order", new { id = orderId }),
                rel: "self",
                method: "GET"
            ),
            new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "GetOrderItems", "Order", new { orderId }),
                rel: "order-items",
                method: "GET"
            )
        };

        // Add additional links based on roles
        if (userRoles.Contains("Admin") || userRoles.Contains("Advanced User"))
        {
            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "UpdateOrderStatus", "Order", new { id = orderId }),
                rel: "update-status",
                method: "PUT"
            ));

            links.Add(new Link(
                href: _linkGenerator.GetUriByAction(httpContext, "DeleteOrder", "Order", new { id = orderId }),
                rel: "delete",
                method: "DELETE"
            ));
        }

        return links;
    }
}
