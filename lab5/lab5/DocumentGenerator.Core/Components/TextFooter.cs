namespace DocumentGenerator.Core.Components;

public class TextFooter : IFooter
{
    public string Render(string author)
    {
        return $"Autor: {author}";
    }
}