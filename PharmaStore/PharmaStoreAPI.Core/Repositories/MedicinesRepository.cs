namespace PharmaStoreAPI.Core.Repositories
{
    using Contracts;
    using PharmaStoreAPI.Core.ViewModels.Core;
    using PharmaStoreAPI.Core.ViewModels.Medicines;
    using System.Collections.Generic;

    public class MedicinesRepository : IMedicinesRepository
    {
        public OperationResult<IEnumerable<Medicine>> GetMedicineList(GetMedicinesViewModel filters)
        {
            return new OperationResult<IEnumerable<Medicine>>(new List<Medicine>());
        }
    }
}
