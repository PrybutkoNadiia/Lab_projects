using System;
using System.Collections.Generic;

namespace BinaryTreeApp
{
    public class Circle : IComparable<Circle>
    {
        public string FillColor { get; set; }
        public string StrokeColor { get; set; }
        public double Radius { get; set; }

        public Circle(string fillColor, string strokeColor, double radius)
        {
            FillColor = fillColor;
            StrokeColor = strokeColor;
            Radius = radius;
        }

        public double CalculateArea()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }

        public double CalculateCircumference()
        {
            return 2 * Math.PI * Radius;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"[Circle] Fill: {FillColor}, Stroke: {StrokeColor}, Radius: {Radius}, Area: {CalculateArea():F2}, Circumference: {CalculateCircumference():F2}");
        }

        // Додано ? для підтримки nullable
        public int CompareTo(Circle? other)
        {
            if (other == null) return 1;
            return Radius.CompareTo(other.Radius);
        }

        public override string ToString()
        {
            return $"Circle (Radius: {Radius}, Area: {CalculateArea():F2})";
        }
    }

    public class CircleAreaComparer : IComparer<Circle>
    {
        // Додано ? для x та y
        public int Compare(Circle? x, Circle? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.CalculateArea().CompareTo(y.CalculateArea());
        }
    }
}