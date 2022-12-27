using Microsoft.EntityFrameworkCore;
using VehicleRentalSystem.Data;
using VehicleRentalSystem.Data.Managers;
using VehicleRentalSystem.Models;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<AdminDataManager>();
builder.Services.AddScoped<CarDataManager>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<UserModel>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();



// Add ToastNotification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

//builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
//{
//    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
//    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
//});

/*builder.Services
    .AddControllersWithViews()
    .AddDataAnnotationsLocalization(x =>
    {
        x.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            return factory.Create(typeof(SharedResource));
        };
    })

    .AddViewLocalization();*/


/*builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en-US");
    options.AddSupportedUICultures("en-US", "lv-LV");
    options.FallBackToParentUICultures = true;

    options
        .RequestCultureProviders
        .Remove(typeof(AcceptLanguageHeaderRequestCultureProvider));
});

builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();
*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.UseNotyf();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();
