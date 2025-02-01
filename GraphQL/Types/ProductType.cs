using TrendLine.DTOs;
using TrendLine.GraphQL.Resolvers;

namespace TrendLine.GraphQL.Types
{
    public class ProductType : ObjectType<ProductDTO>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductDTO> descriptor)
        {
            descriptor
                .Field(t => t.Brand)
                .ResolveWith<ProductResolvers>(r => r.GetBrand(default!, default!));

            descriptor
                .Field(t => t.Category)
                .ResolveWith<ProductResolvers>(r => r.GetCategory(default!, default!));

            descriptor
                .Field(t => t.Color)
                .ResolveWith<ProductResolvers>(r => r.GetColor(default!, default!));

            descriptor
                .Field(t => t.Size)
                .ResolveWith<ProductResolvers>(r => r.GetSize(default!, default!));
        }
    }
}
