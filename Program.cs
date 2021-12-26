using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using BlazorServerTest.Data;
using BlazorServerTest.Data.Database;
using BlazorServerTest.Data.Users;

var builder = WebApplication.CreateBuilder(args);

var mongoDbIdentity = new MongoDbIdentityConfiguration {
    MongoDbSettings = new MongoDbSettings {
        ConnectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString"),
        DatabaseName = "UserDatabase"
    },
    IdentityOptionsAction = options => {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;

        // ApplicationUser settings
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
    }
};

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IAppUserManager, ApplicationUserManager>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureMongoDbIdentity<ApplicationUser, ApplicationRole, Guid>(mongoDbIdentity);


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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
