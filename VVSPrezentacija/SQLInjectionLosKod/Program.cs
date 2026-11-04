
using Microsoft.Data.SqlClient;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Nedostaje veza s bazom.");

var app = builder.Build();

// ==========================================
// PRIMJER 1 - SQL INJECTION U PRIJAVI
// CWE-89 - RANJIVA VERZIJA
// ==========================================

app.MapGet("/", () =>
{
    return Results.Content("""
        <h2>Prijava korisnika</h2>
        <form action="/login" method="post">
            <label>E-mail:</label>
            <input type="text" name="email" required>
            <br><br>
            <label>Lozinka:</label>
            <input type="password" name="password" required>
            <br><br>
            <button type="submit">Prijava</button>
        </form>
        <a href="/pretraga">Pretraga korisnika</a><br>
        <a href="/brisanje">Brisanje zapisa</a>
        """, "text/html; charset=utf-8");
});

app.MapPost("/login", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    string email = form["email"].ToString();
    string password = form["password"].ToString();

    // RANJIVO: direktno spajanje korisnickog unosa u SQL
    string sql = "SELECT TOP 1 Ime, Email FROM Korisnici " +
                 "WHERE Email = '" + email +
                 "' AND Lozinka = '" + password + "'";

    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new SqlCommand(sql, connection);
    await using var reader = await command.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
        string ime = HtmlEncoder.Default.Encode(
            reader["Ime"].ToString() ?? "");

        return Results.Content(
            $"<h2>Prijava uspješna!</h2><p>Dobrodošla, {ime}!</p>",
            "text/html; charset=utf-8");
    }

    return Results.Content(
        "<h2>Neispravan e-mail ili lozinka.</h2>",
        "text/html; charset=utf-8");
});

// ==========================================
// PRIMJER 2 - SQL INJECTION U PRETRAZI
// CWE-89 - RANJIVA VERZIJA
// ==========================================

app.MapGet("/pretraga", () =>
{
    return Results.Content("""
        <h2>Pretraga korisnika</h2>
        <form action="/pretraga" method="post">
            <label>Unesite ime:</label>
            <input type="text" name="ime" required>
            <button type="submit">Pretraži</button>
        </form>
        """, "text/html; charset=utf-8");
});

app.MapPost("/pretraga", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    string ime = form["ime"].ToString();

    // RANJIVO: direktno spajanje korisnickog unosa u SQL
    string sql = "SELECT Id, Ime, Email FROM Korisnici " +
                 "WHERE Ime LIKE '%" + ime + "%'";

    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new SqlCommand(sql, connection);
    await using var reader = await command.ExecuteReaderAsync();

    string rezultat = "<h2>Rezultati pretrage</h2>";

    while (await reader.ReadAsync())
    {
        string imeKorisnika = HtmlEncoder.Default.Encode(
            reader["Ime"].ToString() ?? "");

        string email = HtmlEncoder.Default.Encode(
            reader["Email"].ToString() ?? "");

        rezultat += $"<p>{imeKorisnika} - {email}</p>";
    }

    rezultat += "<a href='/pretraga'>Nova pretraga</a>";

    return Results.Content(rezultat, "text/html; charset=utf-8");
});

// ==========================================
// PRIMJER 3 - SQL INJECTION U BRISANJU
// CWE-89 - RANJIVA VERZIJA
// ==========================================

app.MapGet("/brisanje", () =>
{
    return Results.Content("""
        <h2>Brisanje testnog zapisa</h2>
        <form action="/brisanje" method="post">
            <label>Unesite ID zapisa:</label>
            <input type="text" name="id" required>
            <button type="submit">Obriši</button>
        </form>
        <a href="/zapisi">Prikaži zapise</a>
        """, "text/html; charset=utf-8");
});

// Prikaz testnih zapisa
app.MapGet("/zapisi", async () =>
{
    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    string sql = "SELECT Id, Opis FROM ZapisiZaBrisanje";

    await using var command = new SqlCommand(sql, connection);
    await using var reader = await command.ExecuteReaderAsync();

    string rezultat = "<h2>Testni zapisi</h2>";

    while (await reader.ReadAsync())
    {
        string id = reader["Id"].ToString() ?? "";

        string opis = HtmlEncoder.Default.Encode(
            reader["Opis"].ToString() ?? "");

        rezultat += $"<p>ID: {id} - {opis}</p>";
    }

    rezultat += "<a href='/brisanje'>Nazad na brisanje</a>";

    return Results.Content(rezultat, "text/html; charset=utf-8");
});

app.MapPost("/brisanje", async (HttpRequest request) =>
{
    var form = await request.ReadFormAsync();

    string id = form["id"].ToString();

    // RANJIVO: direktno spajanje korisnickog unosa u SQL
    string sql = "DELETE FROM ZapisiZaBrisanje WHERE Id = " + id;

    await using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    await using var command = new SqlCommand(sql, connection);

    int obrisano = await command.ExecuteNonQueryAsync();

    return Results.Content(
        $"<h2>Broj obrisanih zapisa: {obrisano}</h2>" +
        "<a href='/zapisi'>Pogledaj zapise</a>",
        "text/html; charset=utf-8");
});

app.Run();
