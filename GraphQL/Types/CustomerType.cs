using TrendLine.DTOs;
using TrendLine.GraphQL.Resolvers;

namespace TrendLine.GraphQL.Types
{
    public class CustomerType : ObjectType<CustomerDTO>
    {
        protected override void Configure(IObjectTypeDescriptor<CustomerDTO> descriptor)
        {
            descriptor
                .Field(t => t.Orders)
                .ResolveWith<CustomerResolvers>(r => r.GetOrders(default!, default!));
        }
    }
}
