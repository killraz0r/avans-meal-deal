using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class MealService : IMealService
    {
        private readonly IMealRepository mealRepository;

        public MealService(IMealRepository mealRepository)
        {
            this.mealRepository = mealRepository;
        }

        public async Task<ICollection<Meal>> GetAll()
        {
            return await mealRepository.ReadAll();
        }
    }
}
