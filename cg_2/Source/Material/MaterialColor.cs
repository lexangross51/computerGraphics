namespace cg_2.Source.Material;

public class MaterialColor
{
    public Color Color { get; }

    public MaterialColor(Color color) => Color = color;

    public static MaterialColor Standart => new(Color.Black);
}