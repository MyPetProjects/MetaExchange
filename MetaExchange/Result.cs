using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaExchange
{
    /// <summary>
    /// Results of method execution
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        /// <summary>
        /// get successful result
        /// </summary>
        /// <returns></returns>
        public static Result<T> Ok()
        {
            return new Result<T> { Success = true };
        }

        /// <summary>
        /// get successful result with data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Ok(T data)
        {
            return new Result<T>
            {
                Success = true,
                Data = data
            };
        }

        /// <summary>
        /// get fail result
        /// </summary>
        /// <param name="message">error message</param>
        /// <returns></returns>
        public static Result<T> Fail(string message = null)
        {
            return new Result<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}
