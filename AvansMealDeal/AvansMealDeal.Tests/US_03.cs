using AvansMealDeal.Application.Services;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Domain.Services.Repositories;
using Moq;

namespace AvansMealDeal.Tests
{
    public class US_03
    {
        private readonly IMealPackageService mealPackageService;
        private readonly Mock<IMealPackageRepository> mealPackageRepositoryMock;

        public US_03()
        {
            var canteenRepositoryMock = new Mock<ICanteenRepository>();
            mealPackageRepositoryMock = new Mock<IMealPackageRepository>();
            mealPackageService = new MealPackageService(mealPackageRepositoryMock.Object, canteenRepositoryMock.Object);

            // mock canteen repository so that it always returns a valid canteen
            canteenRepositoryMock.Setup(x => x.ReadById(It.IsAny<int>())).ReturnsAsync((int canteenId) => new Canteen
            {
                Id = canteenId,
                City = City.Breda,
                Address = "Lovensdijkstraat 61",
                OffersHotMeals = true
            });
        }

        [Fact]
        public async Task Add_ValidData_AddsPackageWithMeals()
        {
            // arrange
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            var mealIds = new List<int> { 1, 2, 3 };

            // act
            await mealPackageService.Add(mealPackage, mealIds);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Create(mealPackage), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackage.Id, 1), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackage.Id, 2), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackage.Id, 3), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackage.Id, It.IsAny<int>()), Times.Exactly(3));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(49)]
        public async Task Add_InvalidPickupDeadline_ThrowsException(int hoursToAdd)
        {
            // arrange
            var mealPackage = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddHours(hoursToAdd),
                CanteenId = 1
            };
            var mealIds = new List<int> { 1, 2, 3 };

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Add(mealPackage, mealIds));

            // assert
            Assert.Equal("The pickup deadline is invalid", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Create(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Edit_ValidData_ChangesEditableFieldsAndChangesMealsOfPackage()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);
            var newMealPackageData = new MealPackage
            {
                Id = mealPackageInDatabase.Id,
                Name = "Bier met braadworst",
                Price = 7.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddDays(2),
                CanteenId = 2 // this cannot be changed
            };
            var mealIds = new List<int> { 1, 5 };

            // act
            await mealPackageService.Edit(newMealPackageData, mealIds);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Update(mealPackageInDatabase), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.ClearMealsFromPackage(mealPackageInDatabase), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackageInDatabase.Id, 1), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackageInDatabase.Id, 5), Times.Once);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(mealPackageInDatabase.Id, It.IsAny<int>()), Times.Exactly(2));
            Assert.Equal(newMealPackageData.Name, mealPackageInDatabase.Name);
            Assert.Equal(newMealPackageData.Price, mealPackageInDatabase.Price);
            Assert.Equal(newMealPackageData.MealPackageType, mealPackageInDatabase.MealPackageType);
            Assert.Equal(newMealPackageData.PickupDeadline, mealPackageInDatabase.PickupDeadline);
            Assert.Equal(mealPackageInDatabase.CanteenId, mealPackageInDatabase.CanteenId); // this has not changed
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(49)]
        public async Task Edit_InvalidPickupDeadline_ThrowsException(int hoursToAdd)
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);
            var newMealPackageData = new MealPackage
            {
                Id = mealPackageInDatabase.Id,
                Name = "Bier met braadworst",
                Price = 7.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddHours(hoursToAdd),
                CanteenId = 2
            };
            var mealIds = new List<int> { 1, 5 };

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Edit(newMealPackageData, mealIds));

            // assert
            Assert.Equal("The pickup deadline is invalid", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Update(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.ClearMealsFromPackage(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Edit_InvalidId_ThrowsException()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);
            var newMealPackageData = new MealPackage
            {
                Id = mealPackageInDatabase.Id + 1, // not the same id, does not exist in repository
                Name = "Bier met braadworst",
                Price = 7.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddDays(2),
                CanteenId = 2
            };
            var mealIds = new List<int> { 1, 5 };

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Edit(newMealPackageData, mealIds));

            // assert
            Assert.Equal("Meal package 101 not found", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Update(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.ClearMealsFromPackage(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Edit_HasReservation_ThrowsException()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1,
                ReservationId = 1 // meal package has reservation
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);
            var newMealPackageData = new MealPackage
            {
                Id = mealPackageInDatabase.Id,
                Name = "Bier met braadworst",
                Price = 7.00m,
                MealPackageType = MealPackageType.HotMeal,
                PickupDeadline = DateTimeOffset.Now.AddDays(2),
                CanteenId = 2
            };
            var mealIds = new List<int> { 1, 5 };

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Edit(newMealPackageData, mealIds));

            // assert
            Assert.Equal("Meal package 100 already has a reservation", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Update(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.ClearMealsFromPackage(It.IsAny<MealPackage>()), Times.Never);
            mealPackageRepositoryMock.Verify(x => x.AddMealToPackage(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Remove_ValidData_RemovesPackage()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);

            // act
            await mealPackageService.Remove(mealPackageInDatabase.Id);

            // assert
            mealPackageRepositoryMock.Verify(x => x.Delete(mealPackageInDatabase), Times.Once);
        }

        [Fact]
        public async Task Remove_InvalidId_ThrowsException()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Remove(mealPackageInDatabase.Id + 1));

            // assert
            Assert.Equal("Meal package 101 not found", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Delete(It.IsAny<MealPackage>()), Times.Never);
        }

        [Fact]
        public async Task Remove_HasReservation_ThrowsException()
        {
            // arrange
            var mealPackageInDatabase = new MealPackage
            {
                Id = 100,
                Name = "Bierpakket",
                Price = 6.00m,
                MealPackageType = MealPackageType.Drinks,
                PickupDeadline = DateTimeOffset.Now.AddDays(1),
                CanteenId = 1,
                ReservationId = 1 // meal package has reservation
            };
            mealPackageRepositoryMock.Setup(x => x.ReadById(mealPackageInDatabase.Id)).ReturnsAsync(mealPackageInDatabase);

            // act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () => await mealPackageService.Remove(mealPackageInDatabase.Id));

            // assert
            Assert.Equal("Meal package 100 already has a reservation", exception.Message);
            mealPackageRepositoryMock.Verify(x => x.Delete(It.IsAny<MealPackage>()), Times.Never);
        }
    }
}