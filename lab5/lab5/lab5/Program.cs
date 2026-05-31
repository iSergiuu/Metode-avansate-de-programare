using DocumentGenerator.Core.Builders;
using DocumentGenerator.Core.Configuration;
using DocumentGenerator.Core.Factories;
using DocumentGenerator.Core.Configuration;

var document = new DocumentDataBuilder()
    .WithTitle("Raport lunar")
    .ByAuthor("Radu")
    .WithSection("Sectiunea 1")
    .WithSection("Sectiunea 2")
    .Build();

IDocumentComponentFactory factory = new HtmlDocumentComponentFactory();

var header = factory.CreateHeader();
var body = factory.CreateBody();
var footer = factory.CreateFooter();

Console.WriteLine(header.Render(document.Title));
Console.WriteLine(body.Render(document.Sections));
Console.WriteLine(footer.Render(document.Author));
Console.WriteLine("\n--- PROTOTYPE ---");

var original = new DocumentDataBuilder()
    .WithTitle("Template raport")
    .ByAuthor("Admin")
    .WithSection("Sectiune template")
    .Build();

var copy = original.Clone();

copy.Title = "Raport nou";

Console.WriteLine("Original: " + original.Title);
Console.WriteLine("Copy: " + copy.Title);

Console.WriteLine("\n--- SINGLETON ---");

var config1 = ConfigurationManager.Instance;
var config2 = ConfigurationManager.Instance;

config1.DefaultFormat = "TEXT";

Console.WriteLine("Config1: " + config1.DefaultFormat);
Console.WriteLine("Config2: " + config2.DefaultFormat);