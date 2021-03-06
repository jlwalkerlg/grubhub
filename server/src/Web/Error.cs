using System;
using System.Collections.Generic;
using System.Linq;

namespace Web
{
    public class Error
    {
        private Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message must not be empty.");
            }
        }

        private Error(Dictionary<string, string> errors)
            : this(ErrorType.ValidationError, "Invalid request.")
        {
            Errors = errors;
        }

        public string Message { get; }
        public ErrorType Type { get; }
        public Dictionary<string, string> Errors { get; }

        public static Error BadRequest(string message)
        {
            return new Error(ErrorType.BadRequest, message);
        }

        public static Error Unauthenticated()
        {
            return new Error(ErrorType.Unauthenticated, "Unauthenticated.");
        }

        public static Error Unauthorised(string message = null)
        {
            return new Error(ErrorType.Unauthorised, message ?? "Unauthorised.");
        }

        public static Error NotFound(string message)
        {
            return new Error(ErrorType.NotFound, message);
        }

        public static Error ValidationError(string field, string error)
        {
            return new Error(new Dictionary<string, string>()
            {
                { field, error },
            });
        }

        public static Error ValidationError(Dictionary<string, string> errors)
        {
            return new Error(errors);
        }

        public static Error Internal(string message)
        {
            return new Error(ErrorType.Internal, message);
        }

        public override string ToString()
        {
            var message = Type == ErrorType.ValidationError
                ? string.Join(", ", Errors.Select(x => $"{x.Key}: {x.Value}"))
                : Message;

            return $"Error ({Type.ToString()}): {message}";
        }

        public static implicit operator string(Error error) => error.ToString();
        public static implicit operator bool(Error error) => error != null;
    }

    public enum ErrorType
    {
        BadRequest,
        Unauthenticated,
        Unauthorised,
        NotFound,
        ValidationError,
        Internal,
    }
}
