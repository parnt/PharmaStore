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
        /// <returns>Medicine list</returns>
        [HttpGet]
        public IActionResult GetMedicineList([FromForm] GetMedicinesViewModel filters)
        {
            if (ModelState.IsValid)
            {
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
        public string GetSpecificMedicineDetails(int id)
        {
            return "value";
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
