namespace cg_2.Views;

public partial class OpenGlControl
{
    public static readonly DependencyProperty GlControlProperty = DependencyProperty
        .Register(nameof(GlControl),
            typeof(OpenGLControl),
            typeof(OpenGlControl),
            new(null));

    public OpenGLControl GlControl
    {
        get => (OpenGLControl)GetValue(GlControlProperty);
        set => SetValue(GlControlProperty, value);
    }

    public OpenGlControl() => InitializeComponent();
}