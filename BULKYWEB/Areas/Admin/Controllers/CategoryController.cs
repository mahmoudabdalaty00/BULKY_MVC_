
using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll()
                .Where(a => a.IsHidden == false)
                .ToList();
            
            return View(categories);
        }


        //challenge that i check name & display to see if they both exist or not in edit ?

        #region  Create Category
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category categ)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ApplicationUser = _unitOfWork.ApplicationUser
                               .Get(a => a.Id == userId);

            if (!ModelState.IsValid)
            {
                return View(categ);
            }

            if (string.IsNullOrEmpty(categ.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categ);
            }

            if (categ.DisplayOrder == 0)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order can not be 0");
                return View(categ);
            }

            if (categ.Name == categ.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "A Category can not be Number.");
                return View(categ);
            }
            var categories = _unitOfWork.Category.GetAll();
            if (categories.Any(c => c.Name == categ.Name))
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(categ);
            }

            try
            {
                categ.CreatedAt = DateTime.Now;
                categ.UpdatedAt = DateTime.Now;
                categ.CreatedBy = ApplicationUser.Name;
                categ.UpdatedBy = ApplicationUser.Name;


                _unitOfWork.Category.Add(categ);
                _unitOfWork.Save();
                TempData["create"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "An error occurred while saving the category.");
                return View(categ);
            }
        }
        #endregion

        #region Update Category
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.Get(c => c.Id == id);

            if (category == null)
            {
                throw new Exception($"Cannot Find Category With this Id :{id}");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category categ)
        {


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ApplicationUser = _unitOfWork.ApplicationUser
                               .Get(a => a.Id == userId);


            if (!ModelState.IsValid)
            {
                return View(categ);
            }

            if (string.IsNullOrEmpty(categ.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categ);
            }

            if (categ.DisplayOrder == 0)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order can not be 0");
                return View(categ);
            }

            if (categ.Name == categ.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "A Category can not be Number.");
                return View(categ);
            }
           
            try
            {
                categ.UpdatedBy = ApplicationUser.Name;
                categ.UpdatedAt = DateTime.Now;

                _unitOfWork.Category.Update(categ);
                _unitOfWork.Save();
                TempData["update"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "An error occurred while saving the category.");
                return View(categ);
            }
        }
        #endregion

        #region Delete
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Category.Get(c => c.Id == id);

            if (category == null)
            {
                throw new Exception($"Cannot Find Category With this Id :{id}");
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category categ)
        {
            try
            {
                var categoryFromDb = _unitOfWork.Category.Get(c => c.Id == categ.Id);
                if (categoryFromDb == null)
                {
                    return NotFound();
                }
                _unitOfWork.Category.Remove(categoryFromDb);
                _unitOfWork.Save();
                TempData["delete"] = "Category Deleted Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the category: {ex.Message}");
                return View(categ);
            }
        }
        #endregion

    }
}
