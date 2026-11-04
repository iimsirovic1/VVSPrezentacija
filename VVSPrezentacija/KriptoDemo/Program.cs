
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Pocetna stranica
app.MapGet("/", () => "KriptoDemo - MD5, PBKDF2, SHA-1, SHA-256, DES i AES-GCM");

// 1. MD5 - nesigurno
app.MapGet("/md5-demo", () =>
{
    LosKod.MD5Example.Run();
    return "MD5 demonstracija izvrsena. Pogledaj konzolu.";
});

// 2. PBKDF2 - sigurnija zamjena za MD5 kod lozinki
app.MapGet("/md5-fixed-demo", () =>
{
    IspravljenKod.MD5Fixed.Run();
    return "PBKDF2 demonstracija izvrsena. Pogledaj konzolu.";
});

// 3. SHA-1 - nesigurno za primjene koje zahtijevaju otpornost na kolizije
app.MapGet("/sha1-demo", () =>
{
    LosKod.SHA1Example.Run();
    return "SHA-1 demonstracija izvrsena. Pogledaj konzolu.";
});

// 4. SHA-256 - sigurnija zamjena za SHA-1
app.MapGet("/sha256-demo", () =>
{
    IspravljenKod.SHA256Fixed.Run();
    return "SHA-256 demonstracija izvrsena. Pogledaj konzolu.";
});

// 5. DES - nesigurno sifriranje
app.MapGet("/des-demo", () =>
{
    LosKod.DESExample.Run();
    return "DES demonstracija izvrsena. Pogledaj konzolu.";
});

// 6. AES-GCM - sigurnija zamjena za DES
app.MapGet("/aes-demo", () =>
{
    IspravljenKod.AESFixed.Run();
    return "AES-GCM demonstracija izvrsena. Pogledaj konzolu.";
});

app.Run();
