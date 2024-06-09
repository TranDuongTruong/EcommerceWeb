@model AppMvc.Net.Models.Cart.CartModel

@{
    ViewData["Title"] = "My Cart";
    var items = Model.CartItems;
    var shopName = ViewBag.shopName as List<String>;

}
<h1>My Cart</h1>
<form asp-action="CreateOrder" method="post">
    @{

        <table class="table">
            <thead>
                <tr>
                    <th>
                        <input type="checkbox" id="select-all" />
                    </th>

                    <th>Product</th>
                    <th>Unit price</th>
                    <th>Quantity</th>
                    <th> Amount of money</th>

                    <th>Operation</th>
                </tr>
            </thead>
            <tbody>


                @for (int i = 0; i < items.Count; i++)
                {
                    <tr>
                        @if (i == 0)
                        {
                            <td>@shopName[i]</td>
                        }
                    </tr>
                    var photo = "";
                    if (items[i].Product.Photos != null)
                    {
                        photo = (File.Exists($"/contents/Products/{items[i].Product.Photos[0].FileName}")) ?
                        ("Products/" + items[i].Product.Photos.FirstOrDefault().FileName) : "icon.jpg";
                    }
                    else
                    {
                        photo = "icon.jpg";
                    }
                    var srcImg = $"/contents/{photo}";

                    <tr>
                        <td>
                            <input type="checkbox" name="selectedItems" value="@items[i].Id" class="item-checkbox"
                                data-price="@items[i].Product.Prices" data-quantity="@items[i].Quantity "
                                data-cartItemId="@items[i].Id" />
                        </td>
                        <td>
                            <img src="@srcImg" alt="Product Image" width="50" height="50" />
                            @items[i].Product.Title
                        </td>
                        <td>@items[i].Product.Prices.ToString("n3")</td>
                        <td class="form-inline">
                            <form asp-action="DecreaseQuantity" method="post" class="form-inline">
                                <input type="hidden" name="cartItemId" value="@items[i].Id" />

                                <button type="submit">-</button>
                            </form>

                            @items[i].Id

                            <form asp-action="IncreaseQuantity" method="post" class="form-inline">
                                <input type="hidden" name="cartItemId" value="@items[i].Id" />
                                <button type="submit">+</button>
                            </form>
                        </td>
                        <td>@((items[i].Quantity * items[i].Product.Prices).ToString("n3"))</td>
                        <td>
                            <form asp-action="RemoveItem" method="post">
                                <input type="hidden" name="cartItemId" value="@items[i].Id" />
                                <button type="submit" class="btn btn-danger">Remove</button>
                            </form>
                        </td>
                    </tr>
                    <tr>
                        @if (i < items.Count - 1)
                        {
                            if (items[i].Product.AuthorId != items[i + 1].Product.AuthorId)
                            {
                                <td id="add-voucher-button" onclick="toggleVoucherModal('@items[i-1].Product.AuthorId', 'Seller')">
                                    Thêm mã giảm giá của shop
                                </td>
                            }
                        }
                        @if (i == items.Count - 1)
                        {
                            <td id="add-voucher-button" onclick="toggleVoucherModal('@items[i].Product.AuthorId', 'Seller')">
                                Thêm mã giảm giá của shop
                            </td>
                        }


                    </tr>
                    <tr>
                        @if (i < items.Count - 1)
                        {
                            if (items[i].Product.AuthorId != items[i + 1].Product.AuthorId)
                            {

                                <td>@shopName[i + 1]</td>


                                <hr />
                                <hr />
                            }
                        }
                    </tr>
                }
            </tbody>
            <tr>
                <td colspan="4" class="text-right">Total</td>

                @* <td id="total-price">@(Model.CartItems.Sum(ci => ci.Quantity * ci.Product.Prices).ToString("n3"))</td> *@
                <td>
                    <span id="total-price" style="text-decoration: ">
                        @(Model.CartItems.Sum(ci => ci.Quantity * ci.Product.Prices).ToString("n3"))
                    </span>
                    <br />
                    <span id="discounted-price" style="color: red;">

                    </span>
                </td>
                <td id="add-voucher-button" onclick="toggleVoucherModal('000', 'System')">
                    Thêm mã giảm giá của hệ thống
                </td>
            </tr>

        </table>
    }

</form>

<form>
    <a class="btn btn-primary" onclick="Checkout()">Checkout </a>

</form>


<div id="voucher-modal">
    <div id="voucher-modal-content">
        <h2>Chọn mã giảm giá</h2>
        <div class="form-group d-flex justify-content-between">
            <input type="text" id="voucher-code" class="form-control" placeholder="Nhập mã voucher">
            <button id="apply-voucher-button" class="btn btn-primary">ÁP DỤNG</button>
        </div>
        <ul id="discount-list">
            <!-- Các item mã giảm giá sẽ được chèn vào đây -->
        </ul>
        <div id="modal-footer">
            <button class="modal-button" onclick="applyDiscount()">OK</button>
        </div>
    </div>
</div>


<script>
    document.getElementById('select-all').addEventListener('change', function () {
        var checkboxes = document.querySelectorAll('.item-checkbox');
        for (var checkbox of checkboxes) {
            checkbox.checked = this.checked;
        }
        updateTotal();
    });



    document.querySelectorAll('.item-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', function () {
            var allChecked = true;
            document.querySelectorAll('.item-checkbox').forEach(c => {
                if (!c.checked) {
                    allChecked = false;
                }
            });
            document.getElementById('select-all').checked = allChecked;
            //    document.getElementById('select-all-footer').checked = allChecked;
            updateTotal();
        });
    });

    function submitForm(input) {
        input.form.submit();
    }

    function updateTotal() {
        var total = 0;
        var selectedItems = [];

        document.querySelectorAll('.item-checkbox:checked').forEach(checkbox => {
            var price = parseFloat(checkbox.getAttribute('data-price'));
            var quantity = parseInt(checkbox.getAttribute('data-quantity'));
            var cartItemId = parseInt(checkbox.getAttribute('data-cartItemId'));
            console.log(price, quantity, cartItemId)
            if (!isNaN(price) && !isNaN(quantity)) {
                total += price * quantity;
                selectedItems.push({ id: checkbox.value, quantity: quantity, cartItemId: cartItemId });
            }
        });

        if (!isNaN(total)) {
            document.getElementById('total-price').innerText = total.toFixed(3);
            console.log(selectedItems)
            localStorage.setItem('selectedItems', JSON.stringify(selectedItems));

            // Lấy giá trị giảm giá từ localStorage và tính toán lại giá giảm giá
            var systemDiscountDetails = JSON.parse(localStorage.getItem('systemDiscountDetails'));
            var shopDiscountDetails = JSON.parse(localStorage.getItem('shopDiscountDetails'));

            var newPrice = total;
            if (systemDiscountDetails) {
                newPrice = applyDiscountToTotal(newPrice, systemDiscountDetails);
            }
            if (shopDiscountDetails) {
                newPrice = applyDiscountToTotal(newPrice, shopDiscountDetails);
            }

            if (systemDiscountDetails || shopDiscountDetails) {
                document.getElementById('discounted-price').innerText = newPrice.toFixed(3);
                document.getElementById('total-price').style.textDecoration = 'line-through';
            } else {
                document.getElementById('discounted-price').innerText = "";
                document.getElementById('total-price').style.textDecoration = 'none';
            }
        } else {
            document.getElementById('total-price').innerText = "0.000";
            document.getElementById('discounted-price').innerText = "";
            document.getElementById('total-price').style.textDecoration = 'none';
        }
    }

    function applyDiscountToTotal(total, discountDetails) {
        if (discountDetails.type === 2) {
            return (total - discountDetails.amount) > 0 ? (total - discountDetails.amount) : 0;
        } else if (discountDetails.type === 1) {
            return (total - (total * (discountDetails.percentage / 100))) > 0 ? (total - (total * (discountDetails.percentage / 100))) : 0;
        }
        return total;
    }

    window.addEventListener('DOMContentLoaded', function () {
        // Check if selected items are stored in local storage
        var storedItems = localStorage.getItem('selectedItems');
        if (storedItems) {
            var selectedItems = JSON.parse(storedItems);
            // Check the checkboxes for stored selected items
            selectedItems.forEach(item => {
                var checkbox = document.querySelector('.item-checkbox[value="' + item.id + '"]');
                if (checkbox) {
                    checkbox.checked = true;
                    // Update quantity input for selected item
                    var quantityInput = document.querySelector('.quantity-input[data-id="' + item.id + '"]');
                    if (quantityInput) {
                        quantityInput.value = item.quantity;
                    }
                }
            });
        }

        // Check if discount details are stored in local storage
        var systemDiscountDetails = JSON.parse(localStorage.getItem('systemDiscountDetails'));
        var shopDiscountDetails = JSON.parse(localStorage.getItem('shopDiscountDetails'));

        var total = parseFloat(document.getElementById('total-price').innerText);
        var newPrice = total;

        if (systemDiscountDetails) {
            newPrice = applyDiscountToTotal(newPrice, systemDiscountDetails);
        }
        if (shopDiscountDetails) {
            newPrice = applyDiscountToTotal(newPrice, shopDiscountDetails);
        }

        if (systemDiscountDetails || shopDiscountDetails) {
            document.getElementById('discounted-price').innerText = newPrice.toFixed(3);
            document.getElementById('total-price').style.textDecoration = 'line-through';
        }

        // Update total based on stored selected items
        updateTotal();
    });

    function toggleVoucherModal(authorId, discountAuthorType) {
        var modal = document.getElementById("voucher-modal");
        console.log("aa:", (`/ViewCartController/GetDiscounts/${authorId}/${discountAuthorType}`));
        if (modal.style.display === "block") {
            modal.style.display = "none";
        } else {
            // Make an AJAX call to get the discounts
            fetch(`/ViewCartController/GetDiscounts/${authorId}/${discountAuthorType}`)
                .then(response => response.json())
                .then(data => {
                    console.log("aaaaaa", data);
                    // Update the modal content with the discounts
                    var discountList = document.getElementById("discount-list");
                    discountList.innerHTML = ''; // Clear existing content
                    var selectedDiscountCode;
                    // Lấy mã giảm giá đã lưu trong local storage
                    if (discountAuthorType == "Seller") selectedDiscountCode = localStorage.getItem('selectedShopDiscountCode');
                    else selectedDiscountCode = localStorage.getItem('selectedSystemDiscountCode');
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            var listItem = document.createElement("li");
                            var label = document.createElement("label");
                            var radio = document.createElement("input");
                            radio.setAttribute("type", "radio");
                            radio.setAttribute("name", "discount");
                            radio.setAttribute("value", data[key]);
                            radio.setAttribute("authorId", authorId);

                            // Đánh dấu radio nếu nó đã được chọn trước đó
                            if (selectedDiscountCode && data[key] === selectedDiscountCode) {
                                radio.checked = true;
                            }

                            radio.addEventListener('click', function () {
                                // Kiểm tra nếu đã được chọn và click vào nó thì bỏ tick
                                if (this.checked && this.previousChecked) {
                                    this.checked = false;
                                    this.previousChecked = false;
                                    if (discountAuthorType == "Seller") {
                                        localStorage.removeItem('selectedShopDiscountCode');
                                        localStorage.removeItem('shopDiscountDetails');
                                    } else {
                                        localStorage.removeItem('selectedSystemDiscountCode');
                                        localStorage.removeItem('systemDiscountDetails');
                                    }
                                    var totalPriceElement = document.getElementById('total-price');
                                    var discountedPriceElement = document.getElementById('discounted-price');
                                    if (totalPriceElement && discountedPriceElement) {
                                        totalPriceElement.style.textDecoration = 'none';
                                        discountedPriceElement.innerText = '';
                                    }
                                    updateTotal();
                                } else {
                                    // Nếu không, lưu giá trị hiện tại là được chọn
                                    document.querySelectorAll('input[name="discount"]').forEach(rb => {
                                        rb.previousChecked = false;
                                    });
                                    this.previousChecked = true;
                                    if (discountAuthorType == "Seller") localStorage.setItem('selectedShopDiscountCode', this.value);
                                    else localStorage.setItem('selectedSystemDiscountCode', this.value);
                                }
                            });

                            label.appendChild(radio);
                            label.appendChild(document.createTextNode(key));
                            listItem.appendChild(label);
                            discountList.appendChild(listItem);
                        }
                    }

                    // Show the modal
                    modal.style.display = "block";
                })
                .catch(error => console.error('Error fetching discounts:', error));
        }
    }

    window.onclick = function (event) {
        var modal = document.getElementById("voucher-modal");
        if (event.target === modal) {
            modal.style.display = "none";
        }
    }

    function applyDiscount() {

        var authorId = [];
        var price = parseFloat(document.getElementById('total-price').innerText);

        // Lặp qua tất cả các checkbox discount để lấy những mã giảm giá được chọn
        var discountCode = [];
        document.querySelectorAll('input[name="discount"]:checked').forEach(checkbox => {
            discountCode.push(checkbox.value);
            authorId.push(checkbox.getAttribute("authorId"));
        });
        if (!authorId[0] || !discountCode[0]) {
            return;
        }
        console.log(`/ViewCartController/ApplyDiscount/${discountCode[0]}/${authorId[0]}/${price}`);
        fetch(`/ViewCartController/ApplyDiscount/${discountCode[0]}/${authorId[0]}/${price}`).then(response => response.json())
            .then(data => {
                console.log(`/ViewCartController/ApplyDiscount/${discountCode[0]}/${authorId[0]}/${price}`);
                console.log(data.newPrice);
                if (data.success) {
                    // Cập nhật giá thành công
                    var discountDetails = {
                        type: data.discountType,
                        amount: data.amount,
                        percentage: data.percentage
                    };
                    if (authorId[0] == "000") {
                        localStorage.setItem('selectedSystemDiscountCode', discountCode[0]);
                        localStorage.setItem('systemDiscountDetails', JSON.stringify(discountDetails));
                    } else {
                        localStorage.setItem('selectedShopDiscountCode', discountCode[0]);
                        localStorage.setItem('shopDiscountDetails', JSON.stringify(discountDetails));
                    }

                    var totalPriceElement = document.getElementById('total-price');
                    var discountedPriceElement = document.getElementById('discounted-price');
                    if (totalPriceElement && discountedPriceElement) {
                        totalPriceElement.style.textDecoration = 'line-through';
                        discountedPriceElement.innerText = data.newPrice.toFixed(3);
                    }

                    // Đóng modal sau khi cập nhật thành công
                    var modal = document.getElementById("voucher-modal");
                    modal.style.display = "none";
                    updateTotal();
                } else {
                    // Hiển thị thông báo lỗi nếu có
                    console.log(`/ViewCartController/ApplyDiscount/${discountCode[0]}/${authorId[0]}/${price}`);
                    alert("aaaaaaaaaaaaaa", data.message,);
                }
            })
            .catch(error => console.error('Error applying discounts:', error));
    }



    function Checkout() {
        var selectedItems = JSON.parse(localStorage.getItem('selectedItems'));

        // Tạo hai mảng riêng biệt cho itemIds và quantities
        var itemIds = selectedItems.map(item => item.id);
        var quantities = selectedItems.map(item => item.quantity);
        console.log(itemIds)
        var shopDiscountCode = localStorage.getItem('selectedShopDiscountCode');
        var systemDiscountCode = localStorage.getItem('selectedSystemDiscountCode');

        // Tạo mảng items và quantities từ localStorage
        var items = selectedItems.map(item => item.id);
        var quantities = selectedItems.map(item => item.quantity);

        // Tạo payload từ các mảng và các giá trị khác
        var payload = {
            itemIds: items,
            quantities: quantities,
            selectedShopDiscountCode: shopDiscountCode,
            selectedSystemDiscountCode: systemDiscountCode
        };
        var url = ""
        selectedItems.forEach(item => {
            url += item.id + "_" + item.quantity + "_";
        });

        console.log("aaa" + (`/cart/checkout/${url}/${shopDiscountCode}/${systemDiscountCode}`));
        //  alert("aaaaaa");
        // Gửi yêu cầu POST với dữ liệu JSON
        fetch(`/cart/checkout/${url}/${shopDiscountCode}/${systemDiscountCode}`)
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                throw new Error('Có lỗi xảy ra khi gửi yêu cầu.');
            })
            .then(data => {
                console.log(data);
                console.log("Dữ liệu đã nhận được.");
            })
            .catch(error => {
                console.error('Lỗi:', error);
                alert("Có lỗi xảy ra: " + error.message);
            });

    }

</script>










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

namespace AppMvc.Net.Areas.User.Controllers
{
    [Area("Cart")]
    [Authorize]
    [Route("/cart")]

    [ApiController]
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


            if (authorId == "000")
            {
                var discount = await _context.DiscountModels
                                             .Where(d => d.AuthorType == DiscountAuthorType.System && d.Code == discountCode)
                                             .FirstOrDefaultAsync() as DiscountModel;
                if (discount != null)
                {

                    newPrice = discount.ApplyDiscount(Convert.ToDecimal(price));
                    success = true;
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


                if (discount != null)
                {

                    newPrice = discount.ApplyDiscount(Convert.ToDecimal(price));
                    success = true;
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

        public class CartSelectedItem
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
        }
        // [HttpPost]
        // [Route("/ViewCartController/Checkout/")]
        // public void Checkout([FromBody] List<CartItemPayload> selectedProducts)
        // {

        // }
        // public class CartItemPayload
        // {
        //     public int Id { get; set; }
        //     public int Quantity { get; set; }
        // }
        // public class CartDiscountPayload
        // {
        //     public int Id { get; set; }
        //     public int Quantity { get; set; }
        // }


        // [Route("/ViewCartController/Checkout/")]
        // public IActionResult Checkout()
        // {
        //     return View();
        // }



        public static List<SelectedProducts> ParseProducts(string products)
        {
            List<SelectedProducts> productList = new List<SelectedProducts>();

            // Remove the trailing underscore if it exists
            if (products.EndsWith("_"))
            {
                products = products.TrimEnd('_');
            }

            // Split the string by underscore
            string[] parts = products.Split('_');

            // Iterate over the parts and create SelectedProducts objects
            for (int i = 0; i < parts.Length; i += 2)
            {
                int id = int.Parse(parts[i]);
                int quantity = int.Parse(parts[i + 1]);
                productList.Add(new SelectedProducts { Id = id, Quantity = quantity });
            }

            return productList;
        }
        [Route("/cart/checkout/{products}/{shopDiscountCode}/{systemDiscountCode}")]
        public IActionResult Checkout(string products, string shopDiscountCode, string systemDiscountCode)


        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (products == null)
            {
                string message = ("Invalid items ");
                return Json(new { success = false, message = message });
            }
            List<SelectedProducts> productList = ParseProducts(products);
            SelectedCartItems selectedCartItems = new SelectedCartItems()
            {
                SelectedProducts = productList,
                SelectedShopDiscountCode = shopDiscountCode,
                SelectedSystemDiscountCode = systemDiscountCode
            };

            cart.SelectedCartItems = selectedCartItems;
            // return Json(new { success = true, message = (products + shopDiscountCode + "\t" + selectedCartItems.SelectedProducts.Count) });


            decimal totalPrice = 0;

            // Kiểm tra sản phẩm và số lượng tồn kho
            for (int i = 0; i < selectedCartItems.SelectedProducts.Count; i++)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == selectedCartItems.SelectedProducts[i].Id);
                if (product == null)
                {
                    return Json(new { success = false, message = $"Product with ID {selectedCartItems.SelectedProducts[i].Id} does not exist" });
                }

                if (!product.CheckoutProduct(selectedCartItems.SelectedProducts[i].Quantity))
                {
                    return Json(new { success = false, message = $"Not enough stock for product with ID {selectedCartItems.SelectedProducts[i].Id}" });
                }

                // Tính tổng giá tiền
                totalPrice += product.Prices * selectedCartItems.SelectedProducts[i].Quantity;
            }

            // Xử lý mã giảm giá của shop
            if (!string.IsNullOrEmpty(selectedCartItems.SelectedShopDiscountCode))
            {
                var shopDiscount = _context.DiscountModels.Where(d => d.AuthorType == DiscountAuthorType.Seller).FirstOrDefault(d => d.Code == selectedCartItems.SelectedShopDiscountCode);
                if (shopDiscount == null || !shopDiscount.IsActive)
                {
                    return Json(new { success = false, message = "Invalid or inactive shop discount code" });
                }

                totalPrice = shopDiscount.ApplyDiscount(totalPrice);
                shopDiscount.CurrentUsage++;

                // In giá cuối cùng ra console
                System.Diagnostics.Debug.WriteLine($"Total Price after shop discount: {totalPrice}");
            }

            // Xử lý mã giảm giá của hệ thống
            if (!string.IsNullOrEmpty(selectedCartItems.SelectedSystemDiscountCode))
            {
                var systemDiscount = _context.DiscountModels.Where(d => d.AuthorType == DiscountAuthorType.System).FirstOrDefault(d => d.Code == selectedCartItems.SelectedSystemDiscountCode);
                if (systemDiscount == null || !systemDiscount.IsActive)
                {
                    return Json(new { success = false, message = "Invalid or inactive system discount code" });
                }

                totalPrice = systemDiscount.ApplyDiscount(totalPrice);
                systemDiscount.CurrentUsage++;

                // In giá cuối cùng ra console
                System.Diagnostics.Debug.WriteLine($"Total Price after system discount: {totalPrice}");
            }

            // Nếu mọi thứ đều thành công, cập nhật số lượng tồn kho và lưu thông tin đơn hàng
            for (int i = 0; i < selectedCartItems.SelectedProducts.Count; i++)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == selectedCartItems.SelectedProducts[i].Id);
                if (product != null)
                {
                    product.Quantity -= selectedCartItems.SelectedProducts[i].Quantity;
                }
            }



            _context.SaveChanges();

            // In giá cuối cùng ra console
            System.Diagnostics.Debug.WriteLine($"Final Total Price: {totalPrice}");

            return Json(new { success = true, totalPrice });
        }



    }

}












