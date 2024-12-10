using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AvansMealDeal.Infrastructure.Application.SQLServer.Repositories
{
    public class SqlServerReservationRepository : IReservationRepository
    {
        private readonly DbContextApplicationSqlServer dbContext;

        public SqlServerReservationRepository(DbContextApplicationSqlServer dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(Reservation reservation)
        {
            dbContext.Reservations.Add(reservation);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Reservation>> ReadReservationsForStudent(string studentId)
        {
            return await dbContext.Reservations
                .Where(x => x.StudentId == studentId)
                .ToListAsync();
        }
    }
}
