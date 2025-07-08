namespace MeuAppSeguranca.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string Target { get; set; } = string.Empty;

    public bool HasResult { get; set; }
    public string BasicResult { get; set; } = "";
    public string MediumResult { get; set; } = "";
    public string AdvancedResult { get; set; } = "";
    public string SecurityLevel { get; set; } = "";
    public string ThreatLevel { get; set; } = "";
    public string SecurityLevelColor { get; set; } = "";

    public void OnPost()
    {
        if (string.IsNullOrWhiteSpace(Target))
        {
            ModelState.AddModelError("Target", "O campo URL/IP é obrigatório.");
            return;
        }

        HasResult = true;

        // Simulação dos testes
        BasicResult = "✅ Conexão segura. SSL válido.";
        MediumResult = "⚠️ Headers parcialmente protegidos.";
        AdvancedResult = "❌ Possível XSS detectado em parâmetro de entrada.";

        SecurityLevel = "Médio";
        ThreatLevel = "Moderado";
        SecurityLevelColor = "#facc15"; // amarelo
    }
}
