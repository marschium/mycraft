#version 330 core

layout(location = 4) in vec3 modelspace_postion;
layout(location = 5) in vec2 vertex_uv;

uniform mat4 mvp;

out vec2 uv;

void main()
{
	gl_Position =  mvp * vec4(modelspace_postion,1);
	uv = vertex_uv;
}