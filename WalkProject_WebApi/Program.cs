using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalkProject_WebApi.Data;
using WalkProject_WebApi.Mapping;
using WalkProject_WebApi.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Usinge Logger Service
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Errer.text",rollingInterval:RollingInterval.Minute)
    //.MinimumLevel.Information()
    //.MinimumLevel.Warning()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Add Service in Image 
builder.Services.AddHttpContextAccessor();

// jwt token attach swagger 
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Walk API", Version = "v1" });
    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
              new  OpenApiSecurityScheme{
               Reference=new OpenApiReference{
                Type=ReferenceType.SecurityScheme,
                Id=JwtBearerDefaults.AuthenticationScheme
               },
               Scheme="Oauth2",
               Name=JwtBearerDefaults.AuthenticationScheme,
               In=ParameterLocation.Header
              },new List<string>()
      }
    });
});

// Add Dp For ConnectionString
builder.Services.AddDbContext<WalkProjectDbContext>(option =>option.UseSqlServer(builder.Configuration.GetConnectionString("WalkConStr")));
// Add Dp For Auth Connection
builder.Services.AddDbContext<NZWalkAuthDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("AuthConStr")));

//Add Repository Region 
builder.Services.AddScoped<IRegionRepository,SqlRegionRepository>();

//Add Repository Walk
builder.Services.AddScoped<IWalkRepository,SqlWalkRepository>();

//Add Respository  Token
builder.Services.AddScoped<ITokenRespository, TokenRespository>();

// Add Repository Image
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

//Add AutoMapper Services
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//Add JWT Services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JWT:Issure"],
    ValidAudience = builder.Configuration["JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
});

// set up identity
builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZwalks")
    .AddEntityFrameworkStores<NZWalkAuthDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 1;
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

app.Run();
