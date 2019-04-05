namespace PharmaStoreAPI.Helpers
{
    using Core.Enums;
    using Core.Resources;
    using Core.ViewModels.Core;
    using System.Linq;

    public static class GlobalHelpers
    {
        public static OperationError ModelStateError() =>
            new OperationError((int) ErrorCodes.BadRequest, ErrorResources.ModelStateError);

        public static string TrimString(this string str) => str?.Trim() ?? str;

        public static T TrimObj<T>(this T obj)
        {
            var objectStringFields = obj.GetType().GetProperties().Where(x => x.PropertyType == typeof(string));

            foreach (var field in objectStringFields)
            {
                var value = (string)field.GetValue(obj, null);
                if (!string.IsNullOrEmpty(value))
                {
                    field.SetValue(obj, value.Trim(), null);
                    }
            }

            return obj;
        }
    }
}
