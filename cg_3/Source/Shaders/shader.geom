﻿#version 330 core

layout (lines) in;
layout (line_strip, max_vertices = 2) out;

void main() {
    gl_PointSize = 3;
    gl_Position = gl_in[0].gl_Position;
    EmitVertex();
    gl_Position = gl_in[1].gl_Position;
    EmitVertex();
    EndPrimitive();
}     