using NUnit.Framework;

namespace PharmaStoreAPI.UnitTests.Controllers
{
    using Core.Resources;
    using Core.ViewModels.Core;
    using Core.ViewModels.Medicines;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using PharmaStoreAPI.Controllers;
    using PharmaStoreAPI.Core.Enums;
    using PharmaStoreAPI.Core.Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    class MedicinesControllerTests
    {
        private Mock<IMedicinesRepository> mock;
        private List<MedicineHeader> returnedMedicineList;

        [SetUp]
        public void Setup()
        {
            mock = new Mock<IMedicinesRepository>();
            returnedMedicineList = new List<MedicineHeader>
            {
                new MedicineHeader
                {
                    Id = 1,
                    Name = "Nazwa leku",
                    ContentQuantity = "20szt",
                    Price = 6.99M,
                    Producer = "Pharma"
                },
                new MedicineHeader
                {
                    Id = 2,
                    Name = "Nazwa drugiego leku",
                    ContentQuantity = "10ml",
                    Price = 16.99M,
                    Producer = "Pharma"
                }
            };
        }

        [Test]
        public void GetMedicineList_CorrectData_ListOfMedicines()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineList(It.IsAny<GetMedicinesViewModel>()))
                .Returns(() =>
                    new OperationResult<IEnumerable<MedicineHeader>>(new List<MedicineHeader>(returnedMedicineList)));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetMedicineList(It.IsAny<GetMedicinesViewModel>()) as OkObjectResult;
            var response = result.Value as Result<IEnumerable<MedicineHeader>>;

            // Assert
            Assert.AreEqual((int) HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(2, response.TotalCount);
            Assert.AreEqual(2, response.Items.Count());
        }

        [Test]
        public void GetMedicineList_CorrectDataWithSearchValue_ListOfMedicines()
        {
            // Arrange
            var searchValue = new GetMedicinesViewModel
            {
                SearchValue = "lek"
            };

            mock.Setup(x => x.GetMedicineList(It.IsAny<GetMedicinesViewModel>()))
                .Returns(() =>
                    new OperationResult<IEnumerable<MedicineHeader>>(new List<MedicineHeader>(returnedMedicineList)));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetMedicineList(searchValue) as OkObjectResult;
            var response = result.Value as Result<IEnumerable<MedicineHeader>>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(2, response.TotalCount);
            Assert.AreEqual(2, response.Items.Count());
        }

        [Test]
        public void GetMedicineList_InvalidData_ModelStateError()
        {
            // Arrange
            var medicinesController = new MedicinesController(It.IsAny<IMedicinesRepository>());

            medicinesController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = medicinesController.GetMedicineList(It.IsAny<GetMedicinesViewModel>()) as BadRequestObjectResult;
            var response = result.Value as List<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorResources.ModelStateError, response.Single().Message);
        }

        [Test]
        public void GetMedicineList_DatabaseConnectionProblem_DatabaseError()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineList(It.IsAny<GetMedicinesViewModel>()))
                .Returns(() =>
                    new OperationResult<IEnumerable<MedicineHeader>>(
                        new OperationError((int) ErrorCodes.InternalServerError, ErrorResources.DatabaseError)));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetMedicineList(It.IsAny<GetMedicinesViewModel>()) as BadRequestObjectResult;
            var response = result.Value as IEnumerable<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, response.Single().Message);
            Assert.AreEqual((int) ErrorCodes.InternalServerError, response.Single().ErrorCode);
        }

        [Test]
        public void GetSpecificMedicineDetails_CorrectData_MedicineDetails()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineDetails(It.IsAny<int>()))
                .Returns(() =>
                    new OperationResult<Medicine>(new Medicine
                    {
                        Id = 1,
                        Name = "Nazwa leku",
                        ContentQuantity = "20szt",
                        Price = 6.99M,
                        Producer = "Pharma",
                        Description = "Nowy super świetny lek!",
                        MedicineTypeName = "Tabletki"
                    }));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetSpecificMedicineDetails(It.IsAny<int>()) as OkObjectResult;
            var response = result.Value as Result<Medicine>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(1, response.Items.Id);
        }

        [Test]
        public void GetSpecificMedicineDetails_InvalidData_ModelStateError()
        {
            // Arrange
            var medicinesController = new MedicinesController(It.IsAny<IMedicinesRepository>());

            medicinesController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = medicinesController.GetSpecificMedicineDetails(It.IsAny<int>()) as BadRequestObjectResult;
            var response = result.Value as List<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorResources.ModelStateError, response.Single().Message);
        }

        [Test]
        public void GetSpecificMedicineDetails_IdDoNotExists_NotFoundError()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineDetails(It.IsAny<int>()))
                .Returns(() =>
                    new OperationResult<Medicine>(
                        new OperationError((int)ErrorCodes.NotFound, ErrorResources.ItemNotFound)));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetSpecificMedicineDetails(It.IsAny<int>()) as BadRequestObjectResult;
            var response = result.Value as IEnumerable<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
            Assert.AreEqual(ErrorResources.ItemNotFound, response.Single().Message);
            Assert.AreEqual((int)ErrorCodes.NotFound, response.Single().ErrorCode);
        }

        [Test]
        public void AddMedicine_CorrectData_AddedNewMedicine()
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

            mock.Setup(x => x.CreateNewMedicine(model)).Returns(() =>
                new OperationResult<string>(ResultResources.CreatingMedicineComplete));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.AddMedicine(model) as OkObjectResult;
            var response = result.Value as Result<string>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(ResultResources.CreatingMedicineComplete, response.Items);
        }

        [Test]
        public void AddMedicine_InvalidData_ModelStateError()
        {
            // Arrange
            var medicinesController = new MedicinesController(It.IsAny<IMedicinesRepository>());

            medicinesController.ModelState.AddModelError(string.Empty, string.Empty);

            // Act
            var result = medicinesController.AddMedicine(It.IsAny<MedicineInputModel>()) as BadRequestObjectResult;
            var response = result.Value as List<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorResources.ModelStateError, response.Single().Message);
        }

        [Test]
        public void AddMedicine_NullInputModel_ModelStateError()
        {
            // Arrange
            var medicinesController = new MedicinesController(It.IsAny<IMedicinesRepository>());

            // Act
            var result = medicinesController.AddMedicine(It.IsAny<MedicineInputModel>()) as BadRequestObjectResult;
            var response = result.Value as List<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(ErrorResources.ModelStateError, response.Single().Message);
        }

        [Test]
        public void AddMedicine_InvalidData_ManyValidationErrors()
        {
            // Arrange
            var model = new MedicineInputModel
            {
                Name = "",
                ContentQuantity = "1",
                Price = 23.49M,
                Description = "     ",
                Producer = "      P        ",
                MedicineTypeId = 0
            };

            mock.Setup(x => x.CreateNewMedicine(model))
                .Returns(() =>
                    new OperationResult<string>(new List<OperationError>
                    {
                        new OperationError((int) ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty,
                            nameof(model.Name)),
                        new OperationError((int) ErrorCodes.BadRequest,
                            string.Format(ErrorResources.InvalidFieldLength, 2, 10), nameof(model.ContentQuantity)),
                        new OperationError((int) ErrorCodes.BadRequest, ErrorResources.FieldCannotBeEmpty,
                            nameof(model.Description))
                    }));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.AddMedicine(model) as BadRequestObjectResult;
            var response = result.Value as IEnumerable<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Count());
            Assert.AreEqual((int)ErrorCodes.BadRequest, response.First().ErrorCode);
        }

        [Test]
        public void GetMedicineTypes_CorrectData_ListOfMedicineTypes()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineTypes())
                .Returns(() =>
                    new OperationResult<IEnumerable<MedicineType>>(new List<MedicineType>
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
                    }));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetMedicineTypes() as OkObjectResult;
            var response = result.Value as Result<IEnumerable<MedicineType>>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual(2, response.TotalCount);
            Assert.AreEqual(2, response.Items.Count());
        }

        [Test]
        public void GetMedicineTypes_DatabaseConnectionProblem_DatabaseError()
        {
            // Arrange
            mock.Setup(x => x.GetMedicineTypes())
                .Returns(() =>
                    new OperationResult<IEnumerable<MedicineType>>(
                        new OperationError((int) ErrorCodes.InternalServerError, ErrorResources.DatabaseError)));

            var medicinesController = new MedicinesController(mock.Object);

            // Act
            var result = medicinesController.GetMedicineTypes() as BadRequestObjectResult;
            var response = result.Value as IEnumerable<OperationError>;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count());
            Assert.AreEqual(ErrorResources.DatabaseError, response.Single().Message);
            Assert.AreEqual((int)ErrorCodes.InternalServerError, response.Single().ErrorCode);
        }
    }
}
