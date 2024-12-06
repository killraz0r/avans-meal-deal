// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using AvansMealDeal.Application.Services.Interfaces;
using AvansMealDeal.Domain.Models;
using AvansMealDeal.Infrastructure.Application.SQLServer.Migrations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AvansMealDeal.UserInterface.WebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ICanteenService canteenService;
        private readonly SignInManager<MealDealUser> _signInManager;
        private readonly UserManager<MealDealUser> _userManager;
        private readonly IUserStore<MealDealUser> _userStore;
        private readonly IUserEmailStore<MealDealUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            ICanteenService canteenService,
            UserManager<MealDealUser> userManager,
            IUserStore<MealDealUser> userStore,
            SignInManager<MealDealUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.canteenService = canteenService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // list of canteens an employee can register for
        public ICollection<SelectListItem> Canteens { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel : IValidatableObject
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "E-mailadres ontbreekt")]
            [EmailAddress]
            [Display(Name = "E-mailadres")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Wachtwoord ontbreekt")]
            [StringLength(100, ErrorMessage = "Het wachtwoord moet tussen {2} en {1} tekens lang zijn", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Wachtwoord")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Herhaal wachtwoord")]
            [Compare("Password", ErrorMessage = "Het wachtwoord moet overeenkomen")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Naam ontbreekt")]
            [DataType(DataType.Text)]
            [Display(Name = "Naam")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Rol ontbreekt")]
            [Display(Name = "Rol")]
            public string Role { get; set; }

            // employee fields
            [DataType(DataType.Text)]
            [Display(Name = "Personeelsnummer")]
            public string EmployeeNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Kantine")]
            public int? EmployeeCanteenId { get; set; }

            // student fields
            [DataType(DataType.Text)]
            [Display(Name = "Studentnummer")]
            public string StudentNumber { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Geboortedatum")]
            public DateOnly? StudentBirthDate { get; set; }

            [Display(Name = "Studiestad")]
            public City? StudentCity { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Telefoonnummer")]
            public string PhoneNumber { get; set; }

            // validation for specific roles
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Role == Domain.Models.Role.Employee)
                {
                    if (string.IsNullOrWhiteSpace(EmployeeNumber))
                    {
                        yield return new ValidationResult("Personeelnummer ontbreekt", [nameof(EmployeeNumber)]);
                    }
                    if (!EmployeeCanteenId.HasValue)
                    {
                        yield return new ValidationResult("Kantine ontbreekt", [nameof(EmployeeCanteenId)]);
                    }
                }

                if (Role == Domain.Models.Role.Student)
                {
                    if (string.IsNullOrWhiteSpace(StudentNumber)) 
                    {
                        yield return new ValidationResult("Studentnummer ontbreekt", [nameof(StudentNumber)]);
                    }

                    if (!StudentBirthDate.HasValue)
                    {
                        yield return new ValidationResult("Geboortedatum ontbreekt", [nameof(StudentBirthDate)]);
                    }
                    else
                    {
                        // prevent people under 16 from registration
                        var today = DateOnly.FromDateTime(DateTimeOffset.Now.Date);
                        var sixteenYearsAgo = today.AddYears(-16);

                        if (StudentBirthDate > sixteenYearsAgo)
                        {
                            yield return new ValidationResult("Je moet minimaal 16 jaar zijn om te registreren", [nameof(StudentBirthDate)]);
                        }
                    }

                    if (!StudentCity.HasValue)
                    {
                        yield return new ValidationResult("Studiestad ontbreekt", [nameof(StudentCity)]);
                    }   

                    if (string.IsNullOrWhiteSpace(PhoneNumber))
                    {
                        yield return new ValidationResult("Telefoonnummer ontbreekt", [nameof(PhoneNumber)]);
                    }
                }
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            await LoadCanteens();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.Name = Input.Name;
                if (Input.Role == Role.Employee) 
                {
                    user.EmployeeNumber = Input.EmployeeNumber;
                    user.EmployeeCanteenId = Input.EmployeeCanteenId;
                }
                if (Input.Role == Role.Student) 
                {
                    user.StudentNumber = Input.StudentNumber;
                    user.StudentBirthDate = Input.StudentBirthDate;
                    user.StudentCity = Input.StudentCity;
                    await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                }
                await _userManager.AddToRoleAsync(user, Input.Role);
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // canteens need to be reloaded if the form needs to be resubmitted
            await LoadCanteens();
            return Page();
        }

        private async Task LoadCanteens()
        {
            var canteens = await canteenService.GetAll();
            Canteens = canteens.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.ToString()
            }).ToList();
        }

        private MealDealUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<MealDealUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(MealDealUser)}'. " +
                    $"Ensure that '{nameof(MealDealUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<MealDealUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<MealDealUser>)_userStore;
        }
    }
}
