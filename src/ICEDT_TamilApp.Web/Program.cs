using Amazon.S3;
using ICEDT_TamilApp.Application;
using ICEDT_TamilApp.Application.Common;
using ICEDT_TamilApp.Infrastructure;
using ICEDT_TamilApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// =================================================================
// 1. Configure Services by calling Extension Methods
// =================================================================

// Add services from the Application layer
builder.Services.AddApplicationServices();

// Add services from the Infrastructure layer
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.

// This is for API Controllers
builder.Services.AddControllers();

// This is for Razor Pages
builder.Services.AddRazorPages();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Staging"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // This makes the swagger page available at the root URL (e.g., http://your-ip/swagger)
        // instead of the default which might be buried under a subpath.
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ICEDT TamilApp API V1");
        // To make Swagger the default page in these environments, you can do this:
        // options.RoutePrefix = string.Empty; 
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// =================================================================
// 3. SEED THE DATABASE (Crucial Step)
// =================================================================
// This block will create a scope, get the DbContext, and run your seeder.
// It's wrapped in a try-catch to log any errors during startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Ensure migrations are applied (best practice for production)
        await context.Database.MigrateAsync();

        // Call your DbInitializer to seed the data
        await DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        // Get a logger and log the error
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization/seeding.");
    }
}

// =================================================================

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
