using DocumentGenerator.Core.Models;

namespace DocumentGenerator.Core.Builders;

public class DocumentDataBuilder
{
    private readonly DocumentData _document = new();

    public DocumentDataBuilder WithTitle(string title)
    {
        _document.Title = title;
        return this;
    }

    public DocumentDataBuilder ByAuthor(string author)
    {
        _document.Author = author;
        return this;
    }

    public DocumentDataBuilder WithSection(string section)
    {
        _document.Sections.Add(section);
        return this;
    }

    public DocumentDataBuilder InLandscape()
    {
        _document.Orientation = "Landscape";
        return this;
    }

    public DocumentDataBuilder WithFootnote(string footnote)
    {
        _document.Footnotes.Add(footnote);
        return this;
    }

    public DocumentData Build()
    {
        if (string.IsNullOrWhiteSpace(_document.Title))
            throw new InvalidOperationException("Documentul trebuie să aibă titlu.");

        if (string.IsNullOrWhiteSpace(_document.Author))
            throw new InvalidOperationException("Documentul trebuie să aibă autor.");

        if (_document.Sections.Count == 0)
            throw new InvalidOperationException("Documentul trebuie să aibă cel puțin o secțiune.");

        return _document;
    }
}