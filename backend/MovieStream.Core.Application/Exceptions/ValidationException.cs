using FluentValidation.Results;

namespace MovieStream.Core.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            foreach (var failure in failures) 
            {
                Errors.Add(failure.ErrorMessage);
            }
        }
    }
}
