using System.Collections.Generic;
using System.Linq;

namespace PharmaStoreAPI.ViewModels.Core
{
    public class Result<T> where T : class
    {
        public int TotalCount { get; set; }

        public string Message { get; set; }

        public T Items { get; set; }

        public Result(T items, string message = null)
        {
            Items = items;
            Message = message;

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
