using DocumentGenerator.Core.Components;

namespace DocumentGenerator.Core.Factories;

public interface IDocumentComponentFactory
{
    IHeader CreateHeader();
    IBody CreateBody();
    IFooter CreateFooter();
}