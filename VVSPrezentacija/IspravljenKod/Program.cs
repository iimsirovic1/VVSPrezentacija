using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Primjer 1 — ISPRAVNO: vrijednost dolazi iz konfiguracije, a kod ne zna lozinku
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' nije pronađen ili je prazan.");
using var conn = new SqlConnection(connectionString);

// Primjer 2 — ISPRAVNO: ključ dolazi iz konfiguracije
var apiKey = builder.Configuration["PaymentApi:ApiKey"];
if (string.IsNullOrWhiteSpace(apiKey))
    throw new InvalidOperationException(
        "API ključ 'PaymentApi:ApiKey' nije pronađen ili je prazan.");
builder.Services.AddHttpClient("PaymentApi", client =>
{
    client.BaseAddress = new Uri("https://api.payment.example/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
});

// Primjer 3 — ISPRAVNO: ključ dolazi iz konfiguracije
var jwtKey = builder.Configuration["Jwt:SigningKey"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException(
        "JWT ključ 'Jwt:SigningKey' nije pronađen ili je prazan.");
var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtKey));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = signingKey
    });

var app = builder.Build();
app.MapGet("/", () => "IspravljenKod");
app.Run();