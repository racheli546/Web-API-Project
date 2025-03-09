using ProjectCore.Services;
using ProjectCore.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProjectCore.Middlewares;
using Serilog;
using Serilog.Events;
using ProjectCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// 📌 קביעת פורטים מותאמים אישית ל-Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // ✅ HTTP על פורט 5000
    options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // ✅ HTTPS על פורט 5001
});

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key), // 📌 לוודא שהמפתח באורך נכון
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.DateFormatPath(
        pathFormat: "Logs/log-{date:format=yyyy-MM-dd}.txt"
    )
    .CreateLogger();
builder.Host.UseSerilog();


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KsPizza", Version = "v1" });
});

builder.Services.AddSingleton<IBakeryService, BakeryService>();

builder.Services.AddBakeryService();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireRole("User", "Admin"));
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// ✅ טיפול גלובלי בשגיאות
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ שגיאה גלובלית: {ex.GetType().Name} - {ex.Message}");
        Console.WriteLine($"🔍 פרטי שגיאה: {ex.StackTrace}");
        
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseLoggerMiddleware();
app.UseErrorHandlingMiddleware();

/*js (remove "launchUrl" from Properties\launchSettings.json*/
app.UseHttpsRedirection();




app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.MapFallbackToFile("login.html");
app.Run();



