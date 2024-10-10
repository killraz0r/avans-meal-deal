using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for canteens
    public interface ICanteenRepository
    {
        Task<ICollection<Canteen>> RealAdd();
    }
}
