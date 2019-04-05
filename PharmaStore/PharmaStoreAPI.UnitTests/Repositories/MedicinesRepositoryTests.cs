namespace PharmaStoreAPI.UnitTests.Repositories
{
    using Core.Models;
    using Core.Repositories;
    using Core.Resources;
    using Core.ViewModels.Medicines;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;
    using PharmaStoreAPI.Core.Enums;
    using System.Collections.Generic;
    using System.Linq;
    using Medicine = Core.Models.Medicines.Medicine;
    using MedicineType = Core.Models.Medicines.MedicineType;

    class MedicinesRepositoryTests
    {
        private Mock<PharmaStoreContext> mockContext;
        private Mock<DbSet<MedicineType>> mockMedicineType;
        private Mock<DbSet<Medicine>> mockMedicine;

        private IQueryable<MedicineType> medicineTypeList;
        private IQueryable<Medicine> medicineList;

        [SetUp]
        public void Setup()
        {
            mockContext = new Mock<PharmaStoreContext>();
            mockMedicineType = new Mock<DbSet<MedicineType>>();
            mockMedicine = new Mock<DbSet<Medicine>>();

            medicineTypeList = new List<MedicineType>
            {
                new MedicineType
                {
                    Id = 1,
                    Name = "Liquid"
                },
                new MedicineType
                {
                    Id = 2,
                    Name = "Tablet"
                }
            }.AsQueryable();

            medicineList = new List<Medicine>
            {
                new Medicine
                {
                    Id = 1,
                    Name = "Nazwa leku",
                    ContentQuantity = "20szt",
                    Price = 6.99M,
                    Producer = "Pharma",
                    Description = "Super lek",
                    MedicineType = medicineTypeList.Single(x => x.Id == 2),
                    MedicineTypeId = 2,
                },
                new Medicine
                {
                    Id = 2,
                    Name = "Nazwa drugiego leku",
                    ContentQuantity = "40szt",
                    Price = 11.99M,
                    Producer = "Pharma",
                    Description = "Super lek powiększony",
                    MedicineType = medicineTypeList.Single(x => x.Id == 2),
                    MedicineTypeId = 2
                },
                new Medicine
                {
                    Id = 3,
                    Name = "Syropek",
                    ContentQuantity = "200ml",
                    Price = 16.99M,
                    Producer = "PharmaStone",
                    Description = "Na kaszelek",
                    MedicineType = medicineTypeList.Single(x => x.Id == 1),
                    MedicineTypeId = 1
                }
            }.AsQueryable();

            mockMedicineType.As<IQueryable<MedicineType>>().Setup(x => x.Provider).Returns(medicineTypeList.Provider);
            mockMedicineType.As<IQueryable<MedicineType>>().Setup(x => x.Expression).Returns(medicineTypeList.Expression);
            mockMedicineType.As<IQueryable<MedicineType>>().Setup(x => x.ElementType).Returns(medicineTypeList.ElementType);
            mockMedicineType.As<IQueryable<MedicineType>>().Setup(x => x.GetEnumerator()).Returns(medicineTypeList.GetEnumerator());

            mockMedicine.As<IQueryable<Medicine>>().Setup(x => x.Provider).Returns(medicineList.Provider);
            mockMedicine.As<IQueryable<Medicine>>().Setup(x => x.Expression).Returns(medicineList.Expression);
            mockMedicine.As<IQueryable<Medicine>>().Setup(x => x.ElementType).Returns(medicineList.ElementType);
            mockMedicine.As<IQueryable<Medicine>>().Setup(x => x.GetEnumerator()).Returns(medicineList.GetEnumerator());
        }

        [Test]
        public void GetMedicineList_CorrectData_ListOfMedicines()
        {
            // Arrange
            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.GetMedicineList(It.IsAny<GetMedicinesViewModel>());

            // Assert
            Assert.IsNull(result.Errors);
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Result.Items.Count());
            Assert.AreEqual(3, result.Result.TotalCount);
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void GetMedicineList_CorrectDataWithFilters_ListOfMedicines()
        {
            // Arrange
            var filters = new GetMedicinesViewModel
            {
                SearchValue = "lek"
            };

            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.GetMedicineList(filters);

            // Assert
            Assert.IsNull(result.Errors);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Result.Items.Count());
            Assert.AreEqual(2, result.Result.TotalCount);
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void GetMedicineList_DatabaseError_InternalServerError()
        {
            // Arrange
            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.GetMedicineList(It.IsAny<GetMedicinesViewModel>());

            // Assert
            Assert.IsNotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Result);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, result.Errors.Single().Message);
            Assert.AreEqual((int) ErrorCodes.InternalServerError, result.Errors.Single().ErrorCode);
        }

        [Test]
        public void GetMedicineDetails_CorrectData_DetailsOfMedicine()
        {
            // Arrange
            int id = 2;

            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.GetMedicineDetails(id);

            // Assert
            Assert.IsNull(result.Errors);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Result.TotalCount);
            Assert.AreEqual(2, result.Result.Items.Id);
            Assert.AreEqual("Tabletki", result.Result.Items.MedicineTypeName);
            Assert.AreEqual("Nazwa drugiego leku", result.Result.Items.Name);
            Assert.AreEqual("40szt", result.Result.Items.ContentQuantity);
            Assert.AreEqual(11.99M, result.Result.Items.Price);
            Assert.AreEqual("Pharma", result.Result.Items.Producer);
            Assert.AreEqual("Super lek powiększony", result.Result.Items.Description);
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void GetMedicineDetails_IdDoNotExists_NotFoundError()
        {
            // Arrange
            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.GetMedicineDetails(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result.Errors);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.ItemNotFound, result.Errors.Single().Message);
            Assert.AreEqual((int)ErrorCodes.NotFound, result.Errors.Single().ErrorCode);
            Assert.False(result.IsSuccess);
        }

        [Test]
        public void GetMedicineDetails_DatabaseError_InternalServerError()
        {
            // Arrange
            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.GetMedicineDetails(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Result);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, result.Errors.Single().Message);
            Assert.AreEqual((int)ErrorCodes.InternalServerError, result.Errors.Single().ErrorCode);
        }

        [Test]
        public void CreateNewMedicine_CorrectData_CreatedMedicine()
        {
            // Arrange
            var model = new MedicineInputModel
            {
                Name = "Nowy lek",
                ContentQuantity = "100ml",
                Price = 23.49M,
                Description = "Nowy jeszcze lepszy super lek",
                Producer = "Pharma",
                MedicineTypeId = 1
            };

            mockContext.Setup(x => x.MedicineTypes).Returns(mockMedicineType.Object);
            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.CreateNewMedicine(model);

            // Assert
            Assert.IsNull(result.Errors);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Result.TotalCount);
            Assert.AreEqual(ResultResources.CreatingMedicineComplete, result.Result.Items);
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void CreateNewMedicine_InvalidDataNullModelFields_BadRequestErrors()
        {
            // Arrange
            var model = new MedicineInputModel();

            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.CreateNewMedicine(model);

            // Assert
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Errors.Count());
            Assert.AreEqual(1, result.Errors.Select(x => x.Message).Distinct().Count());
            Assert.False(result.IsSuccess);
        }

        [Test]
        public void CreateNewMedicine_InvalidDataTooShortModelFields_BadRequestErrors()
        {
            // Arrange
            var model = new MedicineInputModel
            {
                Name = "a",
                ContentQuantity = "aa",
                Description = "a",
                Producer = "a"
            };

            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.CreateNewMedicine(model);

            // Assert
            Assert.IsNull(result.Result);
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Errors.Count());
            Assert.False(result.IsSuccess);
        }

        [Test]
        public void CreateNewMedicine_BadMedicineTypeId_BadRequestError()
        {
            // Arrange
            var model = new MedicineInputModel
            {
                Name = "Nowy lek",
                ContentQuantity = "100ml",
                Price = 23.49M,
                Description = "Nowy jeszcze lepszy super lek",
                Producer = "Pharma",
                MedicineTypeId = 0
            };

            mockContext.Setup(x => x.MedicineTypes).Returns(mockMedicineType.Object);
            mockContext.Setup(x => x.Medicines).Returns(mockMedicine.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.CreateNewMedicine(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Result);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.InvalidInsertDatabaseData, result.Errors.Single().Message);
            Assert.AreEqual((int)ErrorCodes.BadRequest, result.Errors.Single().ErrorCode);
        }

        [Test]
        public void CreateNewMedicine_DatabaseError_InternalServerError()
        {
            // Arrange
            var model = new MedicineInputModel
            {
                Name = "Nowy lek",
                ContentQuantity = "100ml",
                Price = 23.49M,
                Description = "Nowy jeszcze lepszy super lek",
                Producer = "Pharma",
                MedicineTypeId = 1
            };

            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.CreateNewMedicine(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Result);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, result.Errors.Single().Message);
            Assert.AreEqual((int)ErrorCodes.InternalServerError, result.Errors.Single().ErrorCode);
        }

        [Test]
        public void GetMedicineTypes_CorrectData_ListOfMedicineTypes()
        {
            // Arrange
            mockContext.Setup(x => x.MedicineTypes).Returns(mockMedicineType.Object);

            var medicinesRepository = new MedicinesRepository(mockContext.Object);

            // Act
            var result = medicinesRepository.GetMedicineTypes();

            // Assert
            Assert.IsNull(result.Errors);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Result.Items.Count());
            Assert.AreEqual(2, result.Result.TotalCount);
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void GetMedicineTypes_DatabaseError_InternalServerError()
        {
            // Arrange
            var medicinesRepository = new MedicinesRepository(It.IsAny<PharmaStoreContext>());

            // Act
            var result = medicinesRepository.GetMedicineTypes();

            // Assert
            Assert.IsNotNull(result);
            Assert.False(result.IsSuccess);
            Assert.IsNull(result.Result);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, result.Errors.Single().Message);
            Assert.AreEqual((int)ErrorCodes.InternalServerError, result.Errors.Single().ErrorCode);
        }
    }
}
