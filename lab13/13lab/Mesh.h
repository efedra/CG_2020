#pragma once
#include "GL\glew.h"
#include "freeglut.h"
#include "GL\SOIL.h"
#include <iostream>
#include <vector>
#include "GLShader.h"
#include "Material.h"

#include "glm/glm.hpp"

using namespace glm;
using namespace std;

class Mesh
{
	GLuint vertex_vbo;
	GLuint texture_vbo;
	GLuint normal_vbo;
	GLuint texture1;
	GLuint texture2;
	int count;//count vertices for drawingArrays

	Material material;

	mat4 model_matrix;
	mat3 normal_transform_matrix;

public:
	Mesh();

	void FromFile(const char* fname, int count_textures=0, const char* tex_fname="", const char* tex_fname2="");

	void Draw(GLShader& glshader);
	void SetModelMatrix(const glm::mat4& model);
	void SetMaterial(const Material& mat);
};

bool loadOBJ(const char* fname, vector<vec3>& vertices, vector<vec2>& tex_vertices, vector<vec3>& normals);