using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Primjer 1 — LOŠE: lozinka je upisana direktno u kod (CWE-259)
var connectionString =
    "Server=prod-db;Database=Shop;User Id=sa;Password=SuperTajna123;";
using var conn = new SqlConnection(connectionString);

// Primjer 1b — isti propust, ali tajna se dodjeljuje propertyju ConnectionString
var connectionString2 =
    "Server=prod-db;Database=Shop;User Id=sa;Password=SuperTajna123;";
using var conn2 = new SqlConnection();
conn2.ConnectionString = connectionString2;

// Primjer 2 — LOŠE: API ključ je upisan direktno u kod (CWE-798)
var apiKey = "sk_live_51H8xExampleKey123";
builder.Services.AddHttpClient("PaymentApi", client =>
{
    client.BaseAddress = new Uri("https://api.payment.example/");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
});

// Primjer 3 — LOŠE: ključ za potpisivanje JWT-a je upisan direktno u kod (CWE-321)
var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes("moj-super-tajni-kljuc-za-jwt-1234567890"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = signingKey
    });

var app = builder.Build();
app.MapGet("/", () => "LosKod");
app.Run();