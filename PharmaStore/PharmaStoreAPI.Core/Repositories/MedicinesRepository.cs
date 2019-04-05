namespace PharmaStoreAPI.Core.Repositories
{
    using Contracts;
    using Enums;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using PharmaStoreAPI.Core.ViewModels.Core;
    using PharmaStoreAPI.Core.ViewModels.Medicines;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MedicinesRepository : IMedicinesRepository
    {
        private readonly PharmaStoreContext _context;

        public MedicinesRepository(PharmaStoreContext context)
        {
            _context = context;
        }

        public OperationResult<IEnumerable<MedicineHeader>> GetMedicineList(GetMedicinesViewModel filters)
        {
            try
            {
                var medicineList = _context.Medicines.Where(x =>
                    (filters == null || string.IsNullOrEmpty(filters.SearchValue))
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
                var medicine = _context.Medicines.Where(x => x.Id == id)
                    .Select(x => new Medicine
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ContentQuantity = x.ContentQuantity,
                        Producer = x.Producer,
                        Price = x.Price,
                        MedicineTypeName = TranslateResources.ResourceManager.GetString(x.MedicineType.Name),
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

        public OperationResult<string> CreateNewMedicine(MedicineInputModel newMedicine)
        {
            var errorList = MedicineValidation(newMedicine);

            if (errorList.Any())
                return new OperationResult<string>(errorList);

            try
            {
                if (!_context.MedicineTypes.Select(x => x.Id).Contains(newMedicine.MedicineTypeId))
                    return new OperationResult<string>(new OperationError((int)ErrorCodes.BadRequest, ErrorResources.InvalidInsertDatabaseData));

                _context.Medicines.Add(new Models.Medicines.Medicine
                {
                    Name = newMedicine.Name,
                    MedicineTypeId = newMedicine.MedicineTypeId,
                    Producer = newMedicine.Producer,
                    Price = newMedicine.Price,
                    ContentQuantity = newMedicine.ContentQuantity,
                    Description = newMedicine.Description
                });

                _context.SaveChanges();

                return new OperationResult<string>(ResultResources.CreatingMedicineComplete);
            }
            catch
            {
                return new OperationResult<string>(new OperationError((int)ErrorCodes.InternalServerError,
                    ErrorResources.DatabaseError));
            }
        }

        private IEnumerable<OperationError> MedicineValidation(MedicineInputModel medicine)
        {
            var errors = new List<OperationError>();

            if (string.IsNullOrEmpty(medicine.Name))
            {
                errors.Add(new OperationError((int) ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty, nameof(medicine.Name)));
            }
            else if (medicine.Name.Length < 2 || medicine.Name.Length > 50)
            {
                var errorMessage = string.Format(ErrorResources.InvalidFieldLength, 1, 50);

                errors.Add(new OperationError((int)ErrorCodes.BadRequest, errorMessage, nameof(medicine.Name)));
            }

            if (string.IsNullOrEmpty(medicine.ContentQuantity))
            {
                errors.Add(new OperationError((int)ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty, nameof(medicine.ContentQuantity)));
            }
            else if (medicine.ContentQuantity.Length < 3 || medicine.ContentQuantity.Length > 10)
            {
                var errorMessage = string.Format(ErrorResources.InvalidFieldLength, 2, 10);

                errors.Add(new OperationError((int)ErrorCodes.BadRequest, errorMessage, nameof(medicine.ContentQuantity)));
            }

            if (string.IsNullOrEmpty(medicine.Producer))
            {
                errors.Add(new OperationError((int)ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty, nameof(medicine.Producer)));
            }
            else if (medicine.Producer.Length < 2 || medicine.Producer.Length > 50)
            {
                var errorMessage = string.Format(ErrorResources.InvalidFieldLength, 1, 50);

                errors.Add(new OperationError((int)ErrorCodes.BadRequest, errorMessage, nameof(medicine.Producer)));
            }

            if (string.IsNullOrEmpty(medicine.Description))
            {
                errors.Add(new OperationError((int)ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty, nameof(medicine.Description)));
            }
            else if (medicine.Description.Length < 2 || medicine.Description.Length > 2000)
            {
                var errorMessage = string.Format(ErrorResources.InvalidFieldLength, 1, 2000);

                errors.Add(new OperationError((int)ErrorCodes.BadRequest, errorMessage, nameof(medicine.Description)));
            }

            return errors;
        }

        public OperationResult<IEnumerable<MedicineType>> GetMedicineTypes()
        {
            try
            {
                var medicineTypes = _context.MedicineTypes.Select(x => new MedicineType
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return new OperationResult<IEnumerable<MedicineType>>(medicineTypes);
            }
            catch
            {
                return new OperationResult<IEnumerable<MedicineType>>(
                    new OperationError((int)ErrorCodes.InternalServerError, ErrorResources.DatabaseError));
            }
        }
    }
}