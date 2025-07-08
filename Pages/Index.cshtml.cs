using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            // Se vazio, retorna para não executar nada
            if (string.IsNullOrWhiteSpace(Target))
            {
                ModelState.AddModelError("", "Informe um IP ou URL para testar.");
                return;
            }

            // Validação da URL/IP
            Uri uriResult;
            bool validUrl = Uri.TryCreate(Target, UriKind.Absolute, out uriResult);

            // Se não válido, tenta adicionar esquema http:// para validar
            if (!validUrl)
            {
                validUrl = Uri.TryCreate("http://" + Target, UriKind.Absolute, out uriResult);
                if (validUrl)
                {
                    Target = "http://" + Target; // Atualiza para URL completa
                }
            }

            // Se ainda não válido, retorna erro
            if (!validUrl)
            {
                ModelState.AddModelError("", "O valor informado não é um IP ou URL válido.");
                return;
            }

            // Se chegou aqui, Target é válido
            HasResult = true;

            // Simulações (substitua futuramente por testes reais)
            BasicResult = "Conexão segura. SSL válido.";
            MediumResult = "Headers parcialmente protegidos.";
            AdvancedResult = "Possível XSS detectado em parâmetro de entrada.";

            SecurityLevel = "Médio";
            ThreatLevel = "Moderado";
            SecurityLevelColor = "#facc15"; // amarelo
        }
    }
}
