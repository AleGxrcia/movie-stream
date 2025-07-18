using FluentValidation;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.UpdateProductionCompany
{
    public class UpdateProductionCompanyCommandValidator : AbstractValidator<UpdateProductionCompanyCommand>
    {
        public UpdateProductionCompanyCommandValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
