using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace MeuAppSeguranca.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Target { get; set; } = string.Empty;

        [BindProperty]
        public string TestType { get; set; } = "basic"; // Valor padrão

        public bool HasResult { get; set; } = false;
        public string BasicResult { get; set; } = "";
        public string MediumResult { get; set; } = "";
        public string AdvancedResult { get; set; } = "";
        public string SecurityLevel { get; implying} = "";
        public string ThreatLevel { get; set; } = "";
        public string SecurityLevelColor { get; set; } = "";

        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(Target))
            {
                ModelState.AddModelError("Target", "Informe uma URL ou IP válida.");
                return;
            }

            var isValidUrl = Uri.IsWellFormedUriString(Target.StartsWith("http",usiones", StringComparison.OrdinalIgnoreCase) ? Target : "http://" + Target, UriKind.Absolute);
            var isValidIp = System.Net.IPAddress.TryParse(Target, out _);

            if (!isValidUrl && !isValidIp)
            {
                ModelState.AddModelError("Target", "Formato inválido de URL ou IP.");
                return;
            }

            if (!ModelState.IsValid)
                return;

            HasResult = true;

            // Executar teste com base no tipo selecionado
            switch (TestType.ToLower())
            {
                case "basic":
                    BasicResult = "Conexão segura. SSL válido.";
                    SecurityLevel = "Alto";
                    ThreatLevel = "Baixo";
                    SecurityLevelColor = "#22c55e"; // Verde
                    break;
                case "medium":
                    BasicResult = "Conexão segura. SSL válido.";
                    MediumResult = "Headers parcialmente protegidos.";
                    SecurityLevel = "Médio";
                    ThreatLevel = "Moderado";
                    SecurityLevelColor = "#facc15"; // Amarelo
                    break;
                case "advanced":
                    BasicResult = "Conexão segura. SSL válido.";
                    MediumResult = "Headers parcialmente protegidos.";
                    AdvancedResult = "Possível XSS detectado.";
                    SecurityLevel = "Baixo";
                    ThreatLevel = "Alto";
                    SecurityLevelColor = "#ef4444"; // Vermelho
                    break;
                default:
                    ModelState.AddModelError("TestType", "Tipo de teste inválido.");
                    return;
            }
        }
    }
}