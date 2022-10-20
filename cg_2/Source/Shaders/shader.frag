#version 330 core

in vec3 vertexColor;
in vec2 textureCoord;

out vec4 color;

uniform sampler2D myTexture;

void main()
{
    color = vec4(0.0f, 1.0f, 1.0f, 1.0f);
    //color = texture(myTexture, textureCoord);
}