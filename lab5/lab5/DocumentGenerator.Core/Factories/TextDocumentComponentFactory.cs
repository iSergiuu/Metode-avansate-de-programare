using DocumentGenerator.Core.Components;

namespace DocumentGenerator.Core.Factories;

public class TextDocumentComponentFactory : IDocumentComponentFactory
{
    public IHeader CreateHeader()
    {
        return new TextHeader();
    }

    public IBody CreateBody()
    {
        return new TextBody();
    }

    public IFooter CreateFooter()
    {
        return new TextFooter();
    }
}