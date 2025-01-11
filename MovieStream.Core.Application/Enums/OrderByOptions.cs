using System.ComponentModel.DataAnnotations;

namespace MovieStream.Core.Application.Enums
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")] SimpleOrder = 0,
        [Display(Name = "Publication Date ↑")] ByPublicationDate,
    }
}
