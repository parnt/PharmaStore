namespace PharmaStoreAPI.Core.Models
{
    using Microsoft.EntityFrameworkCore;
    using PharmaStoreAPI.Core.Models.Medicines;

    public partial class PharmaStoreContext
    {
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineType> MedicineTypes { get; set; }
    }
}
