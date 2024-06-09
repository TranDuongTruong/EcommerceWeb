

using System.Security.Claims;
using AppMvc.Net.Models.Cart;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;
using AppMvc.Net.Models;
using AppMvc.Net.Areas.User.Controllers;

public class CartService
{
    private readonly IHttpContextAccessor _contextAccessor; private readonly AppDbContext _context;

    public CartService(IHttpContextAccessor contextAccessor, AppDbContext context)
    {
        _contextAccessor = contextAccessor;
        _context = context;
    }



    // Lấy cart từ Session (danh sách CartItem)
    public async Task<int> GetCartItems()
    {
        var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.Carts
                                 .Include(c => c.CartItems)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart != null)
        {
            return cart.CartItems.Count;
        }

        return 0;
    }

    

}