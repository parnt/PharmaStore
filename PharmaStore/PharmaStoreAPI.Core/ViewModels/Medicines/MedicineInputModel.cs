namespace PharmaStoreAPI.Core.ViewModels.Medicines
{
    public class MedicineInputModel
    {
        public string Name { get; set; }

        public int MedicineTypeId { get; set; }

        public string ContentQuantity { get; set; }

        public decimal Price { get; set; }

        public string Producer { get; set; }

        public string Description { get; set; }
    }
}
