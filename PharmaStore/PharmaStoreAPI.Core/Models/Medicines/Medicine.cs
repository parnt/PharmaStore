﻿namespace PharmaStoreAPI.Core.Models.Medicines
{
    public class Medicine
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MedicineTypeId { get; set; }

        public string ContentQuantity { get; set; }

        public decimal Price { get; set; }

        public string Producer { get; set; }

        public string Description { get; set; }

        public virtual MedicineType MedicineType { get; set; }
    }
}
