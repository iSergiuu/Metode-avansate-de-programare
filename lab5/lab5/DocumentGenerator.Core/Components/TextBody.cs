namespace DocumentGenerator.Core.Components;

public class TextBody : IBody
{
    public string Render(IEnumerable<string> sections)
    {
        return string.Join(Environment.NewLine, sections);
    }
}