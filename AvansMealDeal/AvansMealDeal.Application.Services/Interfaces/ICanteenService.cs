using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Application.Services.Interfaces
{
    public interface ICanteenService
    {
        Task<Canteen> GetById(int id);
        Task<ICollection<Canteen>> GetAll();
    }
}
