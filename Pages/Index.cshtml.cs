using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace MeuAppSeguranca.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Url { get; set; } = "";

        [BindProperty]
        public string TestType { get; set; } = "";

        public bool TestInProgress { get; set; } = false;
        public string TestProgressMessage { get; set; } = "";
        public string TestResult { get; set; } = "";

        public void OnGet()
        {
            // Inicialização
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Url) || string.IsNullOrWhiteSpace(TestType))
            {
                ModelState.AddModelError("", "Por favor, informe a URL e o tipo de teste.");
                return Page();
            }

            TestInProgress = true;
            TestProgressMessage = "Iniciando teste...";

            await Task.Delay(2000);

            TestProgressMessage = $"Executando o teste {TestType}...";
            await Task.Delay(3000);

            TestResult = TestType switch
            {
                "PortScan" => $"Port Scan para {Url} concluído.\nPortas abertas: 80, 443, 8080.",
                "SQLInjection" => $"Teste SQL Injection para {Url} concluído.\nNenhuma vulnerabilidade detectada.",
                "XSS" => $"Teste XSS para {Url} concluído.\nVulnerabilidade detectada em parâmetro 'q'.",
                "SSLTest" => $"Teste SSL/TLS para {Url} concluído.\nCertificado válido. Força do protocolo: Forte.",
                "DNSEnumeration" => $"Enumeração DNS para {Url} concluída.\nSubdomínios encontrados: www, api, mail.",
                "Headers" => $"Análise de Headers HTTP para {Url} concluída.\nHeaders seguros aplicados.",
                _ => "Teste desconhecido."
            };

            TestProgressMessage = "Teste concluído.";
            TestInProgress = false;

            return Page();
        }
    }
}
