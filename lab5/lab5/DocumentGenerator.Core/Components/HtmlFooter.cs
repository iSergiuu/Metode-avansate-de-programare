namespace DocumentGenerator.Core.Components;

public class HtmlFooter : IFooter
{
    public string Render(string author)
    {
        return $"<footer>Autor: {author}</footer>";
    }
}