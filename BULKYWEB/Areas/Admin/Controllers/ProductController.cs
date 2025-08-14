using Blky.Utility;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

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
            {
                return View(vm);
            }
            else
            {
                vm.product = _unitOfWork.Product.Get(p => p.Id == id, includeProperties: "ProductImages");
                if (vm.product == null)
                {
                    ModelState.AddModelError("product", "Product not found");
                    vm.listItems = category; // Ensure listItems is set
                    return View(vm); // Return vm, not vm.product
                                     // Or: return NotFound();
                }
                if (vm.product.ProductImages == null)
                {
                    vm.product.ProductImages = new List<ProductImage>(); // Initialize if null
                }

                if (string.IsNullOrEmpty(vm.product.Description))
                {
                    vm.product.Description = "Default description"; // Use a meaningful default
                }
                return View(vm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM pro, List<IFormFile> files)
        {
            if (string.IsNullOrEmpty(pro.product.Description))
            {
                pro.product.Description = "Default description"; // Use a meaningful default
            }
            
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var ApplicationUser = _unitOfWork.ApplicationUser.Get(a => a.Id == userId);
                if (ApplicationUser == null)
                {
                    ModelState.AddModelError("", "User not found");
                    pro.listItems = GetCategorySelectList();
                    return View(pro);
                }

                try
                {
                    if (pro.product.Id == 0)
                    {
                        pro.product.CreatedBy = ApplicationUser.Name;
                        pro.product.UpdatedBy = ApplicationUser.Name;
                        pro.product.CreatedAt = DateTime.UtcNow;
                        pro.product.UpdatedAt = DateTime.UtcNow;

                        if(pro.product.Description == null)
                        {
                            pro.product.Description = "sss";
                        }
                        _unitOfWork.Product.Add(pro.product);
                    }
                    else
                    {
                        pro.product.UpdatedBy = ApplicationUser.Name;
                        pro.product.UpdatedAt = DateTime.UtcNow;
                        _unitOfWork.Product.Update(pro.product);
                    }

                    _unitOfWork.Save();

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (files != null && files.Any())
                    {
                        foreach (IFormFile file in files)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string productPath = Path.Combine("img", "products", "product-" + pro.product.Id);
                            string finalPath = Path.Combine(wwwRootPath, productPath);

                            if (!Directory.Exists(finalPath))
                                Directory.CreateDirectory(finalPath);

                            using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            ProductImage productImage = new()
                            {
                                ImageUrl = Path.Combine(productPath, fileName).Replace("\\", "/"), // Normalize path
                                ProductId = pro.product.Id,
                            };

                            if (pro.product.ProductImages == null)
                                pro.product.ProductImages = new List<ProductImage>();

                            pro.product.ProductImages.Add(productImage);
                        }

                        _unitOfWork.Product.Update(pro.product);
                        _unitOfWork.Save();

                        TempData["success"] = "Product saved successfully";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["success"] = "Product saved successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            pro.listItems = GetCategorySelectList();
            return View(pro);
        }
        #endregion

        #region Image Methods 
        //private string UploadImage(IFormFile file, string? oldImageUrl)
        //{
        //    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        //    var ext = Path.GetExtension(file.FileName).ToLower();

        //    if (!allowedExtensions.Contains(ext))
        //        throw new InvalidOperationException("Invalid file type");

        //    string wwwRootPath = _webHostEnvironment.WebRootPath;
        //    string fileName = Guid.NewGuid() + ext;
        //    string uploadPath = Path.Combine(wwwRootPath, @"img\products");

        //    // Ensure folder exists
        //    if (!Directory.Exists(uploadPath))
        //        Directory.CreateDirectory(uploadPath);

        //    // Delete old image if updating
        //    DeleteImage(oldImageUrl);

        //    using (var fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
        //    {
        //        file.CopyTo(fs);
        //    }

        //    return @"\img\products\" + fileName;
        //}

        //private void DeleteImage(string? imageUrl)
        //{
        //    if (string.IsNullOrEmpty(imageUrl)) return;

        //    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
        //    if (System.IO.File.Exists(fullPath))
        //        System.IO.File.Delete(fullPath);
        //}

        private IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
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
        public IActionResult Delete(int? id)
        {
            var prod = _unitOfWork.Product
                    .Get(p => p.Id == id);
            if (prod == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Error While Deleting"
                });
            }

            DeleteProductImagesWithProduct(id);
            _unitOfWork.Product.Remove(prod);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "Success To Deleting"
            });
        }

        private void DeleteProductImagesWithProduct(int? id)
        {
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }
        }

        #endregion


        #region Delete Product
        public IActionResult Deletes(int? id)
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

        public IActionResult DeleteImage(int id)
        {
            var DeleteImage = _unitOfWork.ProductImage
                        .Get(u => u.Id == id);
            int productId = DeleteImage.ProductId;
            if (DeleteImage != null)
            {
                if (!string.IsNullOrEmpty(DeleteImage.ImageUrl))
                {
                    var oldImagePath = Path.Combine(
                            _webHostEnvironment.WebRootPath, DeleteImage.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(DeleteImage);
                _unitOfWork.Save();

                TempData["success"] = "Deleted Successfully";

            }

            return RedirectToAction(nameof(UpSert), new
            {
                id = productId,
            });

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
    }
}
