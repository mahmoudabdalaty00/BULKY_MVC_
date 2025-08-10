using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private  readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll()
                .OrderBy(x => x.ListPrice)
                .ToList();
            return View(products);
        }




        #region   Create Product
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product pro)
        {
            if (!ModelState.IsValid)
            {
                return View(pro);
            }

            if(string.IsNullOrEmpty(pro.Name))
            {
                ModelState.AddModelError("Name", "Product name cannot be empty.");
                return View(pro);
            }

            if(pro.ListPrice <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0.");
                return View(pro);
            }
            if (int.TryParse(pro.Name, out _))
            {
                ModelState.AddModelError("Name", "Product name cannot be only numbers.");
                return View(pro);
            }


            try
            {
                _unitOfWork.Product.Add(pro);
                _unitOfWork.Save();
                TempData["create"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving the product: {ex.Message}");
                return View(pro);
            }
        }
        #endregion


        #region Edit Product
        public IActionResult Edit(int? id)
        {
            if(id ==null || id <= 0)
            {
                return View();
            }

            var product = _unitOfWork.Product.Get( p => p.Id == id);
            if (product == null)
            {
                ModelState.AddModelError("product", "Product must be Existed");
                return View(product);
            }
            return View(product);
        }

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
        #endregion


        #region Delete Product
        public IActionResult Delete(int? id)
        {
            if(id == 0 || id == null)
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
