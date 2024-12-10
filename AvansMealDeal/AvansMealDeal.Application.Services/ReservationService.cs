using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;

namespace AvansMealDeal.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;

        public ReservationService(IReservationRepository reservationRepository) 
        {
            this.reservationRepository = reservationRepository;
        }

        public async Task Add(Reservation reservation)
        {
            await reservationRepository.Create(reservation);
        }

        public async Task<bool> HasStudentMadeReservationForGivenDate(string studentId, DateOnly date)
        {
            var reservations = await reservationRepository.ReadReservationsForStudent(studentId);
            return reservations.Any(x => x.StudentId == studentId && DateOnly.FromDateTime(x.PlannedPickup.Date) == date);
        }
    }
}
