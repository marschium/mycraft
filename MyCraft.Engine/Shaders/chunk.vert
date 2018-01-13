#version 330 core

layout(location = 0) in vec3 modelspace_postion;
layout(location = 1) in vec2 vertex_uv;
layout(location = 2) in vec3 light_data;

uniform mat4 mvp;
uniform vec3 min_light;

out vec2 uv;
out vec3 light;
out float depth;

void main()
{
	gl_Position =  mvp * vec4(modelspace_postion,1);
	uv = vertex_uv;
	light = (light_data / 16) - 0.2;
	light = clamp(light, min_light, vec3(1,1,1));
	depth = length(gl_Position);
}