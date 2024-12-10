using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Application.Services;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_06
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;
        // list of meal packages inside the repository
        private readonly List<MealPackage> mealPackages = new List<MealPackage>
        {
            new MealPackage { Id = 1, CanteenId = 1, MealPackageType = MealPackageType.Bread, Name = "Brood", Price = 1.00m, PickupDeadline = DateTimeOffset.Now.AddDays(2) }
        };

        public US_06()
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
        public async Task ReadById_ValidId_ReturnsMealPackage()
        {
            // arrange
            var id = 1;

            // act
            var mealPackage = await mealPackageService.GetById(id);

            // assert
            Assert.NotNull(mealPackage);
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
