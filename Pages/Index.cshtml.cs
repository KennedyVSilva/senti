using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MeuAppSeguranca.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Target { get; set; } = string.Empty;

        public bool HasResult { get; set; } = false;
        public string BasicResult { get; set; } = "";
        public string MediumResult { get; set; } = "";
        public string AdvancedResult { get; set; } = "";
        public string SecurityLevel { get; set; } = "";
        public string ThreatLevel { get; set; } = "";
        public string SecurityLevelColor { get; set; } = "";

        public void OnPost()
        {
            // Verifica se Target está vazio
            if (string.IsNullOrWhiteSpace(Target))
            {
                ModelState.AddModelError("Target", "Informe uma URL ou IP válida.");
                return;
            }

            // Valida se é IP ou URL com http(s)
            var isValidUrl = Uri.IsWellFormedUriString(Target.StartsWith("http") ? Target : "http://" + Target, UriKind.Absolute);
            var isValidIp = System.Net.IPAddress.TryParse(Target, out _);

            if (!isValidUrl && !isValidIp)
            {
                ModelState.AddModelError("Target", "Formato inválido de URL ou IP.");
                return;
            }

            if (!ModelState.IsValid)
                return;

            HasResult = true;

            // Simulação de resultados
            BasicResult = "Conexão segura. SSL válido.";
            MediumResult = "Headers parcialmente protegidos.";
            AdvancedResult = "Possível XSS detectado em parâmetro de entrada.";

            SecurityLevel = "Médio";
            ThreatLevel = "Moderado";
            SecurityLevelColor = "#facc15"; // amarelo
        }
    }
}
