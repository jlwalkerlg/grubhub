namespace FoodSnap.Application
{
    public class Result
    {
        public bool IsSuccess { get; private set; }

        private IError error;
        public virtual IError Error
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

        protected Result(IError error)
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

        public static Result Fail(IError error)
        {
            return new Result(error);
        }
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

        protected Result(IError error) : base(error)
        {
        }

        public override IError Error
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

        public new static Result<T> Fail(IError error)
        {
            return new Result<T>(error);
        }
    }
}
