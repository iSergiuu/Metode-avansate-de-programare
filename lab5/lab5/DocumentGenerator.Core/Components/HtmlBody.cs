namespace DocumentGenerator.Core.Components;

public class HtmlBody : IBody
{
    public string Render(IEnumerable<string> sections)
    {
        return string.Join(Environment.NewLine, sections.Select(s => $"<p>{s}</p>"));
    }
}