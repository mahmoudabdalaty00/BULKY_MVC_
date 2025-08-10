using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Category category { get; set; }
        public EditModel(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public void OnGet(int? id)
        {
            if (id != null && id > 0)
            {
                category = _context.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (category.DisplayOrder == 0)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order can not be 0");
                return Page();
            }

            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "A category can not be Number.");
                return Page();
            }
            var categories = _context.Categories;
            if (categories.Any(c => c.Name == category.Name) && categories.Any(c => c.DisplayOrder == category.DisplayOrder))
            {
                ModelState.AddModelError("Name", "A category with this name already exists.");
                return Page();
            }
            try
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["update"] = "category Updated Successfully";
                return RedirectToPage("/Categories/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "An error occurred while saving the category.");
                return Page();
            }
        }
    }
}

