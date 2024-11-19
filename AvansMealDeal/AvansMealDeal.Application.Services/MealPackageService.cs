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

		public async Task Add(MealPackage mealPackage)
		{
            await mealPackageRepository.Create(mealPackage);
		}

		public async Task<ICollection<MealPackage>> GetForCanteen(int canteenId)
        {
            return await mealPackageRepository.ReadForCanteen(canteenId);
        }
    }
}
