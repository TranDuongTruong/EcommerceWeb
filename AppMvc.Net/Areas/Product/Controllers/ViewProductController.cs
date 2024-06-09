using AppMvc.Net.Areas.Product.Models;
using AppMvc.Net.Models;
using AppMvc.Net.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMvc.Net.Areas.Product.Controllers
{
    [Area("Product")]
    public class ViewProductController : Controller
    {
        private readonly ILogger<ViewProductController> _logger;
        private readonly AppDbContext _context;


        public ViewProductController(ILogger<ViewProductController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        // /post/
        // /post/{categoryslug?}
        [Route("/product/{categoryslug?}")]
        public IActionResult Index(string categoryslug, [FromQuery(Name = "p")] int currentPage, int pagesize)
        {

            var categories = GetCategories();
            ViewBag.categories = categories;
            ViewBag.categoryslug = categoryslug;

            CategoryProduct category = null;

            if (!string.IsNullOrEmpty(categoryslug))
            {
                category = _context.CategoryProducts.Where(c => c.Slug == categoryslug)
                                    .Include(c => c.CategoryChildren)
                                    .FirstOrDefault();

                if (category == null)
                {
                    return NotFound("Không thấy category");
                }
            }

            var products = _context.Products
                                .Include(p => p.Author)
                                .Include(p => p.Photos)
                                .Include(p => p.ProductCategoryProducts)
                                .ThenInclude(p => p.Category)
                                .AsQueryable();

            products = products.OrderByDescending(p => p.DateUpdated);

            if (category != null)
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(null, ids);
                ids.Add(category.Id);


                products = products.Where(p => p.ProductCategoryProducts.Where(pc => ids.Contains(pc.CategoryID)).Any());


            }

            int totalProducts = products.Count();
            if (pagesize <= 0) pagesize = 12;
            int countPages = (int)Math.Ceiling((double)totalProducts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

            var productsInPage = products.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize);


            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalProducts;



            ViewBag.category = category;
            return View(productsInPage.ToList());
        }

        [Route("/productdetail/{productslug}.html")]
        public IActionResult Detail(string productslug)
        {
            Console.WriteLine("aaaaaaa:", productslug);
            var categories = GetCategories();
            ViewBag.categories = categories;

            var product = _context.Products.Where(p => p.Slug == productslug)
                               .Include(p => p.Author).Include(p => p.Photos)
                               .Include(p => p.ProductCategoryProducts)
                               .ThenInclude(pc => pc.Category)
                               .FirstOrDefault();

            if (product == null)
            {
                return NotFound("Không thấy sản phẩm");
            }

            CategoryProduct category = product.ProductCategoryProducts.FirstOrDefault()?.Category;
            ViewBag.category = category;

            var otherProducts = _context.Products.Where(p => p.ProductCategoryProducts.Any(c => c.Category.Id == category.Id))
                                            .Where(p => p.Id != product.Id)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
            ViewBag.otherProducts = otherProducts;

            return View(product);
        }

        private List<CategoryProduct> GetCategories()
        {
            var categories = _context.CategoryProducts
                            .Include(c => c.CategoryChildren)
                            .AsEnumerable()
                            .Where(c => c.ParentCategory == null)
                            .ToList();
            return categories;
        }

        /// Thêm sản phẩm vào cart



    }
}
