using AuthorizationServer;
using AuthorizationServer.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/account/login";
            options.ExpireTimeSpan = TimeSpan.FromDays(1);
        });

builder.Services.AddDbContext<AuthServerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connString"));
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict().AddCore(options =>
{
    options.UseEntityFrameworkCore().UseDbContext<AuthServerDbContext>();
}).AddServer(options =>
{
    options.AllowClientCredentialsFlow();
    options.SetTokenEndpointUris("/connect/token");
    options.AddEphemeralEncryptionKey().AddEphemeralSigningKey();
    options.RegisterScopes("api");
    options.UseAspNetCore().EnableTokenEndpointPassthrough();
});

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
app.UseAuthentication();


app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
