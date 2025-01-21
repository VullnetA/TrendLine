using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.Logging;

namespace TrendLine.Services.Utilities
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        private readonly ILogger<GraphQLErrorFilter> _logger;

        public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger)
        {
            _logger = logger;
        }

        public IError OnError(IError error)
        {
            // Log the error (optional)
            _logger.LogError(error.Exception, "GraphQL Error: {Message}", error.Message);

            // Construct a new error with additional extensions
            return ErrorBuilder.New()
                .SetMessage("An error occurred while processing your request.")
                .SetCode("GRAPHQL_ERROR")
                .SetExtension("details", error.Exception?.Message ?? "No exception details available")
                .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                .Build();
        }
    }
}
