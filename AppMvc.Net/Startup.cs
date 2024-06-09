using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppMvc.Net.Data;
using AppMvc.Net.Menu;
using AppMvc.Net.Models;
using AppMvc.Net.Services;
using dotenv.net;



//using App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AppMvc.Net
{
    public class Startup
    {

        public static string ContentRootPath { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ContentRootPath = env.ContentRootPath;
            DotEnv.Load();
            var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables(); // This will add the environment variables

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {

                string connectString = Environment.GetEnvironmentVariable("AppMvcConnectionString");
                options.UseSqlServer(connectString);
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            // services.AddTransient(typeof(ILogger<>), typeof(Logger<>)); //Serilog
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // /View/Controller/Action.cshtml
                // /MyView/Controller/Action.cshtml

                // {0} -> ten Action
                // {1} -> ten Controller
                // {2} -> ten Area
                options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            //  services.AddSingleton<ProductService>();
            // services.AddSingleton<ProductService, ProductService>();
            // services.AddSingleton(typeof(ProductService));
            //services.AddSingleton(typeof(ProductService), typeof(ProductService));
            services.AddSingleton<PlanetService>();


            //dang ky identity
            services.AddIdentity<AppUser, IdentityRole>()
                                .AddEntityFrameworkStores<AppDbContext>()
                                .AddDefaultTokenProviders();            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;
                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login/";
                options.LogoutPath = "/logout/";
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });

            services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
                        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
                        // https://localhost:5001/signin-google
                        options.CallbackPath = "/dang-nhap-tu-google";
                    })

                    // .AddTwitter()
                    // .AddMicrosoftAccount()
                    ;
            services.AddOptions();
            var mailsetting = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsetting);
            services.AddSingleton<IEmailSender, SendMailService>();

            services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("ViewManageMenu", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleName.Administrator);
                });
            });



            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg =>
            {                    // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "appmvc";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 30, 0);    // Thời gian tồn tại của Session
            });

            services.AddTransient<CartService>();


            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<AdminSidebarService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Uploads")
                ),
                RequestPath = "/contents"
            });
            app.UseSession();

            app.UseStatusCodePages();//tuy bien trang khi loi
            app.UseRouting();        // EndpointRoutingMiddleware

            app.UseAuthentication(); // xac dinh danh tinh 
            app.UseAuthorization();  // xac thuc  quyen truy  cap

            app.UseEndpoints(endpoints =>
            {

                // URL: /{controller}/{action}/{id?}
                // First/Index

                // /sayhi
                endpoints.MapGet("/sayhi", async (context) =>
                {
                    await context.Response.WriteAsync($"Hello ASP.NET MVC {DateTime.Now}");
                });
                //endpoints.MapControllers();

                // endpoints.MapControllerRoute
                // endpoints.MapDefaultControllerRoute
                // endpoints.MapAreaControllerRoute


                //URL=> start-here
                // controller=>
                // action=>
                // area=>


                endpoints.MapControllerRoute(
                name: "first",
                 pattern: "{url:regex(^((xemsanpham)|(viewproduct))$)}/{id:range(2,4)}",//url=http://localhost:5268/start-here (id=3)|| http://localhost:5268/start-here/4 (id=4)
                 defaults: new
                 {
                     controller = "First",
                     action = "ViewProduct",

                 },
                 constraints: new
                 {
                     // url = new StringRouteConstraint("xemsanpham"),
                     //   url = new RegexRouteConstraint(@"^((xemsanpham)|(viewproduct))$"),//url =xemsanpham || viewproduct
                     //  id = new RangeRouteConstraint(2, 4)
                 }
                );




                // endpoints.MapControllerRoute(
                // name: "firstroute",
                //  pattern: "start-here/{controller=Home}/{action=Index}/{id?}"//url=http://localhost:5268/start-here (id=3)|| http://localhost:5268/start-here/4 (id=4)
                // //  defaults: new
                // //  {
                // //      controller = "First",
                // //      action = "ViewProduct",
                // //      id = 3
                // //  }
                // );

                //chi thuc hieen tren controller khong co area
                endpoints.MapControllerRoute(
                name: "default",
                 pattern: "/{controller=Home}/{action=Index}/{id?}"//url=http://localhost:5268/start-here (id=3)|| http://localhost:5268/start-here/4 (id=4)
                //  defaults: new
                //  {
                //      controller = "First",
                //      action = "ViewProduct",
                //      id = 3
                //  }
                );
                //dung cho area
                endpoints.MapAreaControllerRoute(
                    name: "default",
                 pattern: "/{controller}/{action=Index}/{id?}",
                 areaName: "ProductManager"
                );
                endpoints.MapAreaControllerRoute(
                     name: "Database",
                    areaName: "Database",
                     pattern: "/{controller=DbManage}/{action=Index}/{id?}");

            });
        }
    }
}