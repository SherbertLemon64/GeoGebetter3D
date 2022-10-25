#version 330 core

uniform vec4 colour;

out vec4 FragColour;
in vec3 Position;

void main() {
    FragColour = vec4((Position + 1.0) * 0.5, 1.0);
}
