using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var Companys = _unitOfWork.Company.GetAll()
                .OrderBy(c => c.DisplayOrder)
                .ToList();


            return View(Companys);
        }

        #region   Create||Update Company
        public IActionResult UpSert(int? id)
        {


            if (id == null || id == 0)
            {      //create
                return View(new Company());
            }
            else
            {      //update
                Company companyDb = _unitOfWork.Company.Get(p => p.Id == id);
                if (companyDb == null)
                {
                    ModelState.AddModelError("Company", "Company must be Existed");
                    return View(companyDb);
                }
                return View(companyDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company pro, IFormFile? file)
        {
            try
            {

                if (pro.Id == 0)
                {
                    pro.CreatedAt = DateTime.UtcNow;
                    pro.CreatedBy = User.Identity?.Name ?? "System";
                    pro.UpdatedAt = DateTime.Now;
                    pro.UpdatedBy = User.Identity?.Name ?? "System";
                    _unitOfWork.Company.Add(pro);
                }
                else
                {
                    pro.UpdatedAt = DateTime.Now;
                    pro.UpdatedBy = User.Identity?.Name ?? "System";
                    _unitOfWork.Company.Update(pro);
                }


                _unitOfWork.Save();
                TempData["success"] = "Company saved successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }


            return View(pro);
        }
        #endregion

        #region Delete Company
        //public IActionResult Delete(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return NotFound();
        //    }
        //    var Company =
        //            _unitOfWork.Company.Get(u => u.Id == id);



        //    if (Company == null)
        //    {
        //        throw new Exception($"Cannot Find Category With this Id :{id}");
        //    }
        //    return View(Company);
        //}


        //[HttpPost]
        //public IActionResult Delete(Company pro)
        //{
        //    try
        //    {
        //        var prod = _unitOfWork.Company
        //                .Get(p => p.Id == pro.Id);

        //        if (prod == null)
        //        {
        //            return NotFound();
        //        }

        //        _unitOfWork.Company.Remove(prod);
        //        _unitOfWork.Save();
        //        TempData["delete"] = "Category Deleted Successfully";
        //        return RedirectToAction("Index");


        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", $"An error occurred while deleting the category: {ex.Message}");
        //        return View(pro);
        //    }
        //}

        #endregion


        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.Company
                .GetAll()
                .OrderBy(c => c.DisplayOrder)
                .ToList();

            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var company = _unitOfWork.Company
                    .Get(p => p.Id == id);
            if (company == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error While Deleting"
                });
            }


            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Success To Deleting"
            });
        }

        #endregion

    }
}
