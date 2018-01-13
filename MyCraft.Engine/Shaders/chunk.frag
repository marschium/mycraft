#version 330 core

out vec4 color;

uniform float fog_density;
uniform vec3 fog_colour;

in vec2 uv;
in vec3 light;
in float depth;

uniform sampler2D myTextureSampler;

vec3 calc_fog(vec3 color, float depth){
    const float e = 2.71828182845904523536028747135266249;
    float f = pow(e, -pow(depth * fog_density, 2));
    return mix(fog_colour, color, clamp(f, 0, 1));
}       


void main()
{
	vec4 c = texture(myTextureSampler, uv).rgba;
	if (c.a < 0.5){
		discard;
	}
	c =  c * vec4(light, 1);
	color = vec4(calc_fog(c.rgb, depth), c.a);
}