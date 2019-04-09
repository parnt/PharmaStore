
namespace PharmaStoreWeb.Models.Core
{
    using System.Collections.Generic;

    public class OperationResult<T>
    {
        public Result<T> Result { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<Error> Errors { get; set; }

        public OperationResult(Result<T> result)
        {
            Result = result;
            IsSuccess = true;
        }

        public OperationResult(IEnumerable<Error> errors)
        {
            Errors = errors;
            IsSuccess = false;
        }

        public OperationResult(Error error)
        {
            Errors = new List<Error> { error };
            IsSuccess = false;
        }
    }
}
