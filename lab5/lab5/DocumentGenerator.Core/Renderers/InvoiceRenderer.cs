namespace DocumentGenerator.Core.Renderers;

public class InvoiceRenderer : IDocumentRenderer
{
    public string Render(string content)
    {
        return $"[INVOICE]\n{content}";
    }
}