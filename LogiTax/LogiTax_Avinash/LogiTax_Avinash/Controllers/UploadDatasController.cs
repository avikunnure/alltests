using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LogiTax_Avinash.Models;
using LogiTax_Avinash.Services;

namespace LogiTax_Avinash.Controllers
{
    public class UploadDatasController : Controller
    {
        private UploadDataService service = new UploadDataService();

        // GET: UploadDatas
        public ActionResult Index()
        {
            
            return View(service.GetList());
        }

      
        
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public ActionResult Create(UploadData uploadDataModel)
        {
            if (ModelState.IsValid)
            {

                service.Save(uploadDataModel,"insert");
                ViewBag.ErrorMessages = service.Errors;
                return RedirectToAction("Index");
            }

            return View(uploadDataModel);
        }
        public ActionResult Edit(Guid id)
        {
           
            UploadData uploadData = service.Get(id);
         
            return View(uploadData);
        }

     
        [HttpPost]
        public ActionResult Edit(UploadData uploadDataModel)
        {
            if (ModelState.IsValid)
            {
                service.Save(uploadDataModel, "update");
                ViewBag.ErrorMessages = service.Errors;
                return RedirectToAction("Index");
            }
            return View(uploadDataModel);
        }
        public ActionResult Delete(Guid id)
        {
           
            UploadData uploadData = service.Get(id);
            service.Save(uploadData, "delete");
            ViewBag.ErrorMessages = service.Errors;
            return RedirectToAction("Index");
        }
    }
}
