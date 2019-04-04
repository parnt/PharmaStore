namespace PharmaStoreAPI.Core.ViewModels.Core
{
    public class OperationError
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public string FieldName { get; set; }

        public OperationError(int errorCode, string message, string fieldName = null)
        {
            ErrorCode = errorCode;
            Message = message;
            FieldName = fieldName;
        }
    }
}
