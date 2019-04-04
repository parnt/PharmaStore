namespace PharmaStoreAPI.Controllers
{
    using Core.Repositories.Contracts;
    using Core.ViewModels.Medicines;
    using Microsoft.AspNetCore.Mvc;
    using PharmaStoreAPI.Helpers;

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
                if (!string.IsNullOrEmpty(filters.SearchValue))
                    filters.SearchValue = filters.SearchValue.Trim();

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
        /// Add new medicine to DB
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void AddMedicine([FromBody] string value)
        {
        }
    }
}
