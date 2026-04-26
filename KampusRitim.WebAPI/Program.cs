using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Infrastructure;
using KampusRitim.Infrastructure.Context;
using KampusRitim.Infrastructure.Persistence;
using KampusRitim.Infrastructure.Persistence.Repositories;
using KampusRitim.Infrastructure.Repositories;
using KampusRitim.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Swagger Security ayarlar� i�in gerekli
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. VER�TABANI BA�LANTISI
// ---------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ---------------------------------------------------------
// 2. TEMEL SERV�SLER (Infrastructure & Utilities)
// ---------------------------------------------------------
builder.Services.AddHttpContextAccessor(); // Token okumak i�in �art
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAIRecommendationService, OpenAIRecommendationService>();
builder.Services.AddScoped<IChatbotService, OpenAIChatbotService>();
builder.Services.AddScoped<IChatDataQueryService, ChatDataQueryService>();


// ---------------------------------------------------------
// 3. REPOSITORIES (Veritaban� Eri�im Katman�)
// ---------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IUserClubRepository, UserClubRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserEventRepository, UserEventRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IVoteRepository, VoteRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

builder.Services.AddHttpClient();
// ---------------------------------------------------------
// 4. MEDIATR (Application Katman� - Handler'lar)
// ---------------------------------------------------------
// Application katman�ndaki herhangi bir Handler'� referans g�stererek
// o katmandaki T�M handler'lar�n otomatik bulunmas�n� sa�l�yoruz.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(KampusRitim.Application.UseCases.Auth.Register.RegisterRequestHandler).Assembly));


// ---------------------------------------------------------
// 5. CONTROLLERS & JSON AYARLARI
// ---------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'lar�n (1, 2 gibi say� de�il) "BirinciSinif" gibi yaz� olarak g�r�nmesini sa�lar
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ---------------------------------------------------------
// 6. SWAGGER AYARLARI (JWT Destekli)
// ---------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "KampusRitim API", Version = "v1" });

    // --- DE����KL�K BURADA: Type = Http yapt�k ---
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "L�tfen sadece Token'� giriniz (Bearer yazman�za gerek yok).",
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // ApiKey yerine Http
        BearerFormat = "JWT",
        Scheme = "bearer" // K���k harfle
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ---------------------------------------------------------
// 7. AUTHENTICATION (Kimlik Do�rulama - JWT)
// ---------------------------------------------------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

// ---------------------------------------------------------
// 8. CORS (Frontend Eri�imi)
// ---------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("Authorization");
        });
});

// =========================================================
// BUILD & PIPELINE
// =========================================================
var app = builder.Build();

// Development ortam�nda Swagger'� a�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS middleware'i Auth'tan �nce olmal�
app.UseCors("AllowReactApp");

// Kimlik Do�rulama ve Yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();