using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IMealPackageRepository mealPackageRepository;

        public ReservationService(IReservationRepository reservationRepository, IMealPackageRepository mealPackageRepository) 
        {
            this.reservationRepository = reservationRepository;
            this.mealPackageRepository = mealPackageRepository;
        }

        public async Task Add(Reservation reservation)
        {
            var mealPackage = await mealPackageRepository.ReadById(reservation.MealPackageId);
            if (mealPackage == null)
            {
                throw new InvalidOperationException($"Meal package {reservation.MealPackageId} not found");
            }
            if (mealPackage.ReservationId != null || mealPackage.Reservation != null)
            {
                throw new InvalidOperationException($"Meal package {reservation.MealPackageId} already has a reservation");
            }
            await reservationRepository.Create(reservation);
        }

        public async Task<bool> HasStudentMadeReservationForGivenDate(string studentId, DateOnly date)
        {
            var reservations = await reservationRepository.ReadReservationsForStudent(studentId);
            return reservations.Any(x => x.StudentId == studentId && DateOnly.FromDateTime(x.PlannedPickup.Date) == date);
        }
    }
}
