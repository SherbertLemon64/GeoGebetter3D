#version 330 core
layout (location = 0) in vec3 aPosition; // doesn't matter if it has uvs specified as that is defined in the stride

uniform mat4 intermidiaryTransform;
uniform mat4 transform;
uniform mat4 view;
uniform mat4 projection;

out vec3 Position;

void main() {
    Position = aPosition;
    gl_Position = vec4(aPosition, 1.0f) * intermidiaryTransform * transform * view * projection;
}
