namespace cg_2.Source;
public class Transformer
{
    private TransformationMatrix _scaleMatrix;
    private TransformationMatrix _translateMatrix;

    public Transformer() => (_scaleMatrix, _translateMatrix) = (new(), new());

    public TransformationMatrix ScaleMatrix(float sx, float sy, float sz)
    {
        _scaleMatrix[0, 0] = sx;
        _scaleMatrix[1, 1] = sy;
        _scaleMatrix[2, 2] = sz;
        _scaleMatrix[3, 3] = 1;

        return _scaleMatrix;
    }

    public TransformationMatrix TranslateMatrix(float dx, float dy, float dz)
    {
        _translateMatrix[0, 0] = 1;
        _translateMatrix[0, 3] = dx;
        _translateMatrix[1, 1] = 1;
        _translateMatrix[1, 3] = dy;
        _translateMatrix[2, 2] = 1;
        _translateMatrix[2, 3] = dz;
        _translateMatrix[3, 3] = 1;

        return _translateMatrix;
    }
}