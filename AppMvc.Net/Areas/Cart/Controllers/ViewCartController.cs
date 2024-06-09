using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AppMvc.Net.Models;
using AppMvc.Net.Models.Cart;
using Microsoft.AspNetCore.Identity;
using AppMvc.Net.Models.Discount;
using Newtonsoft.Json;

namespace AppMvc.Net.Areas.User.Controllers
{
    [Area("Cart")]
    [Authorize]
    public class ViewCartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly CartService _cartService;
        public ViewCartController(AppDbContext context, UserManager<AppUser> UserManager, CartService cartService)
        {
            _context = context;
            _userManager = UserManager;
            _cartService = cartService;
        }

        // GET: User/Cart
        [Route("/viewCart")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cart = await _context.Carts
                                     .Include(c => c.CartItems)
                                     .ThenInclude(ci => ci.Product)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new CartModel { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            List<String> shopName = new List<string>();
            foreach (var p in cart.CartItems)
            {
                string n = await GetProductAuthor(p.Product.AuthorId);
                shopName.Add(n);
            }




            var products = await _context.Products.OrderBy(p => p.AuthorId).ToListAsync();

            ViewBag.Products = products;
            ViewBag.shopName = shopName;
            ViewBag.CartService = _cartService;
            return View(cart);
        }
        public async Task<string> GetProductAuthor(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user?.UserName;
        }


        // POST: User/Cart/AddItem
        [HttpPost]
        [Route("/Cart/AddItem")]
        public async Task<IActionResult> AddItem(int productId, int quantity = 1)
        {
            Console.WriteLine("aaaaaaaddddd");
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cart = await _context.Carts.Include(c => c.CartItems)
                                            .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new CartModel { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItem { ProductId = productId, Quantity = quantity, CartId = cart.CartId };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: User/Cart/RemoveItem
        [HttpPost]
        [Route("/Cart/RemoveItem")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Route("/Cart/DecreaseQuantity")]
        public async Task<IActionResult> DecreaseQuantity(int cartItemId)
        {
            Console.WriteLine("iddđ:" + cartItemId);
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Route("/Cart/IncreaseQuantity")]
        public async Task<IActionResult> IncreaseQuantity(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                _context.CartItems.Update(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: User/Cart/CreateOrder

        [HttpPost]
        [Route("/cart/order")]
        public async Task<IActionResult> CreateOrder(List<int> selectedItems)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Process selected items and create an order
            var selectedCartItems = await _context.CartItems
                                                   .Where(ci => selectedItems.Contains(ci.Id) && ci.Cart.UserId == userId)
                                                   .Include(ci => ci.Product)
                                                   .ToListAsync();

            // Implement order creation logic here
            // Example:
            // var order = new Order { UserId = userId, OrderItems = selectedCartItems.Select(ci => new OrderItem { ... }).ToList() };
            // _context.Orders.Add(order);
            // await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("/ViewCartController/GetDiscounts/{authorId}/{discountAuthorType}")]
        public async Task<IActionResult> GetDiscounts(string authorId, string discountAuthorType)
        {
            Dictionary<string, string> discount = new Dictionary<string, string>();

            if (Enum.TryParse<DiscountAuthorType>(discountAuthorType, out var discountAuthorTypeEnum))
            {
                if (discountAuthorTypeEnum == DiscountAuthorType.System)
                {
                    var systemDiscounts = await _context.DiscountModels
                                                        .Where(d => d.AuthorType == DiscountAuthorType.System)
                                                        .Select(d => new { d.Name, d.Code, d.DiscountType, d.Amount, d.Percentage, d.MinimumOrder, d.EndDate })
                                                        .ToListAsync();

                    foreach (var item in systemDiscounts)
                    {
                        if (item.DiscountType == DiscountType.Amount)
                            discount.Add(item.Name + "\n Giảm " + item.Amount + "K\n Đơn tối thiểu " + item.MinimumOrder + "K\nHSD: " + item.EndDate.ToString().Split(' ')[0], item.Code);
                        else
                        {
                            discount.Add(item.Name + "\n Giảm " + item.Percentage + "%\n Đơn tối thiểu " + item.MinimumOrder + "K\nHSD: " + item.EndDate.ToString().Split(' ')[0], item.Code);
                        }
                    }
                }
                else
                {
                    var sellerDiscount = await _context.DiscountModels
                          .Where(d => d.AuthorType == DiscountAuthorType.Seller && d.AuthorId == authorId)
                          .Select(d => new { d.Name, d.Code, d.DiscountType, d.Amount, d.Percentage, d.MinimumOrder, d.EndDate })
                         .ToListAsync();
                    foreach (var item in sellerDiscount)
                    {
                        if (item.DiscountType == DiscountType.Amount)
                            discount.Add(item.Name + "\n Giảm " + item.Amount + "K\n Đơn tối thiểu " + item.MinimumOrder + "K\nHSD: " + item.EndDate.ToString().Split(' ')[0], item.Code);
                        else
                        {
                            discount.Add(item.Name + "\n Giảm " + item.Percentage + "%\n Đơn tối thiểu " + item.MinimumOrder + "K\nHSD: " + item.EndDate.ToString().Split(' ')[0], item.Code);
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Invalid discount author type");
            }

            return Json(discount);
        }


        [Route("/ViewCartController/ApplyDiscount/{discountCode}/{authorId}/{price}")]
        public async Task<JsonResult> ApplyDiscount(string authorId, string discountCode, double price)
        {
            decimal newPrice = new decimal();
            bool success = false; // Khởi tạo biến thành công
            DiscountType discountType = new DiscountType();
            decimal amount = 0;
            double percentage = 0;

            // Ép kiểu biến Discount thành kiểu DiscountModel
            if (authorId == "000")
            {
                var discount = await _context.DiscountModels
                                             .Where(d => d.AuthorType == DiscountAuthorType.System && d.Code == discountCode)
                                             .FirstOrDefaultAsync() as DiscountModel;
                if (discount != null)
                {
                    // Thực hiện logic tính toán giá mới dựa trên discount
                    newPrice = discount.ApplyDiscount(Convert.ToDecimal(price));
                    success = true; // Đặt giá trị thành công thành true
                    discountType = discount.DiscountType;
                    amount = discount.Amount.HasValue ? discount.Amount.Value : 0;
                    percentage = discount.Percentage.HasValue ? discount.Percentage.Value : 0;
                }
            }
            else
            {
                var discount = await _context.DiscountModels
                                              .Where(d => d.AuthorId == authorId && d.Code == discountCode)
                                              .FirstOrDefaultAsync() as DiscountModel;

                // Kiểm tra nếu discount không null
                if (discount != null)
                {
                    // Thực hiện logic tính toán giá mới dựa trên discount
                    newPrice = discount.ApplyDiscount(Convert.ToDecimal(price));
                    success = true; // Đặt giá trị thành công thành true
                    discountType = discount.DiscountType;
                    amount = discount.Amount.HasValue ? discount.Amount.Value : 0;
                    percentage = discount.Percentage.HasValue ? discount.Percentage.Value : 0;
                }
            }

            // Tạo đối tượng JSON chứa giá mới và thành công
            var result = new { discountType, amount, percentage, newPrice, success };

            // Trả về dữ liệu dưới dạng JSON
            return Json(result);
        }


        [Route("/cart/checkout/{iemIds}/{shopDiscountCode}/{systemDiscountCode}")]
        public async Task<IActionResult> Checkout(string iemIds, string shopDiscountCode, string systemDiscountCode)


        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (iemIds == null)
            {
                string message = ("Invalid items ");
                return Json(new { success = false, message = message });
            }
            List<int> cartItemIds = ParseProducts(iemIds);

            List<CartItem> cartItems = await _context.CartItems
               .Where(ci => cartItemIds.Contains(ci.Id))
               .ToListAsync();

            // return Json(new { success = true, message = (products + shopDiscountCode + "\t" + selectedCartItems.SelectedProducts.Count) });
            Console.WriteLine("cartItems111111:" + cartItemIds.Count + "\t\n");


            decimal totalPrice = 0;

            // Kiểm tra sản phẩm và số lượng tồn kho
            for (int i = 0; i < cartItems.Count; i++)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == cartItems[i].ProductId);
                if (product == null)
                {
                    return Json(new { success = false, message = $"Product with ID {cartItems[i].ProductId} does not exist" });
                }

                if (!product.CheckoutProduct(cartItems[i].Quantity))
                {
                    return Json(new { success = false, message = $"Not enough stock for product with ID {cartItems[i].ProductId}" });
                }

                // Tính tổng giá tiền
                totalPrice += product.Prices * cartItems[i].Quantity;
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
                    //  shopDiscount.CurrentUsage++;

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
                    //    systemDiscount.CurrentUsage++;

                    // In giá cuối cùng ra console
                    System.Diagnostics.Debug.WriteLine($"Total Price after system discount: {totalPrice}");
                }
            }

            // Nếu mọi thứ đều thành công, cập nhật số lượng tồn kho và lưu thông tin đơn hàng
            // for (int i = 0; i < cartItems.Count; i++)
            // {
            //     var product = _context.Products.FirstOrDefault(p => p.Id == cartItems[i].ProductId);
            //     if (product != null)
            //     {
            //         product.Quantity -= cartItems[i].Quantity;
            //     }
            // }

            //   _context.CartItems.RemoveRange(cartItems);


            _context.SaveChanges();

            // In giá cuối cùng ra console
            System.Diagnostics.Debug.WriteLine($"Final Total Price: {totalPrice}");

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(cartItems, jsonSettings));
            HttpContext.Session.SetString("ShopDiscountCode", shopDiscountCode);
            HttpContext.Session.SetString("SystemDiscountCode", systemDiscountCode);

            string redirectUrl = Url.Action("Index", "Checkout", new { area = "Checkout" });

            return Json(new
            {
                success = true,
                totalPrice,
                redirectUrl

            });
        }

        public static List<int> ParseProducts(string cartItems)
        {
            List<int> cartItemIds = new List<int>();

            // Remove the trailing underscore if it exists
            if (cartItems.EndsWith("_"))
            {
                cartItems = cartItems.TrimEnd('_');
            }

            // Split the string by underscore
            string[] parts = cartItems.Split('_');

            // Iterate over the parts and create SelectedProducts objects
            for (int i = 0; i < parts.Length; i++)
            {
                int id = int.Parse(parts[i]);
                cartItemIds.Add(id);
            }

            return cartItemIds;
        }
    }

}