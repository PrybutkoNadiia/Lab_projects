namespace Lab_3_3.DAL;

public class CircleEntity
{
    public string FillColor { get; set; } = string.Empty;
    public string OutlineColor { get; set; } = string.Empty;
    public double Radius { get; set; }

    public double CalculateArea() => Math.PI * Radius * Radius;

    public double CalculateCircumference() => 2 * Math.PI * Radius;

    public override string ToString() =>
        $"Fill: {FillColor}, outline: {OutlineColor}, radius: {Radius}";
}
