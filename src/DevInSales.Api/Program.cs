using System.Text;
using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using DevInSales.Core.Interfaces.Identity;
using DevInSales.Core.Services;
using DevInSales.Identity.Configurations;
using DevInSales.Identity.Data.Context;
using DevInSales.Identity.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtOptions:SecurityKey")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region Identity e JWT

builder.Services.AddDbContext<IdentityDataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<IdentityDataContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<JwtOptions>(options =>
{
    options.Issuer = builder.Configuration.GetValue<string>("JwtOptions:Issuer");
    options.Audience = builder.Configuration.GetValue<string>("JwtOptions:Audience");
    options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
    options.AccessTokenExpiration = int.Parse(builder.Configuration.GetValue<string>("JwtOptions:AccessTokenExpiration") ?? "0");
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
});

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration.GetValue<string>("JwtOptions:Issuer"),

    ValidateAudience = true,
    ValidAudience = builder.Configuration.GetValue<string>("JwtOptions:Audience"),

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = securityKey,

    RequireExpirationTime = true,
    ValidateLifetime = true,

    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
});

#endregion

builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ISaleProductService, SaleProductService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IIdentityInitializer, IdentityInitializer>();
builder.Services.AddScoped<IDataInitializer, DataInitializer>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevInSales API",
        Version = "v1",
        Description = "Projeto 1 do módulo 3 do curso DevInHouse da turma WPP",
        Contact = new OpenApiContact
        {
            Name = "Turma WPP",
            Url = new Uri("https://github.com/DEVin-Way2-Pixeon-Paradigma/M2P2-DEVinSales")
        }
    });
    var xmlFile = "DevInSales.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                            Enter 'Bearer' [space] and then your token in the text input below. 
                            Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{

    var populandoIdentity = scope.ServiceProvider.GetService<IIdentityInitializer>();
    await populandoIdentity.Initializer();

    var populandoData = scope.ServiceProvider.GetService<IDataInitializer>();
    await populandoData.Initializer();

}

await app.RunAsync();
