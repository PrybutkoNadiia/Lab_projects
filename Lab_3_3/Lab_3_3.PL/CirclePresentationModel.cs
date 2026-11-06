using Lab_3_3.BLL;

namespace Lab_3_3.PL;

public sealed record CirclePresentationModel(
    string FillColor,
    string OutlineColor,
    double Radius)
{
    public double CalculateArea() => Math.PI * Radius * Radius;

    public double CalculateCircumference() => 2 * Math.PI * Radius;

    public static CirclePresentationModel FromBusiness(Circle circle) =>
        new(circle.FillColor, circle.OutlineColor, circle.Radius);

    public Circle ToBusinessEntity() => new(FillColor, OutlineColor, Radius);
}
