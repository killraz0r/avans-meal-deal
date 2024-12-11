using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    // US_05 is the website version of the user story, US_11 is the mobile version of the user story
    public class US_05__US_11
    {
        private readonly IReservationService reservationService;
        private readonly Mock<IReservationRepository> reservationRepositoryMock;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;

        public US_05__US_11()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            reservationService = new ReservationService(reservationRepositoryMock.Object, mealPackageRepositoryMock.Object);
        }

        [Fact]
        public async Task HasStudentMadeReservationForGivenDate_NoReservations_ReturnsFalse()
        {
            // arrange
            var studentId = "1";
            var date = DateTimeOffset.Now.Date;
            reservationRepositoryMock.Setup(x => x.ReadReservationsForStudent(studentId)).ReturnsAsync([]);

            // act
            var result = await reservationService.HasStudentMadeReservationForGivenDate(studentId, DateOnly.FromDateTime(date));

            // assert
            Assert.False(result);
            reservationRepositoryMock.Verify(x => x.ReadReservationsForStudent(studentId), Times.Once);
        }

        [Fact]
        public async Task HasStudentMadeReservationForGivenDate_ReservationForStudentOnOtherDate_ReturnsFalse()
        {
            // arrange
            var studentId = "1";
            var date = DateTimeOffset.Now.Date;
            reservationRepositoryMock.Setup(x => x.ReadReservationsForStudent(studentId)).ReturnsAsync(
                [
                    new Reservation { StudentId = studentId, PlannedPickup = date.AddDays(1) }
                ]);

            // act
            var result = await reservationService.HasStudentMadeReservationForGivenDate(studentId, DateOnly.FromDateTime(date));

            // assert
            Assert.False(result);
            reservationRepositoryMock.Verify(x => x.ReadReservationsForStudent(studentId), Times.Once);
        }

        [Fact]
        public async Task HasStudentMadeReservationForGivenDate_ReservationOnDateForOtherStudent_ReturnsFalse()
        {
            // arrange
            var studentId = "1";
            var date = DateTimeOffset.Now.Date;
            reservationRepositoryMock.Setup(x => x.ReadReservationsForStudent(studentId)).ReturnsAsync(
                [
                    new Reservation { StudentId = "2", PlannedPickup = date }
                ]);

            // act
            var result = await reservationService.HasStudentMadeReservationForGivenDate(studentId, DateOnly.FromDateTime(date));

            // assert
            Assert.False(result);
            reservationRepositoryMock.Verify(x => x.ReadReservationsForStudent(studentId), Times.Once);
        }

        [Fact]
        public async Task HasStudentMadeReservationForGivenDate_ReservationForStudentOnGivenDate_ReturnsTrue()
        {
            // arrange
            var studentId = "1";
            var date = DateTimeOffset.Now.Date;
            reservationRepositoryMock.Setup(x => x.ReadReservationsForStudent(studentId)).ReturnsAsync(
                [
                    new Reservation { StudentId = "1", PlannedPickup = date }
                ]);

            // act
            var result = await reservationService.HasStudentMadeReservationForGivenDate(studentId, DateOnly.FromDateTime(date));

            // assert
            Assert.True(result);
            reservationRepositoryMock.Verify(x => x.ReadReservationsForStudent(studentId), Times.Once);
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
        public async Task Add_MealPackageNotFound_ThrowsException()
        {
            // arrange
            var mealPackageId = 101;
            var reservation = new Reservation
            {
                PlannedPickup = DateTimeOffset.Now,
                StudentId = "1",
                MealPackageId = mealPackageId
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageId)).ReturnsAsync((MealPackage?)null); // cast needed to return null

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await reservationService.Add(reservation));

            // assert
            reservationRepositoryMock.Verify(x => x.Create(It.IsAny<Reservation>()), Times.Never);
            Assert.Equal("Meal package 101 not found", exception.Message);
        }
    }
}
