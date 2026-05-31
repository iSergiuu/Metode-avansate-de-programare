namespace DocumentGenerator.Core.Components;

public interface IBody
{
    string Render(IEnumerable<string> sections);
}