using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.UserInterface.WebService.GraphQL
{
    // allows GraphQL to make queries to the database
    public class GraphQLQueryProvider
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<MealPackage> GetMealPackages([Service] IMealPackageRepository mealPackageRepository)
        {
            return mealPackageRepository.Read();
        }
    }
}
