using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for meal packages
    public interface IMealPackageRepository
    {
        Task Create(MealPackage mealPackage);
        Task Update(MealPackage mealPackage);
        Task ClearMealsFromPackage(MealPackage mealPackage);
		Task AddMealToPackage(int mealPackageId, int mealId);
        Task<MealPackage?> ReadById(int id);
        Task<ICollection<MealPackage>> ReadForCity(City city);
        Task<ICollection<MealPackage>> ReadForCanteen(int canteenId);
    }
}
