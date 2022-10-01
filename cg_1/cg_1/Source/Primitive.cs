using System;
using System.Drawing;
using System.Collections.Generic;

namespace ComputerGraphics.Source
{
    public class StripLine : ICloneable
    {
        public List<Point2D> Points { get; private set; }
        public Color Color { get; set; }
        public float Thickness { get; set; }
        public ushort Stipple { get; set; }
        public float ScaleXY { get; private set; }
        public float Angle { get; private set; }

        public Point2D MassCenter()
        {
            float x = 0, y = 0;

            foreach (var p in Points)
            {
                x += p.X;
                y += p.Y;
            }

            x = x / Points.Count;
            y = y / Points.Count;

            return new Point2D(x, y);
        }

        public StripLine()
        {
            Points = new List<Point2D>();
            Color = new Color();
            Thickness = 1.0f;
            Stipple = 0xFFFF;
            ScaleXY = 1.0f;
        }

        public object Clone() => new StripLine
        {
            Points = new List<Point2D>(Points),
            Color = Color,
            Thickness = Thickness,
            Stipple = Stipple,
            ScaleXY = ScaleXY
        };

        public void Reset()
        {
            Points.Clear();
            Color = new Color();
            Thickness = 1.0f;
            Stipple = 0xFFFF;
            ScaleXY = 1.0f;
        }

        public void Scale(Point2D pivot, float scaling)
        {
            ScaleXY *= scaling;

            float xStep = pivot.X * scaling - pivot.X;
            float yStep = pivot.Y * scaling - pivot.Y;

            for (int i = 0; i < Points.Count; i++)
            {
                var x = Points[i].X * scaling - xStep;
                var y = Points[i].Y * scaling - yStep;

                Points[i] = new Point2D(x, y);
            }
        }

        public void Rotate(Point2D pivot, float angle)
        {
            Angle += angle;

            float radAngle = (float)(Math.PI * angle / 180.0);

            for (int i = 0; i < Points.Count; i++)
            {
                var x = Points[i].X - pivot.X;
                var y = Points[i].Y - pivot.Y;

                var newX = (float)(x * Math.Cos(radAngle) + y * Math.Sin(radAngle));
                var newY = (float)(-x * Math.Sin(radAngle) + y * Math.Cos(radAngle));

                newX += pivot.X;
                newY += pivot.Y;

                Points[i] = new Point2D(newX, newY);
            }
        }
    }
}