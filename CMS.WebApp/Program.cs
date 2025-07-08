using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Users.Validators;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Authen;
using CMS.Utilities.Helpers;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using CMS.WebApp.Helper;
using CMS.Services.Supermarket.Interfaces;
using CMS.Services.Supermarket;

var builder = WebApplication.CreateBuilder(args);

#region Add DBContext
var dbType = builder.Configuration.GetConnectionString((ConstantHelper.DBTypeParamName).ToLower());
if (dbType.ToLower().Equals(ConstantHelper.SQLServer.ToLower()))
{
    string connectionString = builder.Configuration.GetConnectionString((ConstantHelper.SQLServerParamName.ToLower()));
    builder.Services.AddDbContext<AICMSDBContext>(options => { options.UseSqlServer(connectionString); }, ServiceLifetime.Transient);
}
else if (dbType.ToLower().Equals(ConstantHelper.MySQL.ToLower()))
{
    string connectionString = builder.Configuration.GetConnectionString((ConstantHelper.MySQLParamName.ToLower()));
    builder.Services.AddDbContext<AICMSDBContext>(options =>
    {
        options.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString),
            mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
            )
        );
    }, ServiceLifetime.Transient);
}
#endregion

#region Đăng ký Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AICMSDBContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Default SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Default User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
#endregion

builder.Services.AddHttpClient();

#region Add dich vu Authen
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/account/login/";
        options.LogoutPath = "/account/logout/";
        options.AccessDeniedPath = "/account/accessdenied/";
        options.ExpireTimeSpan = TimeSpan.FromDays(365);
        options.SlidingExpiration = false;
        //options.EventsType = typeof(ICSoft.Jobman.WebApp.Helper.CookieAuthEvent);
        /*options.Cookie.Name = "ICJCookie";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.Expiration = TimeSpan.FromMinutes(720);*/
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(365);
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.Cookie.Name = "ICJCookie3";
    options.ExpireTimeSpan = TimeSpan.FromDays(365);
    options.Cookie.MaxAge = TimeSpan.FromDays(365);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
})
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateProfileRequestValidator>());


#region Khai bao repositories
#region Authen
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<UserManager<User>, UserManager<User>>();
builder.Services.AddTransient<SignInManager<User>, SignInManager<User>>();
builder.Services.AddTransient<RoleManager<Role>, RoleManager<Role>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IRoleClaimService, RoleClaimService>();
builder.Services.AddTransient<IUserClaimService, UserClaimService>();
builder.Services.AddTransient<IGenderService, GenderService>();
builder.Services.AddTransient<IUserStatusService, UserStatusService>();
builder.Services.AddTransient<IFunctionService, FunctionService>();
builder.Services.AddTransient<IIconService, IconService>();
builder.Services.AddTransient<IUserLogService, UserLogService>();
#endregion
#region Supermarket

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IProductUnitService, ProductUnitService>();
builder.Services.AddTransient<ISupplierService,SupplierService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IStockImportService, StockImportService>();
builder.Services.AddTransient<IStockImportDetailService, StockImportDetailService>();
builder.Services.AddTransient<IHomeService, HomeService>();
#endregion
#endregion


#region Cau hinh de su dung format datetime dd/MM/yyyy
//Trong ham Configure can them dong: app.UseRequestLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("vi-VN");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("vi-VN") };
});
#endregion



#region Cau hinh cho phep F5 de nhan thay doi tren razor code
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (environment == Environments.Development)
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
#endregion

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = null;
    options.MaxRequestBodyBufferSize = int.MaxValue;
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = null;
    options.Limits.MaxRequestBufferSize = int.MaxValue;
    //options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueCountLimit = int.MaxValue;
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          //.AllowCredentials();
                      });
});

var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(cookiePolicyOptions);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

app.StartRecurringJobs();
app.Run();
