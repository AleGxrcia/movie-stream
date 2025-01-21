using FluentValidation;

namespace MovieStream.Core.Application.Features.Episodes.Queries.GetAllEpisodes
{
    public class GetAllEpisodesParametersValidator : AbstractValidator<GetAllEpisodesParameters>
    {
        private static readonly string[] AllowedSortColumns = { "TvSerieName", "SeasonNumber", "ReleaseDate" };
        private static readonly string[] AllowedFilterColumns = { "TvSerieName", "ReleaseDate", "SeasonNumber" };

        public GetAllEpisodesParametersValidator()
        {
            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn)
                .When(x => !string.IsNullOrEmpty(x.SortColumn))
                .WithMessage($"SortColumn must be one of the following: {string.Join(", ", AllowedSortColumns)}");

            RuleFor(x => x.SortOrder)
                .Must(order => order.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                               order.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortOrder must be 'asc' or 'desc'.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, GetAllEpisodesParameters.MaxPageSize)
                .WithMessage($"PageSize must be between 1 and {GetAllEpisodesParameters.MaxPageSize}.");

            RuleFor(x => x.FilterBy)
                .Must(BeAValidFilterProperty)
                .When(x => !string.IsNullOrEmpty(x.FilterBy))
                .WithMessage($"FilterBy must be one of the following: {string.Join(", ", AllowedFilterColumns)}");

            RuleFor(x => x.FilterValue)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.FilterBy))
                .WithMessage("FilterValue must be provided when FilterBy is specified.");
        }

        private static bool BeAValidSortColumn(string? sortColumn)
        {
            return AllowedSortColumns.Contains(sortColumn, StringComparer.OrdinalIgnoreCase);
        }

        private static bool BeAValidFilterProperty(string? filterBy)
        {
            return AllowedFilterColumns.Contains(filterBy, StringComparer.OrdinalIgnoreCase);
        }
    }
}
