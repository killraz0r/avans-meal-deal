using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_05
    {
        private readonly IReservationService reservationService;
        private readonly Mock<IReservationRepository> reservationRepositoryMock;

        public US_05()
        {
            reservationRepositoryMock = new Mock<IReservationRepository>();
            reservationService = new ReservationService(reservationRepositoryMock.Object);
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
            var reservation = new Reservation
            {
                PlannedPickup = DateTimeOffset.Now,
                StudentId = "1",
                MealPackageId = 1
            };

            // act
            await reservationService.Add(reservation);

            // assert
            reservationRepositoryMock.Verify(x => x.Create(reservation), Times.Once);
        }
    }
}
