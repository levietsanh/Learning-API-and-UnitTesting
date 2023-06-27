using APILearning.Data;
using APILearning.Helpers;
using APILearning.Repositories.Implement;
using APILearning.Repositories.Interface;
using APILearning.Services.Implement;
using APILearning.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



//Add Connection
var connectionString = builder.Configuration.GetConnectionString("ProfileCon")
                       ?? throw new InvalidOperationException("Connection string 'Profile' not found");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

//Add Service Token
builder.Services.AddScoped<ITokenService, TokenService>();

//Add Service UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Add Service Account
builder.Services.AddScoped<IUserService, UserService>();

//Add Authencation
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
     options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
             ValidateIssuer = false,
             ValidateAudience = false,
         };
     }
    );


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
