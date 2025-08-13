
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Blky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration
        .GetConnectionString("ConnectionString");

builder.Services.AddDbContext<ApplicationDbContext>(o =>
  o.UseSqlServer(connectionString)
);


builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("Stripe"));








builder.Services.AddIdentity<IdentityUser,IdentityRole>(
    options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});




builder.Services.AddRazorPages();

      

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();





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



 
//STRIPE_SECRET_KEY
var stripeSecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
builder.Services.AddSingleton<IStripeClient>(new StripeClient(stripeSecretKey));

StripeConfiguration.ApiKey = builder
       .Configuration.GetSection("Stripe:SecretKey").Get<string>();



app.UseRouting();
app.UseAuthentication();    
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
