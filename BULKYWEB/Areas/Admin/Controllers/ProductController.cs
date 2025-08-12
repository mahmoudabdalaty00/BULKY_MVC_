using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category")
                .OrderBy(x => x.ListPrice)
                .ToList();


            return View(products);
        }

        #region   Create||Update Product
        public IActionResult UpSert(int? id)
        {
            IEnumerable<SelectListItem> category = _unitOfWork.Category.GetAll().Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.Id.ToString(),
            });

            ProductVM vm = new ProductVM()
            {
                listItems = category,
                product = new Product()
            };

            if (id == null || id == 0)
            {      //create
                return View(vm);
            }
            else
            {      //update
                vm.product = _unitOfWork.Product.Get(p => p.Id == id);
                if (vm.product == null)
                {
                    ModelState.AddModelError("product", "Product must be Existed");
                    return View(vm.product);
                }
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM pro, IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    // Upload new image and delete old one if exists
                    string newImagePath = UploadImage(file, pro.product.ImageUrl);
                    pro.product.ImageUrl = newImagePath;
                }

                if (pro.product.Id == 0)
                    _unitOfWork.Product.Add(pro.product);
                else
                    _unitOfWork.Product.Update(pro.product);

                _unitOfWork.Save();
                TempData["success"] = "Product saved successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }

            pro.listItems = GetCategorySelectList();
            return View(pro);
        }
        #endregion

        #region Image Methods 
        private string UploadImage(IFormFile file, string? oldImageUrl)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Invalid file type");

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid() + ext;
            string uploadPath = Path.Combine(wwwRootPath, @"img\products");

            // Ensure folder exists
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Delete old image if updating
            DeleteImage(oldImageUrl);

            using (var fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return @"\img\products\" + fileName;
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        #endregion

        #region Delete Product
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var product =
                    _unitOfWork.Product.Get(u => u.Id == id);



            if (product == null)
            {
                throw new Exception($"Cannot Find Category With this Id :{id}");
            }
            return View(product);
        }

        private void DeletedImage(Product product)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
        }

        [HttpPost]
        public IActionResult Delete(Product pro)
        {
            try
            {
                var prod = _unitOfWork.Product
                        .Get(p => p.Id == pro.Id);

                if (prod == null)
                {
                    return NotFound();
                }
                DeletedImage(pro);
                _unitOfWork.Product.Remove(prod);
                _unitOfWork.Save();
                TempData["delete"] = "Category Deleted Successfully";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while deleting the category: {ex.Message}");
                return View(pro);
            }
        }

        #endregion
           
        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category")
              .OrderBy(x => x.ListPrice)
              .ToList();

            return Json(new
            {
                data = products
            });
        }

        [HttpDelete]
        public IActionResult Deletes(int? id)
        {
            var prod = _unitOfWork.Product
                    .Get(p => p.Id == id);
             if(prod == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error While Deleting"
                });
            }

            DeletedImage(prod);
            _unitOfWork.Product.Remove(prod);
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
