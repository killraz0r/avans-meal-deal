using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    // repository for canteens
    public interface ICanteenRepository
    {
        Task<Canteen> ReadById(int id);
        Task<ICollection<Canteen>> ReadAll();
    }
}
