namespace PharmaStoreWeb.Models.Medicines
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MedicineTypeName { get; set; }

        public string ContentQuantity { get; set; }

        public decimal Price { get; set; }

        public string Producer { get; set; }

        public string Description { get; set; }
    }
}
