using HotelGuru.DataContext.Context;
using HotelGuru.DataContext.Dtos;
using HotelGuru.DataContext.Entities;
using HotelGuru.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) EF Core
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Application services
builder.Services.AddScoped<IHotelServices, HotelServices>();
builder.Services.AddScoped<IFoglalasService, FoglalasService>();
builder.Services.AddScoped<ISzobaService, SzobaService>();
builder.Services.AddScoped<IVendegService, VendegService>();
builder.Services.AddScoped<IRecepciosService, RecepciosService>();
builder.Services.AddScoped<IPluszSzolgaltatasService, PluszSzolgaltatasService>();
builder.Services.AddScoped<IAdminisztratorService, AdminisztratorService>();

// 3) Identity/PasswordHasher for Felhasznalo
builder.Services.AddScoped<IPasswordHasher<Felhasznalo>, PasswordHasher<Felhasznalo>>();


// 4) ValidateUser service
builder.Services.AddScoped<ValidateUser>();


// 5) Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelGuru API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Adj meg egy érvényes JWT tokent: Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 6) JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    })
    .AddNegotiate();   // ha on-prem AD integrációt is szeretnél

// 7) Authorization: minden végpont védett, kivéve ahol [AllowAnonymous]
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Felhasznalo>>();
if (!ctx.Felhasznalok.Any())
{
    var admin = new Adminisztrator
    {
        Felhasznalonev = "admin",
        TeljesNev = "Adminisztrátor"
    };
    admin.JelszoHash = hasher.HashPassword(admin, "123");
    ctx.Felhasznalok.Add(admin);
    Console.WriteLine("Létrehozva egy admin felhasználó (admin / 123).");
}



app.MapControllers();

app.Run();
