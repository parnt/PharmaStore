using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PharmaStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        /// <summary>
        /// Get medicine list
        /// </summary>
        /// <returns>Medicine list</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get specific medicine details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Medicine details</returns>
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Add new medicine to DB
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
