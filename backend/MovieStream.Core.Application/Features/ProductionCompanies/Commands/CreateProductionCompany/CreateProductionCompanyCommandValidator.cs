using FluentValidation;

namespace MovieStream.Core.Application.Features.ProductionCompanies.Commands.CreateProductionCompany
{
    public class CreateProductionCompanyCommandValidator : AbstractValidator<CreateProductionCompanyCommand>
    {
        public CreateProductionCompanyCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(100).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
