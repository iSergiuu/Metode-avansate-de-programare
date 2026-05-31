namespace DocumentGenerator.Core.Components;

public class HtmlHeader : IHeader
{
    public string Render(string title)
    {
        return $"<h1>{title}</h1>";
    }
}