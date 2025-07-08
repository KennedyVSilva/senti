public void OnPost()
{
    if (string.IsNullOrWhiteSpace(Target))
    {
        ModelState.AddModelError("", "Informe um IP ou URL para testar.");
        HasResult = false;
        return;
    }

    Uri uriResult;
    bool validUrl = Uri.TryCreate(Target, UriKind.Absolute, out uriResult);

    if (!validUrl)
    {
        validUrl = Uri.TryCreate("http://" + Target, UriKind.Absolute, out uriResult);
        if (validUrl)
        {
            Target = "http://" + Target;
        }
    }

    if (!validUrl)
    {
        ModelState.AddModelError("", "O valor informado não é um IP ou URL válido.");
        HasResult = false;
        return;
    }

    // Se chegou aqui, input é válido
    HasResult = true;

    BasicResult = "Conexão segura. SSL válido.";
    MediumResult = "Headers parcialmente protegidos.";
    AdvancedResult = "Possível XSS detectado em parâmetro de entrada.";

    SecurityLevel = "Médio";
    ThreatLevel = "Moderado";
    SecurityLevelColor = "#facc15"; // amarelo
}
