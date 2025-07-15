using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

var builder = WebApplication.CreateBuilder(args);

// Ativa Razor Pages com recompilação em tempo real
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

// Adiciona suporte a antifalsificação (incluso por padrão)
builder.Services.AddAntiforgery(); // opcional, reforça a intenção

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // ✅ Garante redirecionamento para HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // ✅ Mesmo que não use autenticação, isso é necessário para evitar 400

app.MapRazorPages();

app.Run();
