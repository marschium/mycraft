#version 330 core

layout(location = 8) in vec3 modelspace_vertex_position;
layout(location = 9) in vec3 modelspace_position;
layout(location = 10) in vec4 vertex_colour;

uniform mat4 mvp;

out vec4 frag_color;

void main()
{
	gl_Position =  mvp * vec4(modelspace_vertex_position + modelspace_position, 1);
	frag_color = vertex_colour;
}