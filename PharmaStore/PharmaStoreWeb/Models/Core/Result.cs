namespace PharmaStoreWeb.Models.Core
{
    public class Result<T>
    {
        public int TotalCount { get; set; }

        public T Items { get; set; }
    }
}
