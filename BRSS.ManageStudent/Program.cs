using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Application.Service;
using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository;
using BRSS.ManageStudent.Infrastructure.Config;
using BRSS.ManageStudent.Infrastructure.Data;
using BRSS.ManageStudent.Infrastructure.Repository;
using BRSS.ManageStudent.Infrastructure.UnitOfWork;
using BRSS.ManageStudent.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Handler bad request response
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values.SelectMany(v => v.Errors);
        return new BadRequestObjectResult(new BaseException
        {
            ErrorCode = StatusCodes.Status400BadRequest,
            DevMessage = "Lỗi nhập từ người dùng",
            UserMessage = "Lỗi nhập từ người dùng",
            TraceId = "",
            MoreInfo = "",
            Errors = errors
        }.ToString() ?? "");
    };
}).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

//Add DB connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Configure Identity user
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Configure Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("BRSSApp", policy =>policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyMethod()                     
        .AllowAnyHeader()                  
        .AllowCredentials()
    );
});

//Add Scope
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//
builder.Services.Configure<GoogleOAuthConfig>(builder.Configuration.GetSection("OAuth2:Google"));

// Register AutoMapper with all assemblies
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Use Cors
app.UseCors("BRSSApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

//Use MiddleWare Exception
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
