
using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BULKYWEB.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
         

        //challenge that i check name & display to see if they both exist or not in edit ?

        #region  Create Category
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category categ )
        {
            if(!ModelState.IsValid)
            {
                return View(categ);
            }

            if(String.IsNullOrEmpty(categ.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categ);
            }

            if(categ.DisplayOrder == 0)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order can not be 0");
                return View(categ);
            }

            if (categ.Name == categ.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "A Category can not be Number.");
                return View(categ);
            }
            var categories = _context.Categories;
            if(categories.Any( c=> c.Name == categ.Name ))
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(categ);
            }

            try
            {
                _context.Categories.Add(categ);
                _context.SaveChanges();
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
           if(id ==0 || id == null)
            {
                return NotFound();
            }

           var category = _context.Categories
                    .FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                throw new Exception($"Cannot Find Category With this Id :{id}");
            }

             return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category categ)
        {
            if (!ModelState.IsValid)
            {
                return View(categ);
            }

            if (String.IsNullOrEmpty(categ.Name))
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
            var categories = _context.Categories;
            if (categories.Any(c => c.Name == categ.Name) && categories.Any(c => c.DisplayOrder == categ.DisplayOrder)) 
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return View(categ);
            }
            try
            {
                _context.Categories.Update(categ);
                _context.SaveChanges();
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

            var category = _context.Categories
                     .FirstOrDefault(c => c.Id == id);

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
                var categoryFromDb = _context.Categories.Find(categ.Id);
                if (categoryFromDb == null)
                {
                    return NotFound();
                }
                _context.Categories.Remove(categoryFromDb);
                _context.SaveChanges();
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
