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
    using PharmaStoreWeb.Models.Core;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
