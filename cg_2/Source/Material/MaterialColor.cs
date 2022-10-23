namespace cg_2.Source.Material;

/* TODO -> возможно стоит отказаться, но пока не изучена тема материалов, 
   мб в будущем будет сделана иерархия */

public class MaterialColor
{
    public Color Color { get; }

    public MaterialColor(Color color) => Color = color;

    public static MaterialColor Standart => new(Color.Black);
}