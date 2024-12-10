using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Domain.Services.Repositories
{
    public interface IReservationRepository
    {
        Task Create(Reservation reservation);
        Task<ICollection<Reservation>> ReadReservationsForStudent(string studentId);
    }
}
