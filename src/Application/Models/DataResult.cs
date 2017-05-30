using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class DataResult
    {
        private static readonly DataResult _successfulResult = new DataResult { Successful = true };
        protected DataResult() { }

        public bool Successful { get; protected set; }
        public string Error { get; protected set; }

        public static DataResult CreateSuccessfulResult()
        {
            return _successfulResult;
        }

        public static DataResult CreateFailedResult(string error)
        {
            return new DataResult { Error = error, Successful = false };
        }

        public static implicit operator DataResult(string msg)
        {
            return new DataResult { Error = msg, Successful = false };
        }
    }

    public class DataResult<T> : DataResult
    {
        public T Data { get; set; }

        public static DataResult<T> CreateSuccessfulResult(T data)
        {
            return new DataResult<T> { Data = data, Successful = true };
        }

        public new static DataResult<T> CreateFailedResult(string error)
        {
            return new DataResult<T> { Error = error, Successful = false };
        }

        public static implicit operator DataResult<T>(T data)
        {
            return CreateSuccessfulResult(data);
        }
    }
}
