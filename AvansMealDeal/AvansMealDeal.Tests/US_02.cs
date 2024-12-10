using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_02
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;
        // list of meal packages inside the repository
        private readonly List<MealPackage> mealPackages = new List<MealPackage> 
        {
            new MealPackage { Id = 1, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(1) },
            new MealPackage { Id = 2, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Broodplank", Price = 2.00m, PickupDeadline = DateTimeOffset.Now.AddDays(2) },
            new MealPackage { Id = 3, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Stokbrood", Price = 3.00m, PickupDeadline = DateTimeOffset.Now.AddDays(-1) },
            new MealPackage { Id = 4, CanteenId = 2, MealPackageType = MealPackageType.HotMeal, Name = "Warme avondmaaltijd", Price = 4.00m, PickupDeadline = DateTimeOffset.Now.AddDays(2) },
            new MealPackage { Id = 5, CanteenId = 3, MealPackageType = MealPackageType.Drinks, Name = "Drank", Price = 5.00m, PickupDeadline = DateTimeOffset.Now.AddDays(1) },
            new MealPackage { Id = 6, CanteenId = 4, MealPackageType = MealPackageType.Bread, Name = "Brood", Price = 6.00m, PickupDeadline = DateTimeOffset.Now.AddDays(-1) },
        };

        public US_02()
        {
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            mealPackageService = new MealPackageService(mealPackageRepositoryMock.Object, canteenRepositoryMock.Object);

            // setup repository mocks using logic from the actual repository 
            mealPackageRepositoryMock.Setup(x => x.ReadForCanteen(It.IsAny<int>()))
                .ReturnsAsync((int canteenId) => mealPackages
                    .Where(x => x.CanteenId == canteenId && x.PickupDeadline >= DateTimeOffset.Now)
                    .OrderBy(x => x.PickupDeadline).ToList());
            mealPackageRepositoryMock.Setup(x => x.ReadForOtherCanteens(It.IsAny<int>()))
               .ReturnsAsync((int canteenId) => mealPackages
                   .Where(x => x.CanteenId != canteenId && x.PickupDeadline >= DateTimeOffset.Now)
                   .OrderBy(x => x.PickupDeadline).ToList());
        }

        [Fact]
        public async Task ReadForCanteen_ValidId_ShowsMealPackagesForCanteenOrderedByPickupDeadlineAndExcludesExpiredPickupDeadlines()
        {
            // arrange
            var canteenId = 1;

            // act
            var mealPackages = (await mealPackageService.GetForCanteen(canteenId)).ToList();

            // assert
            Assert.Equal(2, mealPackages.Count);
            Assert.True(mealPackages.All(x => x.CanteenId == canteenId)); // only same canteen
            Assert.True(mealPackages.All(x => x.PickupDeadline > DateTimeOffset.Now)); // no expired pickup deadlines
            Assert.True(mealPackages[0].PickupDeadline < mealPackages[1].PickupDeadline); // ordered by pickup deadline ASC
        }

        [Fact]
        public async Task ReadForOtherCanteens_ValidId_ShowsMealPackagesForOtherCanteensOrderedByPickupDeadlineAndExcludesExpiredPickupDeadlines()
        {
            // arrange
            var canteenId = 1;

            // act
            var mealPackages = (await mealPackageService.GetForOtherCanteens(canteenId)).ToList();

            // assert
            Assert.Equal(2, mealPackages.Count);
            Assert.True(mealPackages.All(x => x.CanteenId != canteenId)); // only other canteens
            Assert.True(mealPackages.All(x => x.PickupDeadline > DateTimeOffset.Now)); // no expired pickup deadlines
            Assert.True(mealPackages[0].PickupDeadline < mealPackages[1].PickupDeadline); // ordered by pickup deadline ASC
        }
    }
}
