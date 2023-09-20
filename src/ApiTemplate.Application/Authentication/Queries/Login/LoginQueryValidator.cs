using FluentValidation;

namespace ApiTemplate.Application.Authentication.Queries.Login;

internal class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    internal LoginQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}