using System.ComponentModel.DataAnnotations;

namespace AvansMealDeal.Domain.Models.Attributes
{
	public class PickupDeadlineValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is DateTimeOffset pickupDeadline)
			{
				var now = DateTimeOffset.Now;
				var deadline = now.AddHours(48); // two days ahead maximum

				if (pickupDeadline < now)
				{
					return new ValidationResult("Het ophaaltijdstip moet in de toekomst zijn");
				}

				if (pickupDeadline >= now && pickupDeadline <= deadline)
				{
					return ValidationResult.Success;
				}
			}

			return new ValidationResult("Het ophaaltijdstip mag maximaal 48 uur in de toekomst zijn");
		}
	}
}
