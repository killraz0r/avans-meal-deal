using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for meal packages
    public interface IMealPackageRepository
    {
        Task Create(MealPackage mealPackage);
        Task AddMealToPackage(int mealPackageId, int mealId);
        Task<ICollection<MealPackage>> ReadForCity(City city);
        Task<ICollection<MealPackage>> ReadForCanteen(int canteenId);
    }
}
