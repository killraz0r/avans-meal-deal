using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;
using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Tests
{
    public class US_09
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;
        private readonly Mock<ICanteenRepository> canteenRepositoryMock;

        public US_09()
        {
            canteenRepositoryMock = new Mock<ICanteenRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            mealPackageService = new MealPackageService(mealPackageRepositoryMock.Object, canteenRepositoryMock.Object);
        }

        [Fact]
        public async Task Add_CanteenAllowsHotMeals_AllowsAddingHotMealMealPackage()
        {
            // arrange
            var canteenId = 1000;
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Warme maaltijd",
                Price = 6.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = canteenId
            };
            var mealIds = new List<int> { 1, 2, 3 };
            canteenRepositoryMock.Setup(x => x.ReadById(canteenId)).ReturnsAsync(new Canteen
            {
                Id = canteenId,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = true
            });

            // act
            await mealPackageService.Add(mealPackage, mealIds);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Create(mealPackage), Times.Once);
            canteenRepositoryMock.Verify(x => x.ReadById(canteenId), Times.Once); // canteen check only done for HotMeals
        }

        [Fact]
        public async Task Add_CanteenDoesNotAllowHotMeals_ThrowsExceptionWhenAddingHotMealMealPackage()
        {
            // arrange
            var canteenId = 1000;
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Warme maaltijd",
                Price = 6.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = canteenId
            };
            var mealIds = new List<int> { 1, 2, 3 };
            canteenRepositoryMock.Setup(x => x.ReadById(canteenId)).ReturnsAsync(new Canteen
            {
                Id = canteenId,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = false
            });

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Add(mealPackage, mealIds));

            // assert
            Assert.Equal("Canteen 1000 does not offer hot meals", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Create(mealPackage), Times.Never);
            canteenRepositoryMock.Verify(x => x.ReadById(canteenId), Times.Once); // canteen check only done for HotMeals
        }

        [Fact]
        public async Task Add_CanteenAllowsHotMeals_AllowsAddingNonHotMealMealPackage()
        {
            // arrange
            var canteenId = 1000;
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Brood",
                Price = 6.00m,
                MealPackageType = MealPackageType.Bread,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = canteenId
            };
            var mealIds = new List<int> { 1, 2, 3 };
            canteenRepositoryMock.Setup(x => x.ReadById(canteenId)).ReturnsAsync(new Canteen
            {
                Id = canteenId,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = true
            });

            // act
            await mealPackageService.Add(mealPackage, mealIds);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Create(mealPackage), Times.Once);
            canteenRepositoryMock.Verify(x => x.ReadById(canteenId), Times.Never); // canteen check only done for HotMeals
        }

        [Fact]
        public async Task Add_CanteenDoesNotAllowHotMeals_AllowsAddingNonHotMealMealPackage()
        {
            // arrange
            var canteenId = 1000;
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Brood",
                Price = 6.00m,
                MealPackageType = MealPackageType.Bread,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = canteenId
            };
            var mealIds = new List<int> { 1, 2, 3 };
            canteenRepositoryMock.Setup(x => x.ReadById(canteenId)).ReturnsAsync(new Canteen
            {
                Id = canteenId,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = false
            });

            // act
            await mealPackageService.Add(mealPackage, mealIds);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Create(mealPackage), Times.Once);
            canteenRepositoryMock.Verify(x => x.ReadById(canteenId), Times.Never); // canteen check only done for HotMeals
        }
    }
}
