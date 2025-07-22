using Amazon.S3;
using ICEDT_TamilApp.Application.Common;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This is for Razor Pages
builder.Services.AddRazorPages();

// This is for API Controllers
builder.Services.AddControllers();

// ... DI registration, DbContext, JWT Auth, etc.
// To-do

// *** NEW: Configure the Options Pattern ***
var jwtSettings = new JwtSettings();
builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);
// Make the settings available via DI using IOptions<T>
builder.Services.AddSingleton(Options.Create(jwtSettings));

// *** NEW: Configure AWS Settings and S3 Client for DI ***

// 1. Bind the AwsSettings class using the Options Pattern
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection(AwsSettings.SectionName));

// 2. Add the default AWS options from the configuration
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// 3. Register the IAmazonS3 client. The SDK will automatically use the
//    credentials and region from the AWSOptions.
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Serves your wwwroot folder (JS, CSS)

app.UseRouting();

// Auth must come after Routing but before Authorization and Endpoints
app.UseAuthentication();
app.UseAuthorization();

// This maps your API controllers (e.g., /api/levels)
app.MapControllers();

// This maps your Razor Pages (e.g., /Admin/Levels)
app.MapRazorPages();


app.Run();
