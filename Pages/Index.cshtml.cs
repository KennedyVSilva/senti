using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace MeuAppSeguranca.Pages

    public IActionResult OnPost()
{
    Console.WriteLine("===== [DEBUG] Método OnPost chamado =====");
    Console.WriteLine($"Target recebido: {Target}");
    Console.WriteLine($"Tipo de teste recebido: {TestType}");

    try
    {
        if (string.IsNullOrWhiteSpace(Target))
        {
            Console.WriteLine("[DEBUG] Target vazio ou nulo.");
            ModelState.AddModelError("Target", "O campo é obrigatório.");
            return Page();
        }

        if (!IsValidUrlOrIp(Target))
        {
            Console.WriteLine($"[DEBUG] Target inválido: {Target}");
            ModelState.AddModelError("Target", "Digite uma URL ou IP válido (ex: 192.168.0.1 ou https://site.com).");
            return Page();
        }

        switch (TestType?.ToLower())
        {
            case "basic":
                Console.WriteLine("[DEBUG] Teste: básico");
                BasicResult = $"[Simulado] Executado ping e verificação DNS em {Target}";
                SecurityLevel = "Alto";
                SecurityLevelColor = "green";
                ThreatLevel = "Baixo";
                break;

            case "medium":
                Console.WriteLine("[DEBUG] Teste: médio");
                MediumResult = $"[Simulado] Scan de portas padrão e análise TLS em {Target}";
                SecurityLevel = "Médio";
                SecurityLevelColor = "orange";
                ThreatLevel = "Moderado";
                break;

            case "advanced":
                Console.WriteLine("[DEBUG] Teste: avançado");
                AdvancedResult = $"[Simulado] Verificação de vulnerabilidades XSS, SQLi, headers em {Target}";
                SecurityLevel = "Baixo";
                SecurityLevelColor = "red";
                ThreatLevel = "Crítico";
                break;

            default:
                Console.WriteLine("[DEBUG] Tipo de teste inválido.");
                ModelState.AddModelError("TestType", "Selecione um tipo de teste válido.");
                return Page();
        }

        Console.WriteLine("[DEBUG] Resultado gerado com sucesso.");
        return Page();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERRO] Exceção: {ex.Message}");
        ModelState.AddModelError(string.Empty, $"Erro interno: {ex.Message}");
        return Page();
    }
}
