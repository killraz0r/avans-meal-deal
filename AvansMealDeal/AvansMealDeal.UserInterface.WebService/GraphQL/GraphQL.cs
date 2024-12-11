using AvansMealDeal.Infrastructure.Application.SQLServer;

namespace AvansMealDeal.UserInterface.WebService.GraphQL
{
    public class GraphQL
    {
        // adds GraphQL endpoint to api
        public static void AddGraphQL(IServiceCollection services)
        {
            services.AddGraphQLServer()
                .AddQueryType<GraphQLQueryProvider>()
                .RegisterDbContextFactory<DbContextApplicationSqlServer>()
                .AddProjections()
                .AddFiltering()
                .AddSorting();
        }
    }
}
