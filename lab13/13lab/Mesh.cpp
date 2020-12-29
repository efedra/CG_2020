#define _CRT_SECURE_NO_WARNINGS
#include "Mesh.h"
Mesh::Mesh() {};


bool loadOBJ(const char* fname, vector<vec3>& vertices, vector<vec2>& tex_vertices, vector<vec3>& normals) {
	vector<unsigned int> vertexIndices, uvIndices, normalIndices;
	vector<vec3> temp_vertices;
	vector<vec2> temp_uvs;
	vector<vec3> temp_normals;

	FILE* file = fopen(fname, "r");
	if (file == NULL) {
		getchar();
		return false;
	}
	float maxVal = 0;

	while (true) {
		char lineHeader[128];
		int res = fscanf(file, "%s", lineHeader);
		if (res == EOF)
			break;

		if (strcmp(lineHeader, "v") == 0) {
			vec3 vertex;
			fscanf(file, "%f %f %f\n", &vertex.x, &vertex.y, &vertex.z);
			if (abs(vertex.x) > maxVal)
				maxVal = vertex.x;
			if (abs(vertex.y) > maxVal)
				maxVal = vertex.y;
			if (abs(vertex.z) > maxVal)
				maxVal = vertex.z;
			temp_vertices.push_back(vertex);
		}
		else if (strcmp(lineHeader, "vt") == 0) {
			vec2 uv;
			fscanf(file, "%f %f\n", &uv.x, &uv.y);
			temp_uvs.push_back(uv);
		}
		else if (strcmp(lineHeader, "vn") == 0) {
			vec3 normal;
			fscanf(file, "%f %f %f\n", &normal.x, &normal.y, &normal.z);
			temp_normals.push_back(normal);
		}
		else if (strcmp(lineHeader, "f") == 0) {
			unsigned int vertexIndex[3], uvIndex[3], normalIndex[3];
			int matches = fscanf(file, "%d/%d/%d %d/%d/%d %d/%d/%d\n", &vertexIndex[0], &uvIndex[0], &normalIndex[0], &vertexIndex[1], &uvIndex[1], &normalIndex[1], &vertexIndex[2], &uvIndex[2], &normalIndex[2]);
			vertexIndices.push_back(vertexIndex[0]);
			vertexIndices.push_back(vertexIndex[1]);
			vertexIndices.push_back(vertexIndex[2]);
			uvIndices.push_back(uvIndex[0]);
			uvIndices.push_back(uvIndex[1]);
			uvIndices.push_back(uvIndex[2]);
			normalIndices.push_back(normalIndex[0]);
			normalIndices.push_back(normalIndex[1]);
			normalIndices.push_back(normalIndex[2]);
		}
		else {
			char sBuffer[1000];
			fgets(sBuffer, 1000, file);
		}

	}
	for (unsigned int i = 0; i < vertexIndices.size(); i++) {

		unsigned int vertexIndex = vertexIndices[i];
		unsigned int uvIndex = uvIndices[i];
		unsigned int normalIndex = normalIndices[i];

		vec3 vertex = temp_vertices[vertexIndex - 1];
		vec2 uv = temp_uvs[uvIndex - 1];
		vec3 normal = temp_normals[normalIndex - 1];

		vertices.push_back(vec3(vertex.x / maxVal, vertex.y / maxVal, vertex.z / maxVal));
		tex_vertices.push_back(uv);
		normals.push_back(normal);
	}
	fclose(file);
	return true;
}

GLuint loadTexture(const char* filePath)
{
	int w, h, c;
	unsigned char* data = SOIL_load_image(filePath, &w, &h, &c, SOIL_LOAD_RGB);

	GLuint texture;
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, w, h, 0, GL_RGB, GL_UNSIGNED_BYTE, data);

	glGenerateMipmap(GL_TEXTURE_2D);
	//glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	//glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

	SOIL_free_image_data(data);
	glBindTexture(GL_TEXTURE_2D, 0);

	return texture;
}

void Mesh::FromFile(const char* fname,int count_textures, const char* tex_fname, const char* tex_fname2) {
	vector<vec3> vertsBuff;
	vector<vec2> textBuff;
	vector<vec3> normBuff;

	model_matrix = mat4(1.0);
	normal_transform_matrix = mat4(1.0);

	loadOBJ(fname, vertsBuff, textBuff, normBuff);
	if (count_textures > 0)
	{
		texture1 = loadTexture(tex_fname);
		if (count_textures > 1)
			texture1 = loadTexture(tex_fname2);
	}

	count = vertsBuff.size();
	glGenBuffers(1, &vertex_vbo);
	glBindBuffer(GL_ARRAY_BUFFER, vertex_vbo);
	glBufferData(GL_ARRAY_BUFFER, vertsBuff.size() * sizeof(glm::vec3), &vertsBuff[0], GL_STATIC_DRAW);


	glGenBuffers(1, &texture_vbo);
	glBindBuffer(GL_ARRAY_BUFFER, texture_vbo);
	glBufferData(GL_ARRAY_BUFFER, textBuff.size() * sizeof(glm::vec2), &textBuff[0], GL_STATIC_DRAW);

	glGenBuffers(1, &normal_vbo);
	glBindBuffer(GL_ARRAY_BUFFER, normal_vbo);
	glBufferData(GL_ARRAY_BUFFER, normBuff.size() * sizeof(glm::vec3), &normBuff[0], GL_STATIC_DRAW);
}


void Mesh::SetModelMatrix(const glm::mat4 &model)
{
	model_matrix = glm::mat4(model);
	normal_transform_matrix = glm::transpose(glm::inverse(model_matrix));
}

void Mesh::SetMaterial(const Material & mat)
{
	material = mat;
}

void Mesh::Draw(GLShader & glShader) {

	glShader.setUniform(glShader.getUniformLocation("transform.model"), model_matrix);
	glShader.setUniform(glShader.getUniformLocation("transform.normal"), normal_transform_matrix);
	set_uniform_material(glShader, material);

	if (texture1 != -1)
	{
		glActiveTexture(GL_TEXTURE0);
		glBindTexture(GL_TEXTURE_2D, texture1);
		glShader.setUniform(glShader.getUniformLocation("texture1"), 0);
	}
	if (texture2 != -1)
	{
		glActiveTexture(GL_TEXTURE1);
		glBindTexture(GL_TEXTURE_2D, texture2);
		glShader.setUniform(glShader.getUniformLocation("texture2"), 0);
	}

	glEnableVertexAttribArray(glShader.getAttribLocation("position"));
	glBindBuffer(GL_ARRAY_BUFFER, vertex_vbo);
	glVertexAttribPointer(glShader.getAttribLocation("position"), 3, GL_FLOAT, GL_FALSE, 0, 0);

	glEnableVertexAttribArray(glShader.getAttribLocation("texcoord"));
	glBindBuffer(GL_ARRAY_BUFFER, texture_vbo);
	glVertexAttribPointer(glShader.getAttribLocation("texcoord"), 2, GL_FLOAT, GL_FALSE, 0, 0);

	glEnableVertexAttribArray(glShader.getAttribLocation("normal"));
	glBindBuffer(GL_ARRAY_BUFFER, normal_vbo);
	glVertexAttribPointer(glShader.getAttribLocation("normal"), 3, GL_FLOAT, GL_FALSE, 0, 0);

	glDrawArrays(GL_TRIANGLES, 0, count);

	for (int i = 0; i < 3; i++)
		glDisableVertexAttribArray(i);
}