namespace PharmaStoreAPI.Controllers
{
    using Core.Repositories.Contracts;
    using Core.ViewModels.Medicines;
    using Microsoft.AspNetCore.Mvc;
    using PharmaStoreAPI.Helpers;
    using System.ComponentModel.DataAnnotations;

    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicinesRepository _medicinesRepository;

        public MedicinesController(IMedicinesRepository medicinesRepository)
        {
            _medicinesRepository = medicinesRepository;
        }

        /// <summary>
        /// Get medicine list
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>Medicine list</returns>
        [HttpGet]
        public IActionResult GetMedicineList([FromQuery] GetMedicinesViewModel filters)
        {
            if (ModelState.IsValid)
            {
                if(filters != null)
                    filters.SearchValue = filters.SearchValue.TrimString();

                var result = _medicinesRepository.GetMedicineList(filters);

                if (result.IsSuccess)
                {
                    return Ok(result.Result);
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(GlobalHelpers.ModelStateError());
        }

        /// <summary>
        /// Get specific medicine details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Medicine details</returns>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetSpecificMedicineDetails(int id)
        {
            if (ModelState.IsValid)
            {
                var result = _medicinesRepository.GetMedicineDetails(id);

                if (result.IsSuccess)
                {
                    return Ok(result.Result);
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(GlobalHelpers.ModelStateError());
        }

        /// <summary>
        /// Create new Medicine
        /// </summary>
        /// <param name="newMedicine"></param>
        /// <returns>Information about complete operation</returns>
        [HttpPost]
        public IActionResult AddMedicine([FromBody][Required] MedicineInputModel newMedicine)
        {
            if (ModelState.IsValid && newMedicine != null)
            {
                newMedicine = newMedicine.TrimObj();

                var result = _medicinesRepository.CreateNewMedicine(newMedicine);

                if (result.IsSuccess)
                {
                    return Ok(result.Result);
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(GlobalHelpers.ModelStateError());
        }

        [HttpGet("MedicineTypes")]
        public IActionResult GetMedicineTypes()
        {
            var result = _medicinesRepository.GetMedicineTypes();

            if (result.IsSuccess)
            {
                return Ok(result.Result);
            }

            return BadRequest(result.Errors);
        }
    }
}
