#version 330 core

struct Material
{
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};

struct Light
{
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

uniform Light light;
uniform Material material;
uniform vec3 viewPos;
uniform vec3 objectColor;
uniform int useMaterial;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;

vec3 calcAmbient(in Material material, in vec3 objectColor) {
    if (useMaterial == 1) {
        return material.ambient;
    }

    return objectColor;
}

vec3 calcDiffuse(in Material material, in vec3 objectColor) {
    if (useMaterial == 1) {
        return material.diffuse;
    }

    return objectColor;
}

vec3 calcSpecular(in Material material, in vec3 objectColor) {
    if (useMaterial == 1) {
        return material.specular;
    }

    return objectColor;
}

float calcShininess(in Material material) {
    if (useMaterial == 1) {
        return material.shininess;
    }

    return 32;
}

vec3 calculateResult() {
    vec3 vec;
    if (useMaterial == 1) {
        vec = vec3(1.0f);
        return vec;
    }

    return objectColor;
}

void main()
{
    vec3 ambient = light.ambient * calcAmbient(material, objectColor);

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * calcDiffuse(material, objectColor);

    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), calcShininess(material));
    vec3 specular = light.specular * spec * calcSpecular(material, objectColor);

    float distance    = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;

    vec3 result = (ambient + diffuse + specular) * calculateResult();
    FragColor = vec4(result, 1.0);
}