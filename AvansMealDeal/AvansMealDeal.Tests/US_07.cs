using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_07
    {
        private readonly IReservationService reservationService;
        private readonly Mock<IReservationRepository> reservationRepositoryMock;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;

        public US_07()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            reservationService = new ReservationService(reservationRepositoryMock.Object, mealPackageRepositoryMock.Object);
        }

        [Fact]
        public async Task Add_ValidData_AddsReservation()
        {
            // arrange
            var mealPackageId = 1;
            var reservation = new Reservation
            {
                PlannedPickup = DateTimeOffset.Now,
                StudentId = "1",
                MealPackageId = mealPackageId
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageId)).ReturnsAsync(new MealPackage
            {
                Id = mealPackageId,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1)
            });

            // act
            await reservationService.Add(reservation);

            // assert
            reservationRepositoryMock.Verify(x => x.Create(reservation), Times.Once);
        }

        [Fact]
        public async Task Add_MealPackageHasReservation_ThrowsException()
        {
            // arrange
            var mealPackageId = 101;
            var reservation = new Reservation
            {
                PlannedPickup = DateTimeOffset.Now,
                StudentId = "1",
                MealPackageId = mealPackageId
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageId)).ReturnsAsync(new MealPackage
            {
                Id = mealPackageId,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                ReservationId = 2
            });

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await reservationService.Add(reservation));

            // assert
            reservationRepositoryMock.Verify(x => x.Create(It.IsAny<Reservation>()), Times.Never);
            Assert.Equal("Meal package 101 already has a reservation", exception.Message);
        }
    }
}
