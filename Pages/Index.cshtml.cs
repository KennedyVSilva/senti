using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

        public void OnGet() { }

        public IActionResult OnPost()
        {
            Console.WriteLine("===== [DEBUG] Método OnPost chamado =====");
            Console.WriteLine($"Target recebido: {Target}");
            Console.WriteLine($"Tipo de teste recebido: {TestType}");

            try
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

                var host = SanitizeTarget(Target);

                switch (TestType?.ToLower())
                {
                    case "basic":
                        BasicResult = ExecutarPing(host);
                        SecurityLevel = "Alto";
                        SecurityLevelColor = "green";
                        ThreatLevel = "Baixo";
                        break;

                    case "medium":
                        MediumResult = ExecutarPortScan(host);
                        SecurityLevel = "Médio";
                        SecurityLevelColor = "orange";
                        ThreatLevel = "Moderado";
                        break;

                    case "advanced":
                        AdvancedResult = VerificarHeadersEVulnerabilidades(Target);
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
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO] {ex.Message}");
                ModelState.AddModelError(string.Empty, $"Erro interno: {ex.Message}");
                return Page();
            }
        }

        private string ExecutarPing(string host)
        {
            try
            {
                using var ping = new Ping();
                var reply = ping.Send(host, 3000);
                return reply.Status == IPStatus.Success
                    ? $"Ping OK para {host} em {reply.RoundtripTime}ms"
                    : $"Ping falhou: {reply.Status}";
            }
            catch (Exception ex)
            {
                return $"Erro no ping: {ex.Message}";
            }
        }

        private string ExecutarPortScan(string host)
        {
            var portas = new[] { 21, 22, 25, 80, 443, 3389 };
            var resultados = new List<string>();

            foreach (var porta in portas)
            {
                try
                {
                    using var client = new TcpClient();
                    var resultado = client.ConnectAsync(host, porta).Wait(1000);

                    if (resultado && client.Connected)
                    {
                        resultados.Add($"Porta {porta}: ✅ ABERTA");
                    }
                    else
                    {
                        resultados.Add($"Porta {porta}: ❌ FECHADA");
                    }
                }
                catch
                {
                    resultados.Add($"Porta {porta}: ❌ INACESSÍVEL");
                }
            }

            return $"Scan em {host}:\n" + string.Join("\n", resultados);
        }

        private string VerificarHeadersEVulnerabilidades(string url)
        {
            try
            {
                using var http = new HttpClient();
                var response = http.GetAsync(url).Result;

                var resultado = $"Status HTTP: {(int)response.StatusCode} - {response.ReasonPhrase}\n";

                if (response.Headers.Contains("X-Content-Type-Options"))
                    resultado += "✔ Header X-Content-Type-Options presente\n";
                else
                    resultado += "❌ Header X-Content-Type-Options ausente\n";

                var html = response.Content.ReadAsStringAsync().Result;

                if (html.Contains("<script>") || html.Contains("onerror="))
                    resultado += "⚠️ Possível XSS detectado (conteúdo inline suspeito)\n";
                if (html.ToLower().Contains("sql syntax") || html.ToLower().Contains("mysql"))
                    resultado += "⚠️ Indício de SQLi (mensagem de erro de SQL exposta)\n";

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Erro ao analisar headers ou conteúdo: {ex.Message}";
            }
        }

        private bool IsValidUrlOrIp(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            var ipPattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            var urlPattern = @"^(https?:\/\/)?([a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,})(\/\S*)?$";

            return Regex.IsMatch(input, ipPattern) || Regex.IsMatch(input, urlPattern);
        }

        private string SanitizeTarget(string input)
        {
            if (input.StartsWith("http"))
            {
                try
                {
                    var uri = new Uri(input);
                    return uri.Host;
                }
                catch
                {
                    return input;
                }
            }
            return input;
        }
    }
}
