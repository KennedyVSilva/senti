using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MeuAppSeguranca.Pages
{
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
                ModelState.AddModelError("Target", "Informe uma URL ou IP válida.");
                return;
            }

            // Tenta validar se é uma URL ou IP simples
            if (!Uri.IsWellFormedUriString("http://" + Target, UriKind.Absolute) &&
                !System.Net.IPAddress.TryParse(Target, out _))
            {
                ModelState.AddModelError("Target", "Formato inválido de URL ou IP.");
                return;
            }

            HasResult = true;

            // Simulação de testes (futuramente substituir por reais)
            BasicResult = "Conexão segura. SSL válido.";
            MediumResult = "Headers parcialmente protegidos.";
            AdvancedResult = "Possível XSS detectado em parâmetro de entrada.";

            SecurityLevel = "Médio";
            ThreatLevel = "Moderado";
            SecurityLevelColor = "#facc15"; // Amarelo
        }
    }
}
