namespace Lab_3_3.BLL;

public sealed record Circle(string FillColor, string OutlineColor, double Radius)
{
    public double CalculateArea() => Math.PI * Radius * Radius;

    public double CalculateCircumference() => 2 * Math.PI * Radius;

    public override string ToString() =>
        $"Fill: {FillColor}, outline: {OutlineColor}, radius: {Radius}";
}
