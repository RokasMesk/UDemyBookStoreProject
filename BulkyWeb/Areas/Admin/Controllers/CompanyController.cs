using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CompanyController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork=unitOfWork;

        }
        public IActionResult Index()
        {
            var objCompanysList = _unitOfWork.Company.GetAll(); 
            return View(objCompanysList);
        }
        public IActionResult Upsert(int? id)
        {
           
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(companyObj);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
           
            if (ModelState.IsValid)
            {
               
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }
             
                _unitOfWork.Save();
                TempData["success"] = "Company created succesfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            } ;

          
            
            
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var objCompanysList = _unitOfWork.Company.GetAll();
            return Json(new { data = objCompanysList });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
           if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
