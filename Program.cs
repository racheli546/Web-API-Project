var builder = WebApplication.CreateBuilder(args);

// הוסף את שירותי CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")  // הכתובת שצריכה גישה
              .AllowAnyMethod()  // מאפשר כל סוג של שיטה (GET, POST וכו')
              .AllowAnyHeader(); // מאפשר כל כותרת
    });
});

// הוסף את שאר השירותים
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IBakeryService, BakeryService>();

var app = builder.Build();  // בניית האפליקציה

// אם הממשק מפותח, הצג את Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// הפעלת CORS (הוסף את השורה הזו)
app.UseCors("AllowLocalhost");  // זה מאוד חשוב! 

// שאר הגדרות ה-API
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();  // הרץ את השרת
