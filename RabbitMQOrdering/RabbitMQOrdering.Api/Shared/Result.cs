using System;

namespace RabbitMQOrdering.Api.Shared{
    public enum ResultCode
    {
        Unknown = 0,
        Ok = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        NotFound = 404,
        Conflict = 409,
        UnprocessableEntity = 422,
        InternalServerError = 500
    }

    [Serializable]
    public class Result
    {
        public static readonly Result Success = new Result(true, string.Empty, ResultCode.Ok);

        public Result(bool isSuccess, string error, ResultCode code = ResultCode.Unknown)
        {
            IsSuccess = isSuccess;
            Error = error;
            Code = code;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public ResultCode Code { get; }
        public int StatusCode { get { return (int)Code; } }

        public static Result Ok()
        {
            return Success;
        }

        public static Result<T> Ok<T>(T value, ResultCode code = ResultCode.Ok)
        {
            return new Result<T>(value, true, string.Empty, code);
        }

        public static Result<T> Created<T>(T value)
        {
            return Ok(value, ResultCode.Created);
        }

        public static Result Fail(string message, ResultCode code = ResultCode.BadRequest)
        {
            return new Result(false, message, code);
        }

        public static Result<T> Fail<T>(string message, ResultCode code = ResultCode.BadRequest)
        {
            return new Result<T>(default(T), false, message, code);
        }

        public Result<T> As<T>(T value = default(T))
        {
            return new Result<T>(value, IsSuccess, Error);
        }

        public static implicit operator bool(Result result)
        {
            return result.IsSuccess;
        }

        public static implicit operator Result(bool success)
        {
            if (success)
            {
                return Success;
            }

            return Fail(null);
        }
    }

    [Serializable]
    public class Result<T> : Result
    {
        private readonly T value;

        protected internal Result(T value, bool isSuccess, string error,
            ResultCode code = ResultCode.Unknown) : base(isSuccess, error, code)
        {
            this.value = value;
        }

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException();
                }

                return value;
            }
        }
    }
}