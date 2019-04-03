using System.Collections.Generic;

namespace PharmaStoreAPI.Core.ViewModels.Core
{
    public class OperationResult<T> where T : class
    {
        public Result<T> Result { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<OperationError> Errors { get; set; }

        public OperationResult(T result, string message = null)
        {
            Result = new Result<T>(result, message);
            IsSuccess = true;
        }

        public OperationResult(IEnumerable<OperationError> errors)
        {
            Errors = errors;
            IsSuccess = false;
        }
    }
}
