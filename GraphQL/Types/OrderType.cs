using TrendLine.DTOs;
using TrendLine.GraphQL.Resolvers;

namespace TrendLine.GraphQL.Types
{
    public class OrderType : ObjectType<OrderDTO>
    {
        protected override void Configure(IObjectTypeDescriptor<OrderDTO> descriptor)
        {
            descriptor
                .Field(t => t.Customer)
                .ResolveWith<OrderResolvers>(r => r.GetCustomer(default!, default!));
        }
    }
}
