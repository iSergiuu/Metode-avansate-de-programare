using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawingTool.Core
{
    // modele de baza
    public class BoundingBox
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        public BoundingBox(double minX, double minY, double maxX, double maxY)
        {
            MinX = minX; MinY = minY; MaxX = maxX; MaxY = maxY;
        }

        public static BoundingBox Empty => new BoundingBox(0, 0, 0, 0);

        public void Combine(BoundingBox other)
        {
            if (other == null) return;
            MinX = Math.Min(MinX, other.MinX);
            MinY = Math.Min(MinY, other.MinY);
            MaxX = Math.Max(MaxX, other.MaxX);
            MaxY = Math.Max(MaxY, other.MaxY);
        }
    }

    // 1. bridge pattern - separarea randarii de model
    public interface ICanvas
    {
        void DrawLine(double x1, double y1, double x2, double y2);
        void DrawCircle(double cx, double cy, double r);
        void DrawRect(double x, double y, double w, double h);
        void DrawEllipse(double cx, double cy, double rx, double ry);
    }

    public class ConsoleCanvas : ICanvas
    {
        public void DrawLine(double x1, double y1, double x2, double y2) => Console.WriteLine($"DrawLine: ({x1},{y1}) to ({x2},{y2})");
        public void DrawCircle(double cx, double cy, double r) => Console.WriteLine($"DrawCircle: center({cx},{cy}), radius={r}");
        public void DrawRect(double x, double y, double w, double h) => Console.WriteLine($"DrawRect: pos({x},{y}), size={w}x{h}");
        public void DrawEllipse(double cx, double cy, double rx, double ry) => Console.WriteLine($"DrawEllipse: center({cx},{cy}), rx={rx}, ry={ry}");
    }

    public class SvgCanvas : ICanvas
    {
        private StringBuilder _svgContent = new StringBuilder();

        public void DrawLine(double x1, double y1, double x2, double y2) => _svgContent.AppendLine($"<line x1=\"{x1}\" y1=\"{y1}\" x2=\"{x2}\" y2=\"{y2}\" stroke=\"black\" />");
        public void DrawCircle(double cx, double cy, double r) => _svgContent.AppendLine($"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{r}\" stroke=\"black\" fill=\"none\" />");
        public void DrawRect(double x, double y, double w, double h) => _svgContent.AppendLine($"<rect x=\"{x}\" y=\"{y}\" width=\"{w}\" height=\"{h}\" stroke=\"black\" fill=\"none\" />");
        public void DrawEllipse(double cx, double cy, double rx, double ry) => _svgContent.AppendLine($"<ellipse cx=\"{cx}\" cy=\"{cy}\" rx=\"{rx}\" ry=\"{ry}\" stroke=\"black\" fill=\"none\" />");

        public string GetSvg()
        {
            return $"<svg xmlns=\"http://www.w3.org/2000/svg\">\n{_svgContent.ToString()}</svg>";
        }
    }

    // 2. composite pattern - ierarhia de forme

    public interface IShape
    {
        void Draw(ICanvas canvas);
        void Move(double dx, double dy);
        void Scale(double factor);
        BoundingBox GetBoundingBox();
    }

    public class Line : IShape
    {
        public double X1, Y1, X2, Y2;
        public Line(double x1, double y1, double x2, double y2) { X1 = x1; Y1 = y1; X2 = x2; Y2 = y2; }
        
        public void Draw(ICanvas canvas) => canvas.DrawLine(X1, Y1, X2, Y2);
        public void Move(double dx, double dy) { X1 += dx; Y1 += dy; X2 += dx; Y2 += dy; }
        public void Scale(double f) { X1 *= f; Y1 *= f; X2 *= f; Y2 *= f; }
        public BoundingBox GetBoundingBox() => new BoundingBox(Math.Min(X1, X2), Math.Min(Y1, Y2), Math.Max(X1, X2), Math.Max(Y1, Y2));
    }

    public class Circle : IShape
    {
        public double CX, CY, R;
        public Circle(double cx, double cy, double r) { CX = cx; CY = cy; R = r; }

        public void Draw(ICanvas canvas) => canvas.DrawCircle(CX, CY, R);
        public void Move(double dx, double dy) { CX += dx; CY += dy; }
        public void Scale(double f) { R *= f; }
        public BoundingBox GetBoundingBox() => new BoundingBox(CX - R, CY - R, CX + R, CY + R);
    }

    public class Rectangle : IShape
    {
        public double X, Y, W, H;
        public Rectangle(double x, double y, double w, double h) { X = x; Y = y; W = w; H = h; }

        public void Draw(ICanvas canvas) => canvas.DrawRect(X, Y, W, H);
        public void Move(double dx, double dy) { X += dx; Y += dy; }
        public void Scale(double f) { W *= f; H *= f; }
        public BoundingBox GetBoundingBox() => new BoundingBox(X, Y, X + W, Y + H);
    }

    public class Ellipse : IShape
    {
        public double CX, CY, RX, RY;
        public Ellipse(double cx, double cy, double rx, double ry) { CX = cx; CY = cy; RX = rx; RY = ry; }

        public void Draw(ICanvas canvas) => canvas.DrawEllipse(CX, CY, RX, RY);
        public void Move(double dx, double dy) { CX += dx; CY += dy; }
        public void Scale(double f) { RX *= f; RY *= f; }
        public BoundingBox GetBoundingBox() => new BoundingBox(CX - RX, CY - RY, CX + RX, CY + RY);
    }

    public class Picture : IShape
    {
        private readonly List<IShape> _shapes = new List<IShape>();

        public void Add(IShape shape) => _shapes.Add(shape);
        public void Remove(IShape shape) => _shapes.Remove(shape);

        public void Draw(ICanvas canvas)
        {
            foreach (var shape in _shapes) shape.Draw(canvas);
        }

        public void Move(double dx, double dy)
        {
            foreach (var shape in _shapes) shape.Move(dx, dy);
        }

        public void Scale(double factor)
        {
            foreach (var shape in _shapes) shape.Scale(factor);
        }

        public BoundingBox GetBoundingBox()
        {
            if (!_shapes.Any()) return BoundingBox.Empty;
            
            var box = _shapes.First().GetBoundingBox();
            foreach (var shape in _shapes.Skip(1))
            {
                box.Combine(shape.GetBoundingBox());
            }
            return box;
        }
    }

    // 3. proxy pattern - control de acces (read only)

    public class ReadOnlyShapeProxy : IShape
    {
        private readonly IShape _realShape;

        public ReadOnlyShapeProxy(IShape realShape)
        {
            _realShape = realShape;
        }

        public void Draw(ICanvas canvas) => _realShape.Draw(canvas);
        public BoundingBox GetBoundingBox() => _realShape.GetBoundingBox();

        public void Move(double dx, double dy)
        {
            throw new InvalidOperationException("Shape is locked");
        }

        public void Scale(double factor)
        {
            throw new InvalidOperationException("Shape is locked");
        }
    }
}