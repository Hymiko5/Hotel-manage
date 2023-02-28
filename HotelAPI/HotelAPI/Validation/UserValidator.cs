using FluentValidation;
using HotelAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HotelAPI.Validation
{
    public class UserValidator: AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name).NotNull().NotEmpty().Length(1, 250);
            RuleFor(user => user.Phone).NotNull().NotEmpty().Matches(new Regex(@"^\d{10,11}$")).WithMessage("Invalid phone number");
            RuleFor(user => user.IdentificationCard).NotEmpty().WithMessage("Please add the IdentificationCard");
            RuleFor(user => user.Gmail).NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("A valid email is required");
        }
    }
}
