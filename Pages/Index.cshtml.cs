namespace MeuAppSeguranca.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public void OnPost()
{
    if (string.IsNullOrWhiteSpace(Target))
    {
        ModelState.AddModelError("Target", "Informe uma URL ou IP válida.");
        return;
    }

    // Aqui aceitamos IP ou URL simples
    if (!Uri.IsWellFormedUriString("http://" + Target, UriKind.Absolute) &&
        !System.Net.IPAddress.TryParse(Target, out _))
    {
        ModelState.AddModelError("Target", "Formato inválido de URL ou IP.");
        return;
    }

    HasResult = true;

    // Simulações
    BasicResult = "Conexão segura. SSL válido.";
    MediumResult = "Headers parcialmente protegidos.";
    AdvancedResult = "Possível XSS detectado em parâmetro de entrada.";

    SecurityLevel = "Médio";
    ThreatLevel = "Moderado";
    SecurityLevelColor = "#facc15";
}
