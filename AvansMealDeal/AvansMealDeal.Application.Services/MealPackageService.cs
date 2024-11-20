using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class MealPackageService : IMealPackageService
    {
        private readonly IMealPackageRepository mealPackageRepository;

        public MealPackageService(IMealPackageRepository mealPackageRepository)
        {
            this.mealPackageRepository = mealPackageRepository;
        }

		public async Task Add(MealPackage mealPackage, ICollection<int> mealIds)
		{
			await mealPackageRepository.Create(mealPackage);
            foreach (var mealId in mealIds) 
            {
                await mealPackageRepository.AddMealToPackage(mealPackage.Id, mealId);
            }
		}

        public async Task<MealPackage?> GetById(int id)
        {
            return await mealPackageRepository.ReadById(id);
        }

        public async Task<ICollection<MealPackage>> GetForCanteen(int canteenId)
        {
            return await mealPackageRepository.ReadForCanteen(canteenId);
        }
    }
}
