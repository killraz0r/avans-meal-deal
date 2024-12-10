using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task Add(Reservation reservation);
        Task<bool> HasStudentMadeReservationForGivenDate(string studentId, DateOnly date);
    }
}
