using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        public ProductDetailModel(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace( productId))
            {
                return NotFound();
            }

            Product = await _catalogService.GetCatalog(productId);
            if (Product == null)
            {
                return NotFound();
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
            //            Quantity = Quantity,
            //            Price = product.Price,
            //            ProductName = product.Name,
            //            Color = Color,
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