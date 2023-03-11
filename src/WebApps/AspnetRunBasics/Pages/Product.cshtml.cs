using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        public ProductModel(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string CategoryName)
        {
            var products = await _catalogService.GetCatalog();
            CategoryList =  products.Select(p=>p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                ProductList = await _catalogService.GetCatalogByCategory(CategoryName);
                SelectedCategory = CategoryName;
            }
            else
            {
                ProductList = await _catalogService.GetCatalog();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            string userName = "coresoft";
            //1:get product details
            var product = await _catalogService.GetCatalog(productId);

            //var basket = await _basketService.GetBasket(userName);
            //basket.Items.Add(
            //        new()
            //        {
            //            ProductId = productId,
            //            Quantity = 1,
            //            Price = product.Price,
            //            ProductName = product.Name,
            //            Color = "black",
            //        }
            //    );
            BasketItemModel model = new()
            {
                ProductId = productId,
                ProductCode = productId,
                Quantity = 1,
                Price = product.Price,
                ProductName = product.Name,
                Color = "black",
            };
            await _basketService.UpdateBasket(userName, model);
            return RedirectToPage("Cart");
        }
    }
}