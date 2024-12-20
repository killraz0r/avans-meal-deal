﻿using AvansMealDeal.Domain.Models;

namespace AvansMealDeal.UserInterface.WebApp.Helpers
{
	public class HtmlHelpers
	{
		public static string DisplayMealPackageType(MealPackageType mealPackageType)
		{
			switch (mealPackageType)
			{
				case MealPackageType.Bread: return "Brood";
				case MealPackageType.HotMeal: return "Warme avondmaaltijd";
				case MealPackageType.Drinks: return "Drank";
				default: return "?";
			}
		}

        public static string DisplayCity(City city)
        {
            switch (city)
            {
                case City.Breda: return "Breda";
                case City.Tilburg: return "Tilburg";
                case City.DenBosch: return "Den Bosch";
                default: return "?";
            }
        }

        public static string DisplayDutchBoolean(bool boolean)
		{
			if (boolean)
			{
				return "Ja";
			}
			return "Nee";
		}
	}
}
