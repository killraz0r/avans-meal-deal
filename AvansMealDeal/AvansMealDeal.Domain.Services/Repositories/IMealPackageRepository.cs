﻿using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for meal packages
    public interface IMealPackageRepository
    {
        Task Create(MealPackage mealPackage);
        Task Update(MealPackage mealPackage);
        Task ClearMealsFromPackage(MealPackage mealPackage);
		Task AddMealToPackage(int mealPackageId, int mealId);
		Task Delete(MealPackage mealPackage);
		Task<MealPackage?> ReadById(int id);
        Task<ICollection<MealPackage>> ReadForCanteen(int canteenId);
        Task<ICollection<MealPackage>> ReadForOtherCanteens(int canteenId);
        Task<ICollection<MealPackage>> ReadWithoutReservation();
        Task<ICollection<MealPackage>> ReadWithReservationForStudent(string studentId);
        IQueryable<MealPackage> Read(); // allows reading the repository with custom rules
    }
}
