using FluentValidation;
using MassTransitTest.Models;

namespace MassTransitTest.ModelsValidator;

public class SignReadyValidator : AbstractValidator<SignReady>
{
    public SignReadyValidator()
    {
        RuleFor(n => n.NationalId)
            .NotEmpty()
            .Must(NationalIdIsValid)
            .WithMessage("Validation Id must be valid");
    }

    private bool NationalIdIsValid(string NationalId)
    {
        if (NationalId.Length != 10)
            return false;
        else return true;
    }
}