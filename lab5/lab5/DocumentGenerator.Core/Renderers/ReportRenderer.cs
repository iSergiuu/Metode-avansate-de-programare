namespace DocumentGenerator.Core.Renderers;

public class ReportRenderer : IDocumentRenderer
{
    public string Render(string content)
    {
        return $"[REPORT]\n{content}";
    }
}