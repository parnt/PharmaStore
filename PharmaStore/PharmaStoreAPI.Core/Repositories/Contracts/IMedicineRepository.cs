using PharmaStoreAPI.Core.ViewModels.Core;
using PharmaStoreAPI.Core.ViewModels.Medicines;
using System.Collections.Generic;

namespace PharmaStoreAPI.Core.Repositories.Contracts
{
    public interface IMedicinesRepository
    {
        OperationResult<IEnumerable<Medicine>> GetMedicineList(GetMedicinesViewModel filters);
    }
}
