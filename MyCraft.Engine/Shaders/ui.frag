﻿#version 330 core

out vec4 color;

in vec2 uv;

uniform sampler2D myTextureSampler;

void main()
{
	color = texture(myTextureSampler, uv).rgba;
	if (color.a == 0.0){
		discard;
	}
}