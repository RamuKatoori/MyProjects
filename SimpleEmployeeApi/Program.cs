var builder = WebApplication.CreateBuilder(args);

// Configuration example (reads appsettings.json automatically)
var config = builder.Configuration;

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core InMemory (for demo)
builder.Services.AddDbContext<SimpleEmployeeApi.Data.AppDbContext>(options =>
    options.UseInMemoryDatabase("EmployeeDb"));

// Register application service (DI)
builder.Services.AddScoped<SimpleEmployeeApi.Services.IEmployeeService,
                           SimpleEmployeeApi.Services.EmployeeService>();

// Optional: configure CORS for local frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalAngular", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error"); // production-friendly error page route
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("LocalAngular");

app.UseAuthorization();

app.MapControllers();

// Seed some demo data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SimpleEmployeeApi.Data.AppDbContext>();
    db.Employees.AddRange(new[]
    {
        new SimpleEmployeeApi.Models.Employee { Name = "Alice", Email = "alice@example.com", Role = "Developer" },
        new SimpleEmployeeApi.Models.Employee { Name = "Bob", Email = "bob@example.com", Role = "QA" }
    });
    db.SaveChanges();
}

app.Run();
