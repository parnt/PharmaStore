namespace PharmaStoreAPI.Core.Repositories
{
    using Contracts;
    using Enums;
    using Models;
    using PharmaStoreAPI.Core.ViewModels.Core;
    using PharmaStoreAPI.Core.ViewModels.Medicines;
    using Resources;
    using System.Collections.Generic;
    using System.Linq;

    public class MedicinesRepository : IMedicinesRepository
    {
        public OperationResult<IEnumerable<MedicineHeader>> GetMedicineList(GetMedicinesViewModel filters)
        {
            try
            {
                var context = new PharmaStoreContext();

                var medicineList = context.Medicines.Where(x => string.IsNullOrEmpty(filters.SearchValue)
                    ? true
                    : (x.MedicineType.Name.Contains(filters.SearchValue) ||
                       x.ContentQuantity.Contains(filters.SearchValue) ||
                       x.Id.ToString().Contains(filters.SearchValue) ||
                       x.Name.Contains(filters.SearchValue) ||
                       x.Producer.Contains(filters.SearchValue))).Select(x => new MedicineHeader
                       {
                           Id = x.Id,
                           Name = x.Name,
                           ContentQuantity = x.ContentQuantity,
                           Producer = x.Producer,
                           Price = x.Price
                       }).ToList();

                return new OperationResult<IEnumerable<MedicineHeader>>(medicineList);
            }
            catch
            {
                return new OperationResult<IEnumerable<MedicineHeader>>(
                    new OperationError((int) ErrorCodes.InternalServerError, ErrorResources.DatabaseError));
            }
        }

        public OperationResult<Medicine> GetMedicineDetails(int id)
        {
            try
            {
                var context = new PharmaStoreContext();

                var medicine = context.Medicines.Where(x => x.Id == id)
                    .Select(x => new Medicine
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ContentQuantity = x.ContentQuantity,
                        Producer = x.Producer,
                        Price = x.Price,
                        MedicineTypeName = x.MedicineType.Name,
                        Description = x.Description
                    }).SingleOrDefault();

                return medicine != null
                    ? new OperationResult<Medicine>(medicine)
                    : new OperationResult<Medicine>(new OperationError((int) ErrorCodes.NotFound,
                        ErrorResources.ItemNotFound));
            }
            catch
            {
                return new OperationResult<Medicine>(new OperationError((int) ErrorCodes.InternalServerError,
                    ErrorResources.DatabaseError));
            }
        }
    }
}
