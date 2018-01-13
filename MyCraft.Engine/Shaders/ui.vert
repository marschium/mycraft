#version 330 core

layout(location = 6) in vec2 vertex_position_screenspace;
layout(location = 7) in vec2 vertex_uv;

uniform vec2 pos;

out vec2 uv;

void main(){

    // Output position of the vertex, in clip space
    // map [0..800][0..600] to [-1..1][-1..1]
    vec2 vertexPosition_homoneneousspace = (pos + vertex_position_screenspace) - vec2(400,300); // [0..800][0..600] -> [-400..400][-300..300]
    vertexPosition_homoneneousspace /= vec2(400,300);
    gl_Position =  vec4(vertexPosition_homoneneousspace,0,1);

    // UV of the vertex. Flip Y coord
    uv = vertex_uv * vec2(1, -1);
}