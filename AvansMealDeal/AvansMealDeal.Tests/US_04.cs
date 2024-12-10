using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.Tests
{
    public class US_04
    {
        public void AdultsOnly_MealPackageWithItemThatContainAlcohol_ReturnsTrue()
        {
            // arrange
            var mealPackage = new MealPackage
            {
                Name = "Jupilerpakket",
                MealPackageType = MealPackageType.Drinks,
                Price = 3.00m,
                PickupDeadline = DateTimeOffset.Now,
                Meals = 
                [
                    new MealPackageItem { Meal = new Meal { Name = "Jupiler", ContainsAlcohol = true } },
                    new MealPackageItem { Meal = new Meal { Name = "Jupiler 0.0", ContainsAlcohol = false } },
                ]
            };

            // act
            var result = mealPackage.AdultsOnly();

            // assert
            Assert.True(result);
        }

        public void AdultsOnly_MealPackageWithoutItemThatContainAlcohol_ReturnsFalse()
        {
            // arrange
            var mealPackage = new MealPackage
            {
                Name = "Frisdrank",
                MealPackageType = MealPackageType.Drinks,
                Price = 3.00m,
                PickupDeadline = DateTimeOffset.Now,
                Meals =
                [
                    new MealPackageItem { Meal = new Meal { Name = "Fanta", ContainsAlcohol = false } },
                    new MealPackageItem { Meal = new Meal { Name = "Sprite", ContainsAlcohol = false } },
                ]
            };

            // act
            var result = mealPackage.AdultsOnly();

            // assert
            Assert.False(result);
        }

        public void AdultsOnly_MealPackageWithoutItems_ReturnsFalse()
        {
            // arrange
            var mealPackage = new MealPackage
            {
                Name = "Frisdrank",
                MealPackageType = MealPackageType.Drinks,
                Price = 3.00m,
                PickupDeadline = DateTimeOffset.Now,
                Meals = []
            };

            // act
            var result = mealPackage.AdultsOnly();

            // assert
            Assert.False(result);
        }
    }
}
