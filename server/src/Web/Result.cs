using System.Collections.Generic;

namespace Web
{
    public class Result
    {
        private Error error;

        public Result()
        {
            IsSuccess = true;
        }

        protected Result(Error error)
        {
            Error = error;
        }

        public bool IsSuccess { get; private set; }

        public virtual Error Error
        {
            get => error;
            set
            {
                IsSuccess = false;
                error = value;
            }
        }

        public Dictionary<string, string> Errors => error?.Errors;

        public static Result Ok()
        {
            return new Result();
        }

        public static Result<T> Ok<T>(T value)
        {
            return Result<T>.Ok(value);
        }

        public static Result Fail(Error error)
        {
            return new Result(error);
        }

        public static implicit operator bool(Result result) => result.IsSuccess;
        public static implicit operator Result(Error error) => new(error);
    }

    public class Result<T> : Result
    {
        public Result() : base()
        {
        }

        protected Result(T value) : base()
        {
            Value = value;
        }

        protected Result(Error error) : base(error)
        {
        }

        public T Value { get; private set; }

        public override Error Error
        {
            set
            {
                base.Error = value;
                Value = default(T);
            }
        }

        public void Deconstruct(out T value, out Error error)
        {
            value = Value;
            error = Error;
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public static new Result<T> Fail(Error error)
        {
            return new Result<T>(error);
        }

        public static implicit operator bool(Result<T> result) => result.IsSuccess;
        public static implicit operator Result<T>(Error error) => new(error);
    }
}
