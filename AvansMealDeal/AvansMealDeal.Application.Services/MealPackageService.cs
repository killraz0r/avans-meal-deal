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
            // prevent invalid deadline (in the past and more than 48 hours in the future)
            if (DateTimeOffset.Now > mealPackage.PickupDeadline || mealPackage.PickupDeadline > DateTimeOffset.Now.AddHours(48))
            {
                throw new InvalidOperationException("The pickup deadline is invalid");
            }

            await mealPackageRepository.Create(mealPackage);
            foreach (var mealId in mealIds) 
            {
                await mealPackageRepository.AddMealToPackage(mealPackage.Id, mealId);
            }
		}

		public async Task Edit(MealPackage mealPackage, ICollection<int> mealIds)
		{
            // prevent invalid deadline (in the past and more than 48 hours in the future)
            if (DateTimeOffset.Now > mealPackage.PickupDeadline || mealPackage.PickupDeadline > DateTimeOffset.Now.AddHours(48))
            {
                throw new InvalidOperationException("The pickup deadline is invalid");
            }

            var mealPackageInDatabase = await GetById(mealPackage.Id);
            if (mealPackageInDatabase == null) 
            {
                throw new InvalidOperationException($"Meal package {mealPackage.Id} not found");
            }
            if (mealPackageInDatabase.ReservationId != null || mealPackageInDatabase.Reservation != null)
            {
                throw new InvalidOperationException($"Meal package {mealPackage.Id} already has a reservation");
            }

            // only edit data that is allowed to be edited
            mealPackageInDatabase.Name = mealPackage.Name;
            mealPackageInDatabase.Price = mealPackage.Price;
            mealPackageInDatabase.MealPackageType = mealPackage.MealPackageType;
            mealPackageInDatabase.PickupDeadline = mealPackage.PickupDeadline;

			await mealPackageRepository.Update(mealPackageInDatabase);
            // replace the old list of meals with the new list of meals
			await mealPackageRepository.ClearMealsFromPackage(mealPackageInDatabase);
			foreach (var mealId in mealIds)
			{
				await mealPackageRepository.AddMealToPackage(mealPackage.Id, mealId);
			}
		}

        public async Task Remove(int id)
        {
			var mealPackage = await GetById(id);
            if (mealPackage == null)
            {
                throw new InvalidOperationException($"Meal package {id} not found");
            }
            if (mealPackage.ReservationId != null || mealPackage.Reservation != null)
            {
                throw new InvalidOperationException($"Meal package {mealPackage.Id} already has a reservation");
            }
            await mealPackageRepository.Delete(mealPackage!);
		}

		public async Task<MealPackage?> GetById(int id)
        {
            return await mealPackageRepository.ReadById(id);
        }

        public async Task<ICollection<MealPackage>> GetForCanteen(int canteenId)
        {
            return await mealPackageRepository.ReadForCanteen(canteenId);
        }

        public async Task<ICollection<MealPackage>> GetForOtherCanteens(int canteenId)
        {
            return await mealPackageRepository.ReadForOtherCanteens(canteenId);
        }

        public async Task<ICollection<MealPackage>> GetWithoutReservation()
        {
            return await mealPackageRepository.ReadWithoutReservation();
        }

        public async Task<ICollection<MealPackage>> GetWithReservationForStudent(string studentId)
        {
            return await mealPackageRepository.ReadWithReservationForStudent(studentId);
        }
    }
}
