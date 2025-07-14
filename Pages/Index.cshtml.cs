using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace MeuAppSeguranca.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Target { get; set; }

        [BindProperty]
        public string TestType { get; set; }

        public string BasicResult { get; set; }
        public string MediumResult { get; set; }
        public string AdvancedResult { get; set; }

        public string SecurityLevel { get; set; }
        public string SecurityLevelColor { get; set; }
        public string ThreatLevel { get; set; }

        public bool HasResult =>
            !string.IsNullOrEmpty(BasicResult) ||
            !string.IsNullOrEmpty(MediumResult) ||
            !string.IsNullOrEmpty(AdvancedResult);

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Target))
            {
                ModelState.AddModelError("Target", "O campo é obrigatório.");
                return Page();
            }

            if (!IsValidUrlOrIp(Target))
            {
                ModelState.AddModelError("Target", "Digite uma URL ou IP válido.");
                return Page();
            }

            switch (TestType?.ToLower())
            {
                case "basic":
                    BasicResult = $"[Simulado] Executado ping e verificação DNS em {Target}";
                    SecurityLevel = "Alto";
                    SecurityLevelColor = "green";
                    ThreatLevel = "Baixo";
                    break;

                case "medium":
                    MediumResult = $"[Simulado] Scan de portas padrão e análise TLS em {Target}";
                    SecurityLevel = "Médio";
                    SecurityLevelColor = "orange";
                    ThreatLevel = "Moderado";
                    break;

                case "advanced":
                    AdvancedResult = $"[Simulado] Verificação de vulnerabilidades XSS, SQLi, headers em {Target}";
                    SecurityLevel = "Baixo";
                    SecurityLevelColor = "red";
                    ThreatLevel = "Crítico";
                    break;

                default:
                    ModelState.AddModelError("TestType", "Selecione um tipo de teste válido.");
                    return Page();
            }

            return Page();
        }

        private bool IsValidUrlOrIp(string input)
        {
            string urlPattern = @"^https?:\/\/[^\s\/$.?#].[^\s]*$";
            string ipPattern = @"^(\d{1,3}\.){3}\d{1,3}$";

            return Regex.IsMatch(input, urlPattern) || Regex.IsMatch(input, ipPattern);
        }
    }
}
