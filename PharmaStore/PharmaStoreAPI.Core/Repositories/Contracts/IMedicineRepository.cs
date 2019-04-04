namespace PharmaStoreAPI.Core.Repositories.Contracts
{
    using PharmaStoreAPI.Core.ViewModels.Core;
    using PharmaStoreAPI.Core.ViewModels.Medicines;
    using System.Collections.Generic;

    public interface IMedicinesRepository
    {
        OperationResult<IEnumerable<MedicineHeader>> GetMedicineList(GetMedicinesViewModel filters);
        OperationResult<Medicine> GetMedicineDetails(int id);
    }
}
