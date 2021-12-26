using AspNetCore.Identity.Mongo;
using BlazorServerTest.Data;
using BlazorServerTest.Data.Database;
using BlazorServerTest.Data.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole, Guid>(
    identity => {
        identity.Password.RequireDigit = false;
        identity.Password.RequiredLength = 8;
        identity.Password.RequireNonAlphanumeric = false;
        identity.Password.RequireUppercase = false;
        identity.Password.RequireLowercase = false;

        // Lockout settings
        identity.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        identity.Lockout.MaxFailedAccessAttempts = 10;

        // ApplicationUser settings
        identity.User.RequireUniqueEmail = true;
        identity.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
    },
    mongo => {
        mongo.ConnectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");
        mongo.UsersCollection = "user_info";
    })
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddSignalR();
builder.Services.AddMvc();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UserContext>();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddScoped<IAppUserManager, ApplicationUserManager>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment() ) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

app.Run();
