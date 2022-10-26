namespace cg_2.Source.Descriptors;

public class InstanceDescriptor
{
    private byte _attributeСount;
    private bool _onlyVertices;

    public bool OnlyVertices
    {
        init
        {
            if (!value) return;
            _onlyVertices = true;
            WithNormals = WithTextures = WithColors = false;
            _attributeСount = 1;
        }
    }

    public bool WithNormals
    {
        init
        {
            if (value)
            {
                _attributeСount++;
            }
        }
    }

    public bool WithTextures
    {
        init
        {
            if (value)
            {
                _attributeСount++;
            }
        }
    }

    public bool WithColors
    {
        init
        {
            if (value)
            {
                _attributeСount++;
            }
        }
    }

    public byte AttributeCount
    {
        get
        {
            _attributeСount = _onlyVertices ? (byte)1 : _attributeСount;
            return _attributeСount;
        }
    }
}