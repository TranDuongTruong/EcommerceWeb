using System.Security.Claims;
using AppMvc.Net.Models;
using AppMvc.Net.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;



[Area("Checkout")]
[Authorize]

public class CheckoutController : Controller
{
    private readonly AppDbContext _context;

    public CheckoutController(AppDbContext context)
    {
        _context = context;
    }

    [Route("/checkoutView")]
    public async Task<IActionResult> Index()
    {
        var cartItemsJson = HttpContext.Session.GetString("CartItems");
        var shopDiscountCode = HttpContext.Session.GetString("ShopDiscountCode");
        var systemDiscountCode = HttpContext.Session.GetString("SystemDiscountCode");

        if (string.IsNullOrEmpty(cartItemsJson) || string.IsNullOrEmpty(shopDiscountCode) || string.IsNullOrEmpty(systemDiscountCode))
        {
            return RedirectToAction("Index", "Cart");
        }

        var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartItemsJson);
        Console.WriteLine("cartItemssss" + cartItemsJson.ToString());
        decimal totalPrice = 0;

        foreach (var cartItem in cartItems)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = $"Product with ID {cartItem.ProductId} does not exist" });
            }

            if (!product.CheckoutProduct(cartItem.Quantity))
            {
                return Json(new { success = false, message = $"Not enough stock for product with ID {cartItem.ProductId}" });
            }

            totalPrice += product.Prices * cartItem.Quantity;
        }
        // Xử lý mã giảm giá của shop
        if (!string.IsNullOrEmpty(shopDiscountCode))
        {
            if (shopDiscountCode != "null")
            {


                var shopDiscount = _context.DiscountModels.Where(d => d.AuthorType == DiscountAuthorType.Seller).FirstOrDefault(d => d.Code == shopDiscountCode);
                if (shopDiscount == null || !shopDiscount.IsActive)
                {
                    return Json(new { success = false, message = "Invalid or inactive shop discount code" });
                }

                totalPrice = shopDiscount.ApplyDiscount(totalPrice);
                shopDiscount.CurrentUsage++;

                // In giá cuối cùng ra console
                System.Diagnostics.Debug.WriteLine($"Total Price after shop discount: {totalPrice}");

            }
        }

        // Xử lý mã giảm giá của hệ thống
        if (!string.IsNullOrEmpty(systemDiscountCode))
        {
            if (systemDiscountCode != "null")
            {

                var systemDiscount = _context.DiscountModels.Where(d => d.AuthorType == DiscountAuthorType.System).FirstOrDefault(d => d.Code == systemDiscountCode);
                if (systemDiscount == null || !systemDiscount.IsActive)
                {
                    return Json(new { success = false, message = "Invalid or inactive system discount code" });
                }

                totalPrice = systemDiscount.ApplyDiscount(totalPrice);
                systemDiscount.CurrentUsage++;

                // In giá cuối cùng ra console
                System.Diagnostics.Debug.WriteLine($"Total Price after system discount: {totalPrice}");
            }
        }

        Console.WriteLine("           aaaaaaaaa" + cartItems.Count);
        var viewModel = new CheckoutViewModel
        {
            CartItems = cartItems,
            TotalPrice = totalPrice,
            ShopDiscountCode = shopDiscountCode,
            SystemDiscountCode = systemDiscountCode
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult ProcessPayment()
    {
        // Code to process payment
        return RedirectToAction("PaymentSuccess");
    }

    public IActionResult PaymentSuccess()
    {
        return View();
    }

    private List<int> ParseProducts(string itemIds)
    {
        // Implement your logic to parse itemIds
        throw new NotImplementedException();
    }
}
