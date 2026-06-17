# DrawingTool

Proiect pentru Laboratorul 7 la disciplina Metode Avansate de Programare.
Acest proiect implementeaza trei Design Patterns Structurale aplicate pe un utilitar de desen grafic.

## Design Patterns Aplicate

1. **Composite**: Utilizat pentru a crea o ierarhie intre formele grafice de baza (Line, Circle, Rectangle, Ellipse) si grupuri de forme (Picture). Clientul poate apela metode precum `Move()`, `Scale()` sau `Draw()` pe o structura complexa fara a cunoaste detaliile elementelor din interior, comportamentul propagandu-se recursiv.
2. **Bridge**: Decupleaza logica elementelor (IShape) de modul in care acestea sunt randate pe ecran/fisier (ICanvas). In acest fel putem avea ConsoleCanvas sau SvgCanvas fara sa modificam codul claselor grafice.
3. **Proxy**: Clasa `ReadOnlyShapeProxy` implementeaza un Protection Proxy. Aceasta infasoara un obiect IShape si controleaza accesul: permite desenarea (Draw) si preluarea coordonatelor (GetBoundingBox), dar blocheaza prin aruncarea unei exceptii incercarile de modificare (Move, Scale).

## Exemplu de cod

Mai jos regasiti constructia unei scene in cod si randarea acesteia sub forma SVG:

```csharp
var myPicture = new Picture();
var background = new Rectangle(0, 0, 100, 100);
var sun = new Circle(80, 20, 10);

var house = new Picture(); // Picture imbricat
house.Add(new Rectangle(20, 50, 40, 40));
house.Add(new Line(20, 50, 40, 20)); // Acoperis

myPicture.Add(background);
myPicture.Add(sun);
myPicture.Add(house);

var svgCanvas = new SvgCanvas();
myPicture.Draw(svgCanvas);
Console.WriteLine(svgCanvas.GetSvg());
