namespace PharmaStoreAPI.Core.ViewModels.Core
{
    public class OperationError
    {
        public OperationError(int errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
