#version 330 core

out vec4 color;
in vec4 frag_color;

void main()
{
	//if (in_color.a == 0.0){
	//	discard;
	//}
	color = frag_color;
}