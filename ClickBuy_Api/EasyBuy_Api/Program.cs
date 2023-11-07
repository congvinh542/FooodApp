using ClickBuy_Api.Database.Data;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.Service.Extensions;
using ClickBuy_Api.Service.Services.PermissionServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ClickBuy API", Version = "v1" });
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Add a security definition for bearer token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Add a security requirement to use bearer token in Swagger requests
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});
builder.Services.AddServices(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);

// Add CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

#region Seed Data And Migrate
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ClickBuyDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    var permissionService = services.GetRequiredService<IPermissionService>();
    context.Database.Migrate();

    // Seed data
    if (await Seed.SeedImage(context) <= 0)
    {
        Console.WriteLine("No Images seeded");
    }
    if (await Seed.SeedUser(userManager, roleManager, context) <= 0)
    {
        Console.WriteLine("No users seeded");
    }
    if (await Seed.SeedTinh(context) <= 0)
    {
        Console.WriteLine("No tinhs seeded");
    }
    if (await Seed.SeedHuyen(context) <= 0)
    {
        Console.WriteLine("No huyens seeded");
    }
    if (await Seed.SeedXa(context) <= 0)
    {
        Console.WriteLine("No xas seeded");
    }
    if (await Seed.SeedGender(context) <= 0)
    {
        Console.WriteLine("No Genders seeded");
    }
    if (await Seed.SeedCategory(context) <= 0)
    {
        Console.WriteLine("No Categorys seeded");
    }
    if (await Seed.SeedProduct(context) <= 0)
    {
        Console.WriteLine("No Products seeded");
    }
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}
#endregion

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials() // for signalR
);

app.UseAuthentication(); // Enable authentication
app.UseAuthorization();

app.MapControllers();

app.Run();