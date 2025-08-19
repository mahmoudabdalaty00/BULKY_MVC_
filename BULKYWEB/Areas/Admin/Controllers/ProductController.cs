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
                .OrderBy(x => x.DisplayOrder)
                .ToList();

            return View(products);
        }

        #region Create||Update Product
        public IActionResult UpSert(int? id)
        {
         
            
            
            ProductVM vm = new ProductVM()
            {
                listItems = GetCategorySelectList(),
                product = new Product(),
                StoreList = GetStoreSelectList().ToList(),
                SelectedStoreIds = new List<int>()   ,
                
            };




            if (id == null || id == 0)
            {
                // Creating new product
                return View(vm);
            }


            else
            {
                // Editing existing product
                vm.product = _unitOfWork.Product
                    .Get(p => p.Id == id, includeProperties: "ProductImages,StoreProducts");

                if (vm.product == null)
                {
                    ModelState.AddModelError("product", "Product not found");
                    return View(vm);
                }

                // Initialize collections if null
                if (vm.product.ProductImages == null)
                {
                    vm.product.ProductImages = new List<ProductImage>();
                }

                if (string.IsNullOrEmpty(vm.product.Description))
                {
                    vm.product.Description = "Default description";
                }

                // Populate selected store IDs (simplified - no additional details)
                if (vm.product.StoreProducts != null && vm.product.StoreProducts.Any())
                {
                    vm.SelectedStoreIds = vm.product.StoreProducts
                        .Select(sp => sp.StoreId)
                        .ToList();
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
                pro.product.Description = "Default description";
            }

            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                
                var ApplicationUser = _unitOfWork.ApplicationUser
                                .Get(a => a.Id == userId);

                if (ApplicationUser == null)
                {
                    ModelState.AddModelError("", "User not found");
                    pro.listItems = GetCategorySelectList();
                    pro.StoreList = GetStoreSelectList().ToList();
                    return View(pro);
                }

                try
                {
                    if (pro.product.Id == 0)
                    {
                        // Creating new product
                        pro.product.CreatedBy = ApplicationUser.Name;
                        pro.product.UpdatedBy = ApplicationUser.Name;
                        pro.product.CreatedAt = DateTime.UtcNow;
                        pro.product.UpdatedAt = DateTime.UtcNow;

                         
                        _unitOfWork.Product.Add(pro.product);
                        _unitOfWork.Save(); // Save to get the product ID
                    }
                    else
                    {
                        // Updating existing product
                        pro.product.UpdatedBy = ApplicationUser.Name;
                        pro.product.UpdatedAt = DateTime.UtcNow;

                        _unitOfWork.Product.Update(pro.product);
                        _unitOfWork.Save();
                    }

                    // Handle store-product relationships (simplified)
                    ManageProductStores(pro.product.Id, pro.SelectedStoreIds);

                    // Handle file uploads
                    HandleFileUploads(pro, files);

                    TempData["success"] = "Product saved successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            // Reload data if validation fails
            pro.listItems = GetCategorySelectList();
            pro.StoreList = GetStoreSelectList().ToList();
            return View(pro);
        }

        /// <summary>
        /// Simplified store management - only handles which stores the product is available in
        /// </summary>
        private void ManageProductStores(int productId, List<int> selectedStoreIds)
        {
            // Ensure we have a list (even if empty)
            if (selectedStoreIds == null)
                selectedStoreIds = new List<int>();

            // Get existing store-product relationships
            var existingStoreProducts = _unitOfWork.StoreProduct
                .GetAll(sp => sp.ProductId == productId)
                .ToList();

            var existingStoreIds = existingStoreProducts
                            .Select(esp => esp.StoreId).ToList();

            // Remove stores that are no longer selected
            var storesToRemove = existingStoreProducts
                .Where(esp => !selectedStoreIds.Contains(esp.StoreId))
                .ToList();

            _unitOfWork.StoreProduct.RemoveRange(storesToRemove);
            //foreach (var storeToRemove in storesToRemove)
            //{
            //    _unitOfWork.StoreProduct.Remove(storeToRemove);
            //}

            // Add new store relationships
            var newStoreIds = selectedStoreIds
                .Where(ssId => !existingStoreIds.Contains(ssId))
                .ToList();

            foreach (var newStoreId in newStoreIds)
            {
                var storeProduct = new StoreProduct
                {
                    StoreId = newStoreId,
                    ProductId = productId,
                    StockQuantity = 0, // Default value
                    DateAdded = DateTime.UtcNow,
                    IsFeatured = false, // Default value
                    StoreSpecificPrice = null // Use product's main price
                };
                _unitOfWork.StoreProduct.Add(storeProduct);
            }

            _unitOfWork.Save();
        }

        private void HandleFileUploads(ProductVM pro, List<IFormFile> files)
        {
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
                        ImageUrl = Path.Combine(productPath, fileName).Replace("\\", "/"),
                        ProductId = pro.product.Id,
                    };

                    if (pro.product.ProductImages == null)
                        pro.product.ProductImages = new List<ProductImage>();

                    pro.product.ProductImages.Add(productImage);
                }

                _unitOfWork.Product.Update(pro.product);
                _unitOfWork.Save();
            }
        }

        private IEnumerable<SelectListItem> GetStoreSelectList()
        {
            return _unitOfWork.Store.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
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

        #region Delete Product Image
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

            return RedirectToAction(nameof(UpSert), new { id = productId });
        }
        #endregion

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category")
              .OrderBy(x => x.DisplayOrder)
              .ToList();

            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var prod = _unitOfWork.Product.Get(p => p.Id == id);
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
    }
}