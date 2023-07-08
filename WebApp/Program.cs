using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Plugins.DataStore.InMemory;
using Plugins.DataStore.SQL;
using UseCases.CategoriesUseCase;
using UseCases.DataStorePluginInterfaces;
using UseCases.ProductsUseCase;
using UseCases.TransactionsUseCase;
using UseCases.UseCaseInterfaces;
using WebApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using ASP.NETCoreIdentity.Areas.Identity.Data;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();

            //Add EF Core
            builder.Services.AddDbContext<MarketContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AccountContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AccountContextConnection"));
            });




            //  this should be my reference on finding that fucking code!
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false) // must be true
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AccountContext>();







            // Adding roles
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", p => p.RequireClaim("Position", "Admin"));
                options.AddPolicy("CashierOnly", p => p.RequireClaim("Position", "Cashier"));
            });

            // DI for In-Memory Data Store
            //builder.Services.AddScoped<ICategoryRepository, CategoryInMemoryRepository>();
            //builder.Services.AddScoped<IProductRepository, ProductInMemoryRepository>();
            //builder.Services.AddScoped<ITransactionRepository, TransactionInMemoryRepository>();

            // DI for ef core Data Store for SQL
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            // DI for Use Cases and Repositories
            builder.Services.AddTransient<IViewCategoriesUseCase, ViewCategoriesUseCase>();
            builder.Services.AddTransient<IAddCategoryUseCase, AddCategoryUseCase>();
            builder.Services.AddTransient<IEditCategoryUseCase, EditCategoryUseCase>();
            builder.Services.AddTransient<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
            builder.Services.AddTransient<IDeleteCategoryUseCase, DeleteCategoryUseCase>();

            builder.Services.AddTransient<IViewProductsUseCase, ViewProductsUseCase>();
            builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
            builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();
            builder.Services.AddTransient<IGetProductByIdUseCase, GetProductByIdUseCase>();
            builder.Services.AddTransient<IDeleteProductUseCase, DeleteProductUseCase>();

            builder.Services.AddTransient<IViewProductsByCategoryId, ViewProductsByCategoryId>();
            builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();

            builder.Services.AddTransient<IRecordTransactionUseCase, RecordTransactionUseCase>();
            builder.Services.AddTransient<IGetTodayTransactionsUseCase, GetTodayTransactionsUseCase>();
            builder.Services.AddTransient<IGetTransactionsUseCase, GetTransactionsUseCase>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");


            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Cashier" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                    
                }

            }

            using (var scope = app.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string email = "admin@admin.com";
                string password = "Test09125@";

                if(await userManger.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser();
                    user.UserName = email;
                    user.Email = email;
                    //user.EmailConfirmed = true;

                    await userManger.CreateAsync(user, password);

                    userManger.AddToRoleAsync(user, "Admin");

                }

            }


            app.Run();
        }
    }
}