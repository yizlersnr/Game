#version 330 core
out vec4 outputColor;

in vec2 texCoord;
in vec3 Normal; //The normal of the fragment is calculated in the vertex shader.
in vec3 FragPos; //The fragment position.
in vec3 FragColor; //The fragment colour.

uniform sampler2D texture1;
uniform sampler2D texture2;

uniform vec3 objectColor; //The color of the object.
uniform vec3 lightColor; //The color of the light.
uniform vec3 lightPos; //The position of the light.
uniform vec3 viewPos; //The position of the view and/or of the player.

void main()
{
	//outputColor = mix(texture(texture1, texCoord),  texture(texture2, texCoord * vec2(1.0, 1.0)), 0.2);
	vec4 outputColor1 = texture(texture1, texCoord);
	
	 //The ambient color is the color where the light does not directly hit the object.
    //You can think of it as an underlying tone throughout the object. Or the light coming from the scene/the sky (not the sun).
    float ambientStrength = 0.3;
    vec3 ambient = ambientStrength * lightColor;

    //We calculate the light direction, and make sure the normal is normalized.
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos); //Note: The light is pointing from the light to the fragment

    //The diffuse part of the phong model.
    //This is the part of the light that gives the most, it is the color of the object where it is hit by light.
    float diff = max(dot(norm, lightDir), 0.0); //We make sure the value is non negative with the max function.
    vec3 diffuse = diff * lightColor;


    //The specular light is the light that shines from the object, like light hitting metal.
    //The calculations are explained much more detailed in the web version of the tutorials.
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32); //The 32 is the shininess of the material.
    vec3 specular = specularStrength * spec * lightColor;

    //At last we add all the light components together and multiply with the color of the object. Then we set the color
    //and makes sure the alpha value is 1
    ////vec3 result = (ambient + diffuse + specular) * objectColor;
	vec3 result = (ambient + diffuse + specular);
    outputColor = vec4(result, 1.0) * vec4(FragColor, 1.0);// * outputColor1;
    
    //Note we still use the light color * object color from the last tutorial.
    //This time the light values are in the phong model (ambient, diffuse and specular)
}