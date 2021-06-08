using System;
using System.Collections.Generic;
using System.Text;

namespace Serialize
{
    public class Result<T>
    {
        public readonly bool Success;
        public readonly T Value;
        public Result(bool success, T value)
        {
            Success = success;
            Value = value;
        }

    }

    public static class Result
    {
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(true, value);
        }

        public static Result<T> Fail<T>()
        {
            return new Result<T>(false, default(T));
        }
    }
}
