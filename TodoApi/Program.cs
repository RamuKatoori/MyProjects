using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
var builder = WebApplication.CreateBuilder(args);

// Add DbContext (SQLite)
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=todos.db"));

// Add controllers and swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto-create the database (safe for local dev)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.EnsureCreated(); // creates DB and schema from model without migrations
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
