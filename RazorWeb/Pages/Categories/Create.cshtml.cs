using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWeb.Data;
using RazorWeb.Models;

namespace RazorWeb.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _DbContext;
       
        [BindProperty]
        public Category category { get; set; }
        public CreateModel(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }
      
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _DbContext.Categories.Add(category);
            _DbContext.SaveChanges();
            return RedirectToPage("/Categories/Index");
        }
    }
}
