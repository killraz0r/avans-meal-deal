using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for meals
    public interface IMealRepository
    {      
        Task Create(Meal meal);
        Task<ICollection<Meal>> ReadAll();
    }
}
