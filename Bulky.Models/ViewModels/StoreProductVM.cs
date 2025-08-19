namespace Bulky.Models.ViewModels
{
    // Helper class for store-product specific data
    public class StoreProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public decimal? StoreSpecificPrice { get; set; }
        public bool IsFeatured { get; set; }


        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }

}
