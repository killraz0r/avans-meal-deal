using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Application.Services.Interfaces
{
    public interface IMealPackageService
    {
        Task Add(MealPackage mealPackage);
        Task<ICollection<MealPackage>> GetForCanteen(int canteenId);
    }
}
