namespace DocumentGenerator.Core.Components;

public class TextHeader : IHeader
{
    public string Render(string title)
    {
        return $"=== {title} ===";
    }
}