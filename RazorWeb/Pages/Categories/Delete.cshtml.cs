using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RazorWeb.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Category category { get; set; }
        public DeleteModel(ApplicationDbContext dbContext)
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
            try
            {
                var categoryFromDb = _context.Categories.Find(category.Id);
                if (categoryFromDb == null)
                {
                    return NotFound();
                }
                _context.Categories.Remove(categoryFromDb);
                _context.SaveChanges();
                TempData["delete"] = "Category Deleted Successfully";
                return RedirectToPage("/Categories/Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the category: {ex.Message}");
                return Page();
            }
        }
        }
}
