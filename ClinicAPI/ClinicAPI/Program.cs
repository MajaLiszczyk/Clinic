using ClinicAPI.DB;
using ClinicAPI.Repositories;
using ClinicAPI.Repositories.Interfaces;
using ClinicAPI.Services;
using ClinicAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


// Add services to the container.

builder.Services.AddControllers();



//MOJE POCZATEK
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer(); //dodalam - swagger

builder.Services.AddSwaggerGen(); //dodalam - swagger
/*builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard authorization header using the Bearer scheme (\"bearer {token}\").",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});*/

// Add services to the container.
/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();

        });
});*/
//MOJE KONIEC
builder.Services.AddScoped<IRegistrantService, RegistrantService>(); //dodalam
builder.Services.AddScoped<IRegistrantRepository, RegistrantRepository>(); //dodalam
builder.Services.AddScoped<IPatientService, PatientService>(); //dodalam
builder.Services.AddScoped<IPatientRepository, PatientRepository>(); //dodalam

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
//MOJE POCZATEK
app.UseCors(MyAllowSpecificOrigins);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"); // wskazuje na dokumentacj� API
    });
} 
//MOJE KONIEC

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
