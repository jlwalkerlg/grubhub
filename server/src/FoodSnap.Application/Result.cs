namespace FoodSnap.Application
{
    public class Result
    {
        public bool IsSuccess { get; private set; }

        private Error error;
        public virtual Error Error
        {
            get
            {
                return error;
            }
            set
            {
                IsSuccess = false;
                error = value;
            }
        }

        public Result()
        {
            IsSuccess = true;
        }

        protected Result(Error error)
        {
            Error = error;
        }

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

        public static implicit operator Result(Error error) => new(error);
    }

    public class Result<T> : Result
    {
        public T Value { get; private set; }

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

        public override Error Error
        {
            set
            {
                base.Error = value;
                Value = default(T);
            }
        }

        public static Result<T> Ok(T value)
        {
            return new Result<T>(value);
        }

        public new static Result<T> Fail(Error error)
        {
            return new Result<T>(error);
        }

        public static implicit operator Result<T>(Error error) => new(error);
    }
}
