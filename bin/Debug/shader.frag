#version 330 core
out vec4 FragColor;
in float texcoord;

void main()
{
    FragColor = vec4( texcoord, 0.5f, 0.2f, 1.0f);
}