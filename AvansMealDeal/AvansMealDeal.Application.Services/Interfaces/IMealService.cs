using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Application.Services.Interfaces
{
    // services for meals
    public interface IMealService
    {
        Task Add(Meal meal);
        Task<ICollection<Meal>> GetAll();
    }
}
