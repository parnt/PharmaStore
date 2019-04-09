namespace PharmaStoreWeb.Models.Core
{
    public class Error
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public string FieldName { get; set; }
    }
}
