using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    // US_01 is the website version of the user story, US_12 is the mobile version of the user story
    public class US_06__US_12
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;
        // list of meal packages inside the repository
        private readonly List<MealPackage> mealPackages = new List<MealPackage>
        {
            new MealPackage { Id = 1, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(2),
            Meals =
                [
                    new MealPackageItem { Meal = new Meal { Name = "Croissant", ContainsAlcohol = false } },
                    new MealPackageItem { Meal = new Meal { Name = "Stokbrood", ContainsAlcohol = false } },
                ]}
        };

        public US_06__US_12()
        {
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            mealPackageService = new MealPackageService(mealPackageRepositoryMock.Object, canteenRepositoryMock.Object);

            // setup repository mocks using logic from the actual repository 
            mealPackageRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>()))
               .ReturnsAsync((int id) => mealPackages
                   .SingleOrDefault(x => x.Id == id));
        }

        [Fact]
        public async Task ReadById_ValidId_ReturnsMealPackageWithMeals()
        {
            // arrange
            var id = 1;

            // act
            var mealPackage = await mealPackageService.GetById(id);

            // assert
            Assert.NotNull(mealPackage);
            Assert.Equal(2, mealPackage.Meals.Count);
            Assert.Contains(mealPackage.Meals.Select(x => x.Meal), y => y.Name == "Croissant");
            Assert.Contains(mealPackage.Meals.Select(x => x.Meal), y => y.Name == "Stokbrood");
        }

        [Fact]
        public async Task ReadById_InvalidId_ReturnsNull()
        {
            // arrange
            var id = 0;

            // act
            var mealPackage = await mealPackageService.GetById(id);

            // assert
            Assert.Null(mealPackage);
        }

        [Fact]
        public async Task ReadById_MultipleRecordsHaveSameId_ThrowsException()
        {
            // arrange
            mealPackages.Add(new MealPackage { Id = 1, CanteenId = 2, MealPackageType = MealPackageType.Drinks, Name = "Drank", Price = 2.00m, PickupDeadline = DateTimeOffset.Now.AddDays(1) });
            var id = 1;

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.GetById(id));

            // assert
            Assert.Equal("Sequence contains more than one matching element", exception.Message);
        }
    }
}
