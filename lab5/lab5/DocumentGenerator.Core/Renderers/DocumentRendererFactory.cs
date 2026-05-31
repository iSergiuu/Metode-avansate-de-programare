namespace DocumentGenerator.Core.Renderers;

public static class DocumentRendererFactory
{
    public static IDocumentRenderer Create(string type)
    {
        return type switch
        {
            "report" => new ReportRenderer(),
            "invoice" => new InvoiceRenderer(),
            _ => throw new ArgumentException("Tip necunoscut")
        };
    }
}