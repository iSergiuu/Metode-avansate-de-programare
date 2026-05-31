namespace DocumentGenerator.Core.Models;

public class DocumentData
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;

    public List<string> Sections { get; set; } = new();

    public string PageFormat { get; set; } = "A4";
    public string Orientation { get; set; } = "Portrait";

    public List<string> Footnotes { get; set; } = new();

    public DocumentData Clone()
    {
        return new DocumentData
        {
            Title = this.Title,
            Author = this.Author,
            Date = this.Date,
            PageFormat = this.PageFormat,
            Orientation = this.Orientation,
            Sections = new List<string>(this.Sections),
            Footnotes = new List<string>(this.Footnotes)
        };
    }
}