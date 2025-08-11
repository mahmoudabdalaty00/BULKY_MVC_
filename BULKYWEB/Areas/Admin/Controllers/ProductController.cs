using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var products = _unitOfWork.Product.GetAll(includeProperties:"Category")
                .OrderBy(x => x.ListPrice)
                .ToList();


            return View(products);
        }


        #region   Create Product
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
        public async Task<IActionResult> UpSert(ProductVM pro, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.Length > 0)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, "img", "products");


                        if (!Directory.Exists(productPath))
                        {
                            Directory.CreateDirectory(productPath);
                        }

                        //check in update if image exist
                        if (!string.IsNullOrEmpty(pro.product.ImageUrl))
                        {
                            //delete old image
                            var oldIMagePath = Path.Combine(wwwRootPath, pro.product.ImageUrl
                                    .TrimStart('\\'));

                            if (System.IO.File.Exists(oldIMagePath))
                            {
                                System.IO.File.Delete(oldIMagePath);
                            }

                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        pro.product.ImageUrl = $"/img/products/{fileName}";
                    }


                    if (pro.product.Id == 0)
                    {
                        _unitOfWork.Product.Add(pro.product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(pro.product);
                    }


                    _unitOfWork.Save();

                    TempData["create"] = "Product created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while saving the product: {ex.Message}");
                }
            }

            pro.listItems = _unitOfWork.Category.GetAll()
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                });

            return View(pro);
        }
        #endregion




        [HttpPost]
        public IActionResult Edit(Product pro)
        {
            if (!ModelState.IsValid)
            {
                return View(pro);
            }

            try
            {
                // Fetch the tracked entity
                var existingProduct = _unitOfWork.Product.Get(p => p.Id == pro.Id);
                if (existingProduct == null)
                {
                    ModelState.AddModelError("product", "Product not found.");
                    return View(pro);
                }

                // Update fields
                existingProduct.Name = pro.Name;
                existingProduct.ListPrice = pro.ListPrice;
                existingProduct.Description = pro.Description;
                existingProduct.ISBN = pro.ISBN;
                existingProduct.price50 = pro.price50;
                existingProduct.price100 = pro.price100;
                existingProduct.price = pro.price;
                existingProduct.Author = pro.Author;


                // etc.

                _unitOfWork.Save();
                TempData["update"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving the product: {ex.Message}");
                return View(pro);
            }
        }


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
