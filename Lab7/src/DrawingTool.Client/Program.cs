using System;
using System.IO;
using DrawingTool.Core;

namespace DrawingTool.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tema MAP - Laborator 7: Drawing Tool\n");

            // construim scena exact ca in README
            var myPicture = new Picture();
            var background = new Rectangle(0, 0, 100, 100);
            var sun = new Circle(80, 20, 10);

            var house = new Picture(); 
            house.Add(new Rectangle(20, 50, 40, 40));
            house.Add(new Line(20, 50, 40, 20));

            myPicture.Add(background);
            myPicture.Add(sun);
            myPicture.Add(house);

            // randam in format SVG
            var svgCanvas = new SvgCanvas();
            myPicture.Draw(svgCanvas);
            string svgContent = svgCanvas.GetSvg();

            // salvam rezultatul intr-un fisier fizic
            string currentDirectory = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(currentDirectory, "Desen.svg");
            
            File.WriteAllText(filePath, svgContent);

            Console.WriteLine(svgContent);
            Console.WriteLine($"\n[SUCCES] Fisierul a fost salvat cu succes!");
            Console.WriteLine($"Il gasesti aici: {filePath}");
        }
    }
}