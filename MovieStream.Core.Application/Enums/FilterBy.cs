using System.ComponentModel.DataAnnotations;

namespace MovieStream.Core.Application.Enums
{
    public enum FilterBy
    {
        [Display(Name = "All")] NoFilter = 0,
        [Display(Name = "By Genres")] ByGenres,
        [Display(Name = "By Year published")] ByPublicationYear,
        [Display(Name = "By Production company")] ByProductionCompany
    }
}
