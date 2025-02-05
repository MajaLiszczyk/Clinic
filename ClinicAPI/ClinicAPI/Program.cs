using ClinicAPI.Authorization;
using ClinicAPI.DB;
using ClinicAPI.Models;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Seeders;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using ClinicAPI.UserFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text; // Encoding.UTF8
using Microsoft.IdentityModel.Tokens; // SymmetricSecurityKey, SigningCredentials, SecurityAlgorithms
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer; // JwtSecurityToken, JwtSecurityTokenHandler

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); //Defaultconnection jest w appsettings.json?
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new List<string>() 
        }
    });
});

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
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsA32CharacterLongSecretKey!"))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                Console.WriteLine($"Token validation failed: {context.Error}");
                Console.WriteLine($"Error description: {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityApiEndpoints<User>()
                 .AddRoles<IdentityRole>()
                 .AddClaimsPrincipalFactory<ClinicUserClaimsPrincipalFactory>() 
                .AddEntityFrameworkStores<ApplicationDBContext>();

var applicationAssembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly)); 

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
 
builder.Services.AddScoped<IDiagnosticTestService, DiagnosticTestService>(); 
builder.Services.AddScoped<IDiagnosticTestRepository, DiagnosticTestRepository>(); 

builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); 

builder.Services.AddScoped<ILaboratorySupervisorService, LaboratorySupervisorService>();
builder.Services.AddScoped<ILaboratorySupervisorRepository, LaboratorySupervisorRepository>();

builder.Services.AddScoped<ILaboratoryTestService, LaboratoryTestService>(); 
builder.Services.AddScoped<ILaboratoryTestRepository, LaboratoryTestRepository>(); 

builder.Services.AddScoped<ILaboratoryAppointmentService, LaboratoryAppointmentService>(); 
builder.Services.AddScoped<ILaboratoryAppointmentRepository, LaboratoryAppointmentRepository>(); 

builder.Services.AddScoped<ILaboratoryTestTypeService, LaboratoryTestTypeService>(); 
builder.Services.AddScoped<ILaboratoryTestTypeRepository, LaboratoryTestTypeRepository>(); 

builder.Services.AddScoped<ILaboratoryTestsGroupService, LaboratoryTestsGroupService>(); 
builder.Services.AddScoped<ILaboratoryTestsGroupRepository, LaboratoryTestsGroupRepository>(); 

builder.Services.AddScoped<ILaboratoryWorkerService, LaboratoryWorkerService>(); 
builder.Services.AddScoped<ILaboratoryWorkerRepository, LaboratoryWorkerRepository>(); 

builder.Services.AddScoped<IMedicalAppointmentService, MedicalAppointmentService>(); 
builder.Services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>(); 

builder.Services.AddScoped<IRegistrantService, RegistrantService>();
builder.Services.AddScoped<IRegistrantRepository, RegistrantRepository>(); 

builder.Services.AddScoped<IMedicalSpecialisationService, MedicalSpecialisationService>();
builder.Services.AddScoped<IMedicalSpecialisationRepository, MedicalSpecialisationRepository>(); 

builder.Services.AddScoped<IDiagnosticTestTypeService, DiagnosticTestTypeService>(); 
builder.Services.AddScoped<IDiagnosticTestTypeRepository, DiagnosticTestTypeRepository>(); 

builder.Services.AddScoped<IMedicalAppointmentDiagnosticTestService, MedicalAppointmentDiagnosticTestService>(); 

builder.Services.AddScoped<IUserContext, UserContext>(); 
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClinicSeeder, ClinicSeeder>();

//Sprawdza has³o
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 3; 
    options.Password.RequireUppercase = true; 
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

/*if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
{
    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    Console.WriteLine("Przestawiono");

}*/
var app = builder.Build();
Console.WriteLine($"Current Environment: {app.Environment.EnvironmentName}");

/*app.UseSwagger();
Console.WriteLine("Swagger middleware initialized.");

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    c.RoutePrefix = string.Empty;
    Console.WriteLine("Swagger UI initialized.");
}); */


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.Urls.Add("https://localhost:5001");
app.Urls.Add("http://localhost:5000");

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IClinicSeeder>();
await seeder.Seed();

app.UseCors(MyAllowSpecificOrigins);
Console.WriteLine(app.Environment.IsDevelopment());


/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"); // wskazuje na dokumentacjê API
        c.RoutePrefix = string.Empty;  // To sprawi, ¿e Swagger UI bêdzie dostêpne pod https://localhost:5001/
        Console.WriteLine("Swagger UI initialized");
    });
} */

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Path: {context.Request.Path}");
    Console.WriteLine($"Authorization: {context.Request.Headers["Authorization"]}");
    await next.Invoke();
});
app.UseAuthentication();
app.UseAuthorization(); 

app.MapGroup("api/identity").MapIdentityApi<User>();

app.MapGet("/test", (ClaimsPrincipal user) => $"Hello{user.Identity!.Name}").RequireAuthorization();

app.MapControllers();

app.Run();