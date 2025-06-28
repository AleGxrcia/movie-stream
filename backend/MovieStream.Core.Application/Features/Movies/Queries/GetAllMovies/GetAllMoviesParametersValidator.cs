using FluentValidation;
using MovieStream.Core.Application.Enums;

namespace MovieStream.Core.Application.Features.Movies.Queries.GetAllMovies
{
    public class GetAllMoviesParametersValidator : AbstractValidator<GetAllMoviesParameters>
    {
        public GetAllMoviesParametersValidator()
        {
            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn)
                .When(x => !string.IsNullOrEmpty(x.SortColumn))
                .WithMessage($"SortColumn must be one of the following: {string.Join(", ", Enum.GetNames(typeof(SortByOptions)))}");

            RuleFor(x => x.SortOrder)
                .Must(order => order.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                               order.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortOrder must be 'asc' or 'desc'.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, GetAllMoviesParameters.MaxPageSize)
                .WithMessage($"PageSize must be between 1 and {GetAllMoviesParameters.MaxPageSize}.");

            RuleFor(x => x.FilterBy)
                .Must(BeAValidFilterProperty)
                .When(x => !string.IsNullOrEmpty(x.FilterBy))
                .WithMessage($"FilterBy must be one of the following: {string.Join(", ", Enum.GetNames(typeof(FilterByOptions)))}");

            RuleFor(x => x.FilterValue)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.FilterBy))
                .WithMessage("FilterValue must be provided when FilterBy is specified.");
        }

        private static bool BeAValidSortColumn(string? sortColumn)
        {
            return Enum.TryParse(typeof(SortByOptions), sortColumn, true, out _);
        }

        private static bool BeAValidFilterProperty(string? filterBy)
        {
            return Enum.TryParse(typeof(FilterByOptions), filterBy, true, out _);
        }
    }
}
