using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Application.Services.Interfaces
{
    public interface ICanteenService
    {
        Task<ICollection<Canteen>> GetAll();
    }
}
