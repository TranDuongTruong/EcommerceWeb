using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppMvc.Net.Models.Discount;
using AppMvc.Net.Data;
using AppMvc.Net.Models;
using AppMvc.Net.Models.Blog;
using AppMvc.Net.Models.Product;
using Microsoft.AspNetCore.Identity;
using AppMvc.Net.Areas.Discount.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppMvc.Net.Controllers
{
    [Area("Discount")]
    [Authorize]
    public class DiscountsManageController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DiscountsManageController(AppDbContext context, UserManager<AppUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }
        [Route("/DiscountsManage")]
        // GET: Discounts
        [Authorize(Roles = "Editor,Administrator")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var userRoles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));

            IQueryable<DiscountModel> discounts = _context.DiscountModels.Include(d => d.Author);

            if (userRoles.Contains("Editor"))
            {
                discounts = discounts.Where(d => d.AuthorId == userId);
            }

            return View(await discounts.ToListAsync());
        }


        // GET: Discounts/Details/5
        [HttpGet]
        [Route("/DiscountsManage/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.DiscountModels.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            var discountDetails = new CreateDiscountModel()
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                Code = discount.Code,
                MaxUsage = discount.MaxUsage,
                CurrentUsage = discount.CurrentUsage,
                DiscountType = discount.DiscountType,
                Percentage = discount.Percentage,
                Amount = discount.Amount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                AuthorId = discount.AuthorId,
                Author = discount.Author,
                IsForAllProducts = discount.IsForAllProducts,
                ProductIDs = _context.DiscountProducts.Where(dp => dp.DiscountID == id).Select(dp => dp.ProductID).ToArray(),
                CategoryIDs = _context.DiscountCategories.Where(dc => dc.DiscountID == id).Select(dc => dc.CategoryID).ToArray(),
                AuthorType = discount.AuthorType,

            };
            List<string> Products = new List<string>();
            foreach (var i in discountDetails.ProductIDs)
            {
                var p = _context.Products.Where(p => p.Id == i).Select(p => p.Title).FirstOrDefault();
                Products.Add(p);
            }

            List<string> Categories = new List<string>();
            foreach (var i in discountDetails.CategoryIDs)
            {
                var p = _context.CategoryProducts.Where(p => p.Id == i).Select(p => p.Title).FirstOrDefault();
                Categories.Add(p);
                Console.WriteLine("Categories", p);
            }
            ViewData["Products"] = Products;
            ViewData["Categories"] = Categories;

            ViewBag.AuthorId = new SelectList(_context.Users, "Id", "UserName");

            return View(discountDetails);

        }

        // GET: Discounts/Create
        [HttpGet]
        [Route("/DiscountsManage/Create")]
        public async Task<IActionResult> Create()
        {
            var userId = _userManager.GetUserId(User);
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "UserName");

            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            var products = await _context.Products.Where(p => p.AuthorId == userId).ToListAsync();
            ViewData["products"] = new MultiSelectList(products, "Id", "Title");

            return View();
        }

        [HttpPost]
        [Route("/DiscountsManage/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDiscountModel model)
        {
            if (ModelState.IsValid)
            {
                var discount = new DiscountModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    Code = model.Code,
                    MaxUsage = model.MaxUsage,
                    DiscountType = model.DiscountType,
                    Percentage = model.DiscountType == DiscountType.Percentage ? model.Percentage : null,
                    Amount = model.DiscountType == DiscountType.Amount ? model.Amount : null,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    AuthorId = model.AuthorId,
                    IsForAllProducts = model.IsForAllProducts,
                    AuthorType = model.AuthorType
                };

                // Add the discount to the context first
                _context.DiscountModels.Add(discount);
                await _context.SaveChangesAsync(); // Save to get the ID

                // Add selected categories
                if (model.CategoryIDs != null)
                {
                    foreach (var CateId in model.CategoryIDs)
                    {
                        _context.DiscountCategories.Add(new DiscountCategory()
                        {
                            CategoryID = CateId,
                            DiscountID = discount.Id // Use the ID of the saved discount
                        });
                    }
                }

                // Add selected products
                if (model.ProductIDs != null)
                {
                    foreach (var ProdId in model.ProductIDs)
                    {
                        _context.DiscountProducts.Add(new DiscountProduct()
                        {
                            ProductID = ProdId,
                            DiscountID = discount.Id // Use the ID of the saved discount
                        });
                    }
                }

                await _context.SaveChangesAsync(); // Save changes for categories and products

                return RedirectToAction(nameof(Index));
            }

            ViewData["products"] = new MultiSelectList(_context.Products, "Id", "Name");
            ViewData["categories"] = new MultiSelectList(_context.CategoryProducts, "Id", "Name");
            ViewBag.AuthorId = new SelectList(_context.Users, "Id", "UserName");
            return View(model);
        }

        // GET: Discounts/Edit/5
        // GET: Discounts/Edit/5
        [HttpGet]
        [Route("/DiscountsManage/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.DiscountModels.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            var discountEdit = new CreateDiscountModel()
            {
                Id = discount.Id,
                Name = discount.Name,
                Description = discount.Description,
                Code = discount.Code,
                MaxUsage = discount.MaxUsage,
                CurrentUsage = discount.CurrentUsage,
                DiscountType = discount.DiscountType,
                Percentage = discount.Percentage,
                Amount = discount.Amount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                AuthorId = discount.AuthorId,
                Author = discount.Author,
                IsForAllProducts = discount.IsForAllProducts,
                ProductIDs = _context.DiscountProducts.Where(dp => dp.DiscountID == id).Select(dp => dp.ProductID).ToArray(),
                CategoryIDs = _context.DiscountCategories.Where(dc => dc.DiscountID == id).Select(dc => dc.CategoryID).ToArray(),
                AuthorType = discount.AuthorType,
            };

            ViewData["products"] = new MultiSelectList(_context.Products, "Id", "Title");
            ViewData["categories"] = new MultiSelectList(_context.CategoryProducts, "Id", "Title");
            ViewBag.AuthorId = new SelectList(_context.Users, "Id", "UserName");

            return View(discountEdit);
        }

        [HttpPost]
        [Route("/DiscountsManage/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateDiscountModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var discount = await _context.DiscountModels.FindAsync(id);
                    if (discount == null)
                    {
                        return NotFound();
                    }

                    // Update the discount information
                    discount.Name = model.Name;
                    discount.Description = model.Description;
                    discount.Code = model.Code;
                    discount.MaxUsage = model.MaxUsage;
                    discount.CurrentUsage = model.CurrentUsage;
                    discount.DiscountType = model.DiscountType;
                    discount.Percentage = model.DiscountType == DiscountType.Percentage ? model.Percentage : null;
                    discount.Amount = model.DiscountType == DiscountType.Amount ? model.Amount : null;
                    discount.StartDate = model.StartDate;
                    discount.EndDate = model.EndDate;
                    discount.AuthorId = model.AuthorId;
                    discount.Author = model.Author;
                    discount.IsForAllProducts = model.IsForAllProducts;
                    discount.AuthorType = model.AuthorType;

                    _context.Update(discount);

                    // Remove existing product and category associations
                    var existingProductAssociations = _context.DiscountProducts.Where(dp => dp.DiscountID == id);
                    _context.DiscountProducts.RemoveRange(existingProductAssociations);

                    var existingCategoryAssociations = _context.DiscountCategories.Where(dc => dc.DiscountID == id);
                    _context.DiscountCategories.RemoveRange(existingCategoryAssociations);

                    // Add new product and category associations
                    if (model.CategoryIDs != null)
                    {
                        foreach (var CateId in model.CategoryIDs)
                        {
                            _context.DiscountCategories.Add(new DiscountCategory()
                            {
                                CategoryID = CateId,
                                DiscountID = discount.Id
                            });
                        }
                    }

                    if (model.ProductIDs != null)
                    {
                        foreach (var ProdId in model.ProductIDs)
                        {
                            _context.DiscountProducts.Add(new DiscountProduct()
                            {
                                ProductID = ProdId,
                                DiscountID = discount.Id
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountModelExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["products"] = new MultiSelectList(_context.Products, "Id", "Name");
            ViewData["categories"] = new MultiSelectList(_context.CategoryProducts, "Id", "Name");
            ViewBag.AuthorId = new SelectList(_context.Users, "Id", "UserName");

            return View(model);
        }

        private bool DiscountModelExists(int id)
        {
            return _context.DiscountModels.Any(e => e.Id == id);
        }

        // GET: Discounts/Delete/5
        [Route("/DiscountsManage/Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discountModel = await _context.DiscountModels
                .Include(d => d.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discountModel == null)
            {
                return NotFound();
            }

            return View(discountModel);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("/DiscountsManage/Delete")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discountModel = await _context.DiscountModels.FindAsync(id);
            _context.DiscountModels.Remove(discountModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private async Task<List<CategoryProduct>> GetProductCategoriesAsync()
        {
            var categories = await _context.CategoryProducts
                            .Include(c => c.Title).Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren)
                            .ToListAsync();
            return categories;
        }
    }
}
