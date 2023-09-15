﻿using FluentValidation;

namespace ApiTemplate.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.UserID)
            .NotEmpty();

        RuleFor(x => x.TokenToRefresh)
            .NotEmpty();
    }
}