using NUnit.Framework;
using DrawingTool.Core;
using System;

namespace DrawingTool.Tests
{
    public class DrawingToolTests
    {
        [Test]
        public void Picture_Scale_PropagatesToAllChildren()
        {
            // arrange
            var picture = new Picture();
            var line = new Line(0, 0, 10, 10);
            var circle = new Circle(5, 5, 5);
            var rect = new Rectangle(0, 0, 20, 10);
            
            picture.Add(line);
            picture.Add(circle);
            picture.Add(rect);

            // act
            picture.Scale(2.0);

            // assert
            Assert.That(line.X2, Is.EqualTo(20));
            Assert.That(circle.R, Is.EqualTo(10));
            Assert.That(rect.W, Is.EqualTo(40));
        }

        [Test]
        public void Picture_GetBoundingBox_ReturnsCombinedBox()
        {
            // arrange
            var picture = new Picture();
            picture.Add(new Rectangle(0, 0, 10, 10)); // Box: 0,0 la 10,10
            picture.Add(new Circle(20, 20, 5));       // Box: 15,15 la 25,25

            // act
            var box = picture.GetBoundingBox();

            // assert
            Assert.That(box.MinX, Is.EqualTo(0));
            Assert.That(box.MinY, Is.EqualTo(0));
            Assert.That(box.MaxX, Is.EqualTo(25));
            Assert.That(box.MaxY, Is.EqualTo(25));
        }

        [Test]
        public void SvgCanvas_DrawCircle_GeneratesCorrectSvgTag()
        {
            // arrange
            var canvas = new SvgCanvas();
            var circle = new Circle(10, 10, 5);

            // act
            circle.Draw(canvas);
            var svgOutput = canvas.GetSvg();

            // assert
            Assert.That(svgOutput, Does.Contain("<circle cx=\"10\" cy=\"10\" r=\"5\""));
        }

        [Test]
        public void ReadOnlyShapeProxy_Move_ThrowsInvalidOperationException()
        {
            // arrange
            var circle = new Circle(0, 0, 5);
            var proxy = new ReadOnlyShapeProxy(circle);

            // act & assert
            Assert.Throws<InvalidOperationException>(() => proxy.Move(10, 10));
        }

        [Test]
        public void ReadOnlyShapeProxy_Draw_DoesNotThrow()
        {
            // arrange
            var circle = new Circle(0, 0, 5);
            var proxy = new ReadOnlyShapeProxy(circle);
            var canvas = new ConsoleCanvas();

            // act & assert
            Assert.DoesNotThrow(() => proxy.Draw(canvas));
        }
    }
}