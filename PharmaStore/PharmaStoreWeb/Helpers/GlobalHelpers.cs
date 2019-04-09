namespace PharmaStoreWeb.Helpers
{
    public static class GlobalHelpers
    {
        public static string ParsePriceWithCurrency(string price, string currency)
        {
            decimal.TryParse(price, out var _price);
            price = _price.ToString("# ##0.00").Replace('.', ',');
            price = price.Trim();
            price = price.Replace('.', ',');

            return $"{price} {currency}".Replace('.', ',');
        }
    }
}
