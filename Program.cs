<<<<<<< HEAD
using BikeRental.Endpoints;
using Scalar.AspNetCore;
using BikeRental.Infrastructure.Data;
using BikeRental.Services;
using BikeRental.Services.Interfaces;
using FluentValidation;
=======

using BikeRental.Infrastructure.Data;
>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// 1. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// 2. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 3. DI Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddSingleton<IDynamicPricingService, DynamicPricingService>();

// 4. FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 6. Config JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
=======
// Add services to the container.
builder.Services.AddControllers();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("BikeRentalDb") // Use InMemory for demo to avoid SQL Server dependency
);

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing.");
var jwtDuration = int.Parse(builder.Configuration["Jwt:DurationInMinutes"] ?? "60");
>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
<<<<<<< HEAD
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

builder.Services.AddAuthorization();

// 7. OpenAPI / Scalar
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowAll");

=======
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

<<<<<<< HEAD
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { Message = "An unexpected error occurred." });
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => {
        options.WithTitle("Bike Rental API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// 8. Map Endpoints
app.MapGroup("/api")
   .MapAuthEndpoints()
   .MapBikeEndpoints()
   .MapRentalEndpoints()
   .MapWalletEndpoints();
=======
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Add this BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllers();
>>>>>>> 9d1ea28f7fb269cff41e45279fc921029eb77566

app.Run();