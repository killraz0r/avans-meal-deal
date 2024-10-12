using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class CanteenService : ICanteenService
    {
        private readonly ICanteenRepository canteenRepository;

        public CanteenService(ICanteenRepository canteenRepository)
        {
            this.canteenRepository = canteenRepository;
        }

        public async Task<ICollection<Canteen>> GetAll()
        {
            return await canteenRepository.ReadAll();
        }
    }
}
