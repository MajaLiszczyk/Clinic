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


//using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text; // Encoding.UTF8
using Microsoft.IdentityModel.Tokens; // SymmetricSecurityKey, SigningCredentials, SecurityAlgorithms
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer; // JwtSecurityToken, JwtSecurityTokenHandler

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddControllers();



//MOJE POCZATEK
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); //Defaultconnection jest w appsettings.json?
builder.Services.AddEndpointsApiExplorer(); //dodalam - swagger. TEZ FULLSTACK
//builder.Services.AddSwaggerGen(); //dodalam - swagger
builder.Services.AddSwaggerGen(c => //FULLSTACK. w dzienniku elektr. tego nie ma, ale jest bardziej rozbudowane w serwisie komp
{
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        /*Type = SecuritySchemeType.Http,
        Scheme = "Bearer"*/

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
                //Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
            },
            new List<string>() // Prawid³owe wype³nienie wymaganych uprawnieñ
            //[]
        }
    });
});


//builder.Services.AddAuthorization();// YOUTUBE ANG
//builder.Services.AddAuthentication(); // YOUTUBE ANG



/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
}); */


//builder.Services.AddAuthentication("Bearer")
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
            //ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidIssuer = "your_issuer",
            //ValidAudience = builder.Configuration["JWT:Audience"],
            ValidAudience = "your_audience",
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!))
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsA32CharacterLongSecretKey!"))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Wyœwietl wiêcej szczegó³ów o b³êdzie
                Console.WriteLine($"Token validation failed: {context.Error}");
                Console.WriteLine($"Error description: {context.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
    });





builder.Services.AddAuthorizationBuilder();// FULLSTACK. TAK BY£O
//builder.Services.AddAuthorizationBuilder().
//                 AddPolicy("HasTestString", builder => builder.RequireClaim("TestString"));//ZAMIAST TEGO CO POWY¯EJ.
                                                                                           //DO MOJEGO CLAIM, POLICY. CHYBA WYWALIÆ
                                                                                           //WYMAGA ¯EBY U¯YTKOWNIK MIA£ CAIL O NAZWIE TESTSTRING
                                                                                           //U¿ytkownik musi posiadaæ claim o typie "TestString"
                                                                                           //(klucz) w swoim ClaimsPrincipal, aby przejœæ
                                                                                           //autoryzacjê.


//builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme); // FULLSTACK. Dziêki temu mozna bêdzie dodawaæ endpointy wymagajace uwierzytelnienea


/*builder.Services.AddIdentityCore<User>() //PAN ANG I FULLSTACK TO SAMO
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddApiEndpoints();*/ // AddApiEndpoints konfiguruje wymagane serwisy dla identity endpoints, ktore zosta³y dodane w .net 8

builder.Services.AddIdentityApiEndpoints<User>()//PAN ANG I FULLSTACK TO SAMO
                 .AddRoles<IdentityRole>()
                 .AddClaimsPrincipalFactory<ClinicUserClaimsPrincipalFactory>()  //DO DO MOJEGO CLAIMA. CHYBA WYWALÊ
                 //.AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<ApplicationDBContext>();

var applicationAssembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly)); //

//CZY POWINNAM??? :
// Rejestracja AutoMapper
//builder.Services.AddAutoMapper(applicationAssembly);
// Rejestracja FluentValidation
/*builder.Services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation(); */




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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
 
builder.Services.AddScoped<IDiagnosticTestService, DiagnosticTestService>(); //dodalam
builder.Services.AddScoped<IDiagnosticTestRepository, DiagnosticTestRepository>(); //dodalam

builder.Services.AddScoped<IDoctorService, DoctorService>(); //dodalam
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); //dodalam

builder.Services.AddScoped<ILaboratorySupervisorService, LaboratorySupervisorService>(); //dodalam
builder.Services.AddScoped<ILaboratorySupervisorRepository, LaboratorySupervisorRepository>(); //dodalam

builder.Services.AddScoped<ILaboratoryTestService, LaboratoryTestService>(); //dodalam
builder.Services.AddScoped<ILaboratoryTestRepository, LaboratoryTestRepository>(); //dodalam

builder.Services.AddScoped<ILaboratoryWorkerService, LaboratoryWorkerService>(); //dodalam
builder.Services.AddScoped<ILaboratoryWorkerRepository, LaboratoryWorkerRepository>(); //dodalam

builder.Services.AddScoped<IMedicalAppointmentService, MedicalAppointmentService>(); //dodalam
builder.Services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>(); //dodalam

builder.Services.AddScoped<IPatientService, PatientService>(); //dodalam
builder.Services.AddScoped<IPatientRepository, PatientRepository>(); //dodalam

builder.Services.AddScoped<IRegistrantService, RegistrantService>(); //dodalam
builder.Services.AddScoped<IRegistrantRepository, RegistrantRepository>(); //dodalam

builder.Services.AddScoped<IMedicalSpecialisationService, MedicalSpecialisationService>(); //dodalam
builder.Services.AddScoped<IMedicalSpecialisationRepository, MedicalSpecialisationRepository>(); //dodalam

builder.Services.AddScoped<IDiagnosticTestTypeService, DiagnosticTestTypeService>(); //dodalam
builder.Services.AddScoped<IDiagnosticTestTypeRepository, DiagnosticTestTypeRepository>(); //dodalam

builder.Services.AddScoped<IMedicalAppointmentDiagnosticTestService, MedicalAppointmentDiagnosticTestService>(); //dodalam

builder.Services.AddScoped<IUserContext, UserContext>(); //dodalam
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClinicSeeder, ClinicSeeder>();
//1h 15



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                      });
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IClinicSeeder>();
await seeder.Seed();











// Configure the HTTP request pipeline.
//MOJE POCZATEK
app.UseCors(MyAllowSpecificOrigins);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"); // wskazuje na dokumentacjê API
    });
} 
//MOJE KONIEC

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Path: {context.Request.Path}");
    Console.WriteLine($"Authorization: {context.Request.Headers["Authorization"]}");
    await next.Invoke();
});
app.UseAuthentication();
app.UseAuthorization(); //pierwszy krok do umo¿liwienia autentykacji


app.MapGroup("api/identity").MapIdentityApi<User>(); //ODANE 29.12 O 13.09 ANGL I FULLSTACK. Powinny juz byc dostêpne endopinty do rejestracji, logowania tp

app.MapGet("/test", (ClaimsPrincipal user) => $"Hello{user.Identity!.Name}").RequireAuthorization();

app.MapControllers();



app.Run();
