using DocumentGenerator.Core.Components;

namespace DocumentGenerator.Core.Factories;

public class HtmlDocumentComponentFactory : IDocumentComponentFactory
{
    public IHeader CreateHeader()
    {
        return new HtmlHeader();
    }

    public IBody CreateBody()
    {
        return new HtmlBody();
    }

    public IFooter CreateFooter()
    {
        return new HtmlFooter();
    }
}