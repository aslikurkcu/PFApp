using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PFApp.Contexts;
using PFApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<BillsContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceCS")));

builder.Services.AddDbContext<ExpensesContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceCS")));


builder.Services.AddDbContext<InvestmentsContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceCS")));

builder.Services.AddDbContext<UsersContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("FinanceCS")));

builder.Services.AddIdentity<User,Role>().AddEntityFrameworkStores<UsersContext>()
.AddSignInManager<SignInManager<User>>();
/*builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;


});*/

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Appsettings:Secret").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200","https://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

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

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
