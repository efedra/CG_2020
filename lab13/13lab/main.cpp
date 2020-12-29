#include "GLShader.h"
#include "glm/glm.hpp"
#include "glm/gtc/matrix_transform.hpp"
#include "glm/gtx/transform.hpp"
#include <iostream>
#include "Mesh.h"
#include "Light.h"

using namespace std;

GLShader glShader;


void checkOpenGLerror()
{
	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << gluErrorString(errCode);
}

void initShader()
{
	//glShader.loadFiles("shaders/vertex.txt", "shaders/fragment.txt");
	glShader.loadFiles("shaders/vertex.txt", "shaders/fragment_toon.txt");
	//glShader.loadFiles("shaders/vertex.txt", "shaders/fragment_rim.txt");
	//glShader.loadFiles("shaders/vertex.txt", "shaders/fragment_blinn.txt");
	checkOpenGLerror();
}

std::vector<Mesh> meshes;
void initMeshes()
{
	Mesh mesh;
	mesh.FromFile("tree.obj");//here you can set textures
	mesh.SetMaterial(get_some_material());
	meshes.push_back(mesh);
}

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}

float a = 0;// for rotation

int Width;
int Height;

void render()
{
	a += 0.0015;
	glm::mat4 Projection = glm::perspective(45.0f, (GLfloat)Width / (GLfloat)Height, 1.0f, 200.0f);//these matriñes need changes( you can get this from Christmas lab project)
	glm::mat4 View = glm::lookAt(glm::vec3(4, 3, 3), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	glm::mat4 rotate_x = glm::rotate(0.0f, vec3(1.0f, 0.0f, 0.0f));
	glm::mat4 rotate_y = glm::rotate(a, vec3(0.0f, 1.0f, 0.0f));
	glm::mat4 rotate_z = glm::rotate(0.0f, vec3(0.0f, 0.0f, 1.0f));
	glm::mat4 scale = glm::scale(vec3(3.0f, 3.0f, 3.0f));
	glm::mat4 translate = glm::translate(vec3(0.0f, -0.5f, 0.0f));

	glm::mat4 Model = translate * rotate_x * rotate_y * rotate_z * scale; // this matrix need change for each mesh 
	glm::mat4 ViewProjection = Projection * View;

	meshes[0].SetModelMatrix(Model);

	glShader.use();
	glShader.setUniform(glShader.getUniformLocation("transform.viewProjection"), ViewProjection);
	glShader.setUniform(glShader.getUniformLocation("transform.viewPosition"), vec3(4,3,3));

	set_uniform_point_light(glShader, get_some_point_light());
	for(int i=0; i<meshes.size();i++)
		meshes[i].Draw(glShader);
	glutSwapBuffers();
}



void Init()
{
	initMeshes();
	initShader();
}

int main(int argc, char** argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DEPTH | GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(1000, 800);
	glutCreateWindow("Simple shaders");
	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LESS);

	Width = 1000;
	Height = 800;
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
		std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
		return 1;
	}
	if (!GLEW_VERSION_2_0)
	{
		std::cout << "No support for OpenGL 2.0 found\n";
		return 1;
	}
	Init();
	glutReshapeFunc(resizeWindow);
	glutIdleFunc(render);
	glutDisplayFunc(render);
	glutMainLoop();
}