#ifndef LIGHT
#define LIGHT
#include "glm/glm.hpp"
#include "GLShader.h"

struct PointLight
{
	glm::vec4 position;
	glm::vec4 ambient;
	glm::vec4 diffuse;
	glm::vec4 specular;
	glm::vec3 attenuation;
};

PointLight get_some_point_light();
void set_uniform_point_light(GLShader& glShader, PointLight l);


struct DirectLight
{
	glm::vec4 direction;
	glm::vec4 ambient;
	glm::vec4 diffuse;
	glm::vec4 specular;
};

DirectLight get_some_direction_light();
void set_uniform_direct_light(GLShader& glShader, DirectLight l);

struct Spotlight
{
	glm::vec4 position;
	glm::vec4 ambient;
	glm::vec4 diffuse;
	glm::vec4 specular;
	glm::vec3 attenuation;
	glm::vec4 spotdirection;
	float spotcutoff;
	float spotexponent;
};

Spotlight get_some_spotlight();
void set_uniform_direct_light(GLShader& glShader, Spotlight l);

#endif // !LIGHT
