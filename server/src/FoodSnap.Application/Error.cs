using System.Collections.Generic;

namespace FoodSnap.Application
{
    public class Error
    {
        private Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        private Error(Dictionary<string, string> errors)
        {
            Type = ErrorType.ValidationError;
            Message = "Invalid request.";
            Errors = errors;
        }

        public string Message { get; }
        public ErrorType Type { get; }
        public Dictionary<string, string> Errors { get; }

        public enum ErrorType
        {
            BadRequest,
            ValidationError,
            ServerError,
        }

        public static Error BadRequest(string message)
        {
            return new Error(ErrorType.BadRequest, message);
        }

        public static Error ValidationError(Dictionary<string, string> errors)
        {
            return new Error(errors);
        }

        public static Error ServerError(string message)
        {
            return new Error(ErrorType.ServerError, message);
        }
    }
}
