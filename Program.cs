using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myfirstrestapi.Data;
using myfirstrestapi.IServices;
using myfirstrestapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDBcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

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
        ValidIssuer = "http://localhost:5115",
        ValidAudience = "http://localhost:5115",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("QtGin2leKfRhbQlvv9JzF9krMMSYIIy13PaRhN0tBc8"))
    };
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeService, EmployeService>();
var app = builder.Build();        
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



