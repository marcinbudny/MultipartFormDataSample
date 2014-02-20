using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultipartFormDataSample.Server.Models;
using Newtonsoft.Json;

namespace MultipartFormDataSample.Server.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Send(string name, IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.Name = name;
            ViewBag.PostedFiles = files.Select(pf => pf.FileName).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SendWithJson(string model, IEnumerable<HttpPostedFileBase> files)
        {
            var deserializedModel = JsonConvert.DeserializeObject<SampleModel>(model);

            ViewBag.Name = deserializedModel.ModelName;
            ViewBag.PostedFiles = files.Select(pf => pf.FileName).ToList();
            return View("Send");
        }
    }
}