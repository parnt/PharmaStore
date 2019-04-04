namespace PharmaStoreAPI.Core.ViewModels.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public class Result<T> where T : class
    {
        public int TotalCount { get; set; }

        public T Items { get; set; }

        public Result(T items)
        {
            Items = items;

            if (Items != null)
            {
                if (Items.GetType().GetInterfaces().Where(t => t.IsGenericType).Any(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                {
                    try
                    {
                        TotalCount = (Items as IEnumerable<object>).Count();
                    }
                    catch
                    {
                        TotalCount = 1;
                    }
                }
                else
                {
                    TotalCount = 1;
                }
            }
            else
            {
                TotalCount = 0;
            }
        }
    }
}
