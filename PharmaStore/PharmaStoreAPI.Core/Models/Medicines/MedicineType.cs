namespace PharmaStoreAPI.Core.Models.Medicines
{
    using System.Collections.Generic;

    public class MedicineType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
