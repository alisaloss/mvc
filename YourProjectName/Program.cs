using YourProjectName.Configuration;
using YourProjectName.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

string connectionString =
 builder.Configuration["MongoDb:ConnectionString"]
 ?? throw new InvalidOperationException(
 "MongoDb:ConnectionString has not been configured.");
string databaseName =
 builder.Configuration["MongoDb:DatabaseName"]
 ?? throw new InvalidOperationException(
 "MongoDb:DatabaseName has not been configured.");
string collectionName =
 builder.Configuration["MongoDb:CollectionName"]
 ?? throw new InvalidOperationException(
 "MongoDb:CollectionName has not been configured.");
MongoDbSettings mongoDbSettings = new()
{
    ConnectionString = connectionString,
    DatabaseName = databaseName,
    CollectionName = collectionName
};
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddSingleton<IMongoClient>(
 new MongoClient(mongoDbSettings.ConnectionString));

builder.Services.AddSingleton<MongoBlogPostService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
