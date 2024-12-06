using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_01
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;
        // list of meal packages inside the repository
        private readonly List<MealPackage> mealPackages = new List<MealPackage>
        {
            new MealPackage { Id = 1, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Beschikbaar brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(1) },
            new MealPackage { Id = 2, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Bedorven brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(-1) },
            new MealPackage { Id = 3, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Gereserveerd brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(-1),
                                Reservation = new Reservation { StudentId = "1", PlannedPickup = DateTimeOffset.Now.AddDays(-1) } },
            new MealPackage { Id = 4, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Gereserveerd brood voor iemand anders", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(-1),
                                Reservation = new Reservation { StudentId = "2", PlannedPickup = DateTimeOffset.Now.AddDays(-1) } },
        };

        public US_01()
        {
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            mealPackageService = new MealPackageService(mealPackageRepositoryMock.Object);

            // setup repository mocks using logic from the actual repository 
            mealPackageRepositoryMock.Setup(x => x.ReadWithoutReservation())
                .ReturnsAsync(() => mealPackages
                    .Where(x => x.Reservation == null && x.PickupDeadline >= DateTimeOffset.Now)
                    .OrderBy(x => x.PickupDeadline).ToList());
            mealPackageRepositoryMock.Setup(x => x.ReadWithReservationForStudent(It.IsAny<string>()))
               .ReturnsAsync((string studentId) => mealPackages
                   .Where(x => x.Reservation != null && x.Reservation.StudentId == studentId)
                   .OrderBy(x => x.PickupDeadline).ToList());
        }

        [Fact]
        public async Task ReadWithoutReservation_MealPackagesAvailable_ShowsAvailableMealPackages()
        {
            // act
            var mealPackages = await mealPackageService.GetWithoutReservation();

            // assert
            Assert.True(mealPackages.Count == 1);
            Assert.True(mealPackages.All(x => x.Reservation == null)); // only no reservations
            Assert.True(mealPackages.All(x => x.PickupDeadline > DateTimeOffset.Now)); // no expired pickup deadlines
        }

        [Fact]
        public async Task ReadWithReservationForStudent_ValidId_ShowsReservedMealPackagesForStudent()
        {
            // arrange
            var studentId = "1";

            // act
            var mealPackages = await mealPackageService.GetWithReservationForStudent(studentId);

            // assert
            Assert.True(mealPackages.Count == 1);
            Assert.True(mealPackages.All(x => x.Reservation != null && x.Reservation.StudentId == studentId)); // only reservations for the student
        }

        [Fact]
        public async Task ReadWithReservationForStudent_InvalidId_ShowsNoMealPackages()
        {
            // arrange
            var studentId = "3";

            // act
            var mealPackages = await mealPackageService.GetWithReservationForStudent(studentId);

            // assert
            Assert.False(mealPackages.Any()); // there are no reservations for this student
        }
    }
}
