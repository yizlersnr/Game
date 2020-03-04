#version 330 core
out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
	//outputColor = mix(texture(texture1, texCoord),  texture(texture2, texCoord * vec2(1.0, 1.0)), 0.2);
	outputColor = texture(texture1, texCoord);
}