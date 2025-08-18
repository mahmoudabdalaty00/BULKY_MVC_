using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BULKYWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreController(IUnitOfWork unitOfWork)  
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var stores = _unitOfWork.Store.GetAll().ToList();
            return View(stores);
        }


        #region UpSert 
        public IActionResult UpSert(int? id)
        {

            var storeVM = new StoreVM
            {
                Store = new Store(),
                ProductList = _unitOfWork.Product.GetAll().Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {

                return View(storeVM);
            }
            else
            {

                var storeDb = _unitOfWork.Store.Get(
                    s => s.Id == id,
                    includeProperties: "StoreProducts,StoreProducts.Product"
                );

                if (storeDb == null)
                {
                    ModelState.AddModelError("Store", "Store not found");
                    return View(storeVM);
                }


                storeVM.Store = storeDb;


                storeVM.SelectedProductIds = storeDb.StoreProducts.Select(sp => sp.ProductId).ToList();


                storeVM.StoreProducts = storeDb.StoreProducts.Select(sp => new StoreProductVM
                {
                    ProductId = sp.ProductId,
                    ProductName = sp.Product.Name,
                    StockQuantity = sp.StockQuantity,
                    StoreSpecificPrice = sp.StoreSpecificPrice,
                    IsFeatured = sp.IsFeatured
                }).ToList();

                return View(storeVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(StoreVM storeVM)
        {
            if (ModelState.IsValid)
            {
                bool isNewStore = storeVM.Store.Id == 0;

                if (isNewStore)
                {
                    storeVM.Store.CreatedAt = DateTime.UtcNow;
                    _unitOfWork.Store.Add(storeVM.Store);
                }
                else
                {
                    storeVM.Store.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.Store.Update(storeVM.Store);
                }

                _unitOfWork.Save();


                ManageStoreProducts(storeVM);

                TempData["success"] = "Store saved successfully with products";
                return RedirectToAction(nameof(Index));
            }


            storeVM.ProductList = _unitOfWork.Product.GetAll().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            return View(storeVM);
        }

        private void ManageStoreProducts(StoreVM storeVM)
        {

            var existingStoreProducts = _unitOfWork.StoreProduct
                .GetAll(sp => sp.StoreId == storeVM.Store.Id)
                .ToList();


            var productsToRemove = existingStoreProducts
                .Where(esp => !storeVM.SelectedProductIds.Contains(esp.ProductId))
                .ToList();

            foreach (var productToRemove in productsToRemove)
            {
                _unitOfWork.StoreProduct.Remove(productToRemove);
            }


            var existingProductIds = existingStoreProducts.Select(esp => esp.ProductId).ToList();
            var newProductIds = storeVM.SelectedProductIds
                .Where(spId => !existingProductIds.Contains(spId))
                .ToList();

            foreach (var newProductId in newProductIds)
            {
                var storeProduct = new StoreProduct
                {
                    StoreId = storeVM.Store.Id,
                    ProductId = newProductId,
                    StockQuantity = 0,
                    DateAdded = DateTime.UtcNow,
                    IsFeatured = false
                };
                _unitOfWork.StoreProduct.Add(storeProduct);
            }


            if (storeVM.StoreProducts != null && storeVM.StoreProducts.Any())
            {
                foreach (var storeProductVM in storeVM.StoreProducts)
                {
                    var existingStoreProduct = existingStoreProducts
                        .FirstOrDefault(esp => esp.ProductId == storeProductVM.ProductId);

                    if (existingStoreProduct != null)
                    {
                        existingStoreProduct.StockQuantity = storeProductVM.StockQuantity;
                        existingStoreProduct.StoreSpecificPrice = storeProductVM.StoreSpecificPrice;
                        existingStoreProduct.IsFeatured = storeProductVM.IsFeatured;
                        _unitOfWork.StoreProduct.Update(existingStoreProduct);
                    }
                }
            }

            _unitOfWork.Save();
        }


        #endregion

        #region Api Calls 
        [HttpGet]
        public IActionResult GetAll()
        {
            var stores = _unitOfWork.Store
                .GetAll()
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Email,
                    s.IsActive,
                    s.City,
                    s.Country
                })
                .ToList();

            return Json(new { data = stores });
        }



        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var store = _unitOfWork.Store
                .Get(s => s.Id == id);

            if (store != null)
            {
                _unitOfWork.Store.Remove(store);
                _unitOfWork.Save();

                return Json(
                    new
                    {
                        data = store,
                        message = "Success To Deleting",
                    });
            }

            else
            {
                return Json(new
                {
                    success = false,
                    message = "Error While Deleting",
                });
            }
        }











        #endregion
    }
}