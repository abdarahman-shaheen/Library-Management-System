using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace LibraryManagementSystem.Application.Validators.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }
    }
}
