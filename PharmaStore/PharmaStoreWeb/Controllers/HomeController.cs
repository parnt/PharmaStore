using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using PharmaStoreWeb.Models;

namespace PharmaStoreWeb.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Models.Medicines;
    using Newtonsoft.Json;
    using PharmaStoreWeb.Models.Core;
    using System.Text;

    public class HomeController : Controller
    {
        private const string url = "http://localhost:5001/api/Medicines";

        public async Task<IActionResult> Index()
        {
            string urlBuild = url;

            try
            {
                HttpResponseMessage response = await new HttpClient().GetAsync(urlBuild);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = await response.Content.ReadAsAsync<Result<IEnumerable<MedicineHeader>>>();
                    var result = new OperationResult<IEnumerable<MedicineHeader>>(model);

                    return View(result);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var model = await response.Content.ReadAsAsync<IEnumerable<Error>>();
                    var result = new OperationResult<IEnumerable<MedicineHeader>>(model);

                    return View(result);
                }

                return Error();
            }
            catch
            {
                return Error();
            }
        }

        public async Task<IActionResult> GetMedicine(int id)
        {
            string urlBuild = $"{url}/{id}";

            try
            {
                HttpResponseMessage response = await new HttpClient().GetAsync(urlBuild);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = await response.Content.ReadAsAsync<Result<Medicine>>();
                    var result = new OperationResult<Medicine>(model);

                    return View(result);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var model = await response.Content.ReadAsAsync<IEnumerable<Error>>();
                    var result = new OperationResult<Medicine>(model);

                    return View(result);
                }

                return Error();
            }
            catch
            {
                return Error();
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddMedicine()
        {
            string urlBuild = $"{url}/MedicineTypes";

            try
            {
                HttpResponseMessage response = await new HttpClient().GetAsync(urlBuild);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = await response.Content.ReadAsAsync<Result<IEnumerable<MedicineType>>>();
                    var result = new OperationResult<IEnumerable<MedicineType>>(model);

                    return View(result);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var model = await response.Content.ReadAsAsync<IEnumerable<Error>>();
                    var result = new OperationResult<IEnumerable<MedicineType>>(model);

                    return View(result);
                }

                return Error();
            }
            catch
            {
                return Error();
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddMedicine(MedicineInputModel medicine)
        {
            string urlBuild = url;

            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(medicine), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await new HttpClient().PostAsync(urlBuild, stringContent);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = await response.Content.ReadAsAsync<Result<string>>();
                    var result = new OperationResult<string>(model);

                    return Json(new {type = "OK"});

                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var model = await response.Content.ReadAsAsync<IEnumerable<Error>>();
                    var result = new OperationResult<string>(model);

                    if (result.Errors.All(x => !string.IsNullOrEmpty(x.FieldName)))
                    {
                        return Json(new { type = "Validation", data = result.Errors });
                    }
                }

                return Json(new { type = "Error" });
            }
            catch
            {
                return Json(new { type = "Error" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
