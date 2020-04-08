#version 330 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aColour;

out vec2 texCoord;
out vec3 Normal;
out vec3 FragPos;
out vec3 FragColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main(void)
{
    texCoord = aTexCoord;

	gl_Position = vec4(aPosition, 1.0) * model * view * projection;
	FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = aNormal;
	FragColor = aColour;
}
