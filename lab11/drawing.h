#include "settings.h"

#include "glm\vec4.hpp"
#include "glm\mat4x4.hpp"
#include "glm/ext/matrix_transform.hpp"
#include "glm/geometric.hpp"

using namespace glm;

float snow_pos = 0.0;
float tree_angle = 0.0;
void DrawSnow() {
    set_snow_mat();
    srand(0);
    float r = 1.0, g = 1.0, b = 1.0;
    glColor3f(r, g, b);
    //glRotatef(tree_angle, 0.0f, 1.0f, 0.0f);
    glTranslatef(0, snow_pos, 0);
    snow_pos -= 0.01;
    glBegin(GL_POINTS);
    for (int i1 = 500; i1 < 700; i1++)
    {
        int i = i1 * 0.01;
        int x = rand() % 10;
        int y = rand() % 10;
        int z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(x * i, y * i, -z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(-x * i, y * i, z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(-x * i, y * i, -z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(-x * i, y * i, z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(x * i, y * i, -z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(x * i, y * i, z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(x * i, y * i, -z * i);
        x = rand() % 10;
        y = rand() % 10;
        z = rand() % 10;
        glColor3f(r, g, b); glVertex3f(x * i, y * i, z * i);
    }
    glEnd();
}

void DrawFloor() {  
    glBindTexture(GL_TEXTURE_2D, earthTexture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_REPEAT);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

    glEnable(GL_TEXTURE_2D);
    
    set_floor_mat();
    glBegin(GL_QUADS);
    for (int i = -10; i < 10; i++) 
        for (int j = -10; j < 10; j++) {
            glNormal3f(0.0, 1.0, 0.0); glTexCoord2f(0.0, 0.0); glVertex3f(i * 5, 0.1, j * 5);
            glNormal3f(0.0, 1.0, 0.0);glTexCoord2f(0.0, 1.0); glVertex3f(i * 5, 0.1, (j + 1) * 5);
            glNormal3f(0.0, 1.0, 0.0);glTexCoord2f(1.0, 1.0); glVertex3f((i + 1) * 5, 0.1, (j + 1) * 5);
            glNormal3f(0.0, 1.0, 0.0); glTexCoord2f(1.0, 0.0); glVertex3f((i + 1) * 5, 0.1, j * 5);
        }
    glEnd();
    glDisable(GL_TEXTURE_2D);
}


void DrawStar() {
    glutSolidCone(3, 5, 4, 4);
    glRotatef(180, 0, 1, 0);
    glutSolidCone(3, 5, 4, 4);
}

bool balls_are_shiny = false;
void DrawTreeBalls(float radius) {
    set_ball_mat();
    glPushMatrix();
    radius -= 5;
    glTranslatef(0, 0, 10.0);


    glBindTexture(GL_TEXTURE_2D, ballTexture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_REPEAT);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

    glEnable(GL_TEXTURE_2D);

    GLUquadricObj* quadObj = gluNewQuadric();
    gluQuadricTexture(quadObj, GL_TRUE);
    gluQuadricDrawStyle(quadObj, GLU_FILL);

    float x = 0, y = 0;
    for (int i = 0; i < 12; i++) {
        gluSphere(quadObj, 2, 10, 10);
        glTranslatef(-x, -y, 0.0);
        x = radius * cos(36 * i);
        y = radius * sin(36 * i);
        glTranslatef(x, y, 0.0);
    }
    
    gluDeleteQuadric(quadObj);
    glDisable(GL_TEXTURE_2D);
    glPopMatrix();

    set_ball_mat();
    glPushMatrix();
    radius -= 3.5;
    glTranslatef(0, 0, 14);
    x = 0, y = 0;
    for (int i = 0; i < 12; i++) {
        glutSolidSphere(1, 10, 10);
        glTranslatef(-x, -y, 0.0);
        x = radius * cos(36 * i);
        y = radius * sin(36 * i);
        glTranslatef(x, y, 0.0);
    }
    glPopMatrix();

    glPushMatrix();
    radius += 6;
    glTranslatef(0, 0, 5);
    x = 0, y = 0;
    for (int i = 0; i < 12; i++) {
        glutSolidSphere(1, 10, 10);
        glTranslatef(-x, -y, 0.0);
        x = radius * cos(36 * i);
        y = radius * sin(36 * i);
        glTranslatef(x, y, 0.0);
    }
    glPopMatrix();
}

void DrawChristmasTreeCone(double radius, double height, double stacks)
{
    glPushMatrix();
    set_tree_mat();
    glBindTexture(GL_TEXTURE_2D, treeTexture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_REPEAT);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

    glEnable(GL_TEXTURE_2D);

    glBegin(GL_TRIANGLE_FAN);
    glTexCoord2f(0.5, 0);  glVertex3f(0, 0, height);

    auto ind = 0;
    float PI = glm::pi<float>();
    float angle = 2* PI / stacks;
    for (float angl = 0; angl <= 2* PI+angle; angl += angle)
    {
        auto x = radius * cos(angl);
        auto y = radius * sin(angl);
        float u, v;
        if (ind % 2 == 0)
        {
            u = 0.3;
            v = 1.0;
        }
        else
        {
            u = 0.0;
            v = 1.0;
        }
        ind++;
        glTexCoord2f(u, v); glVertex3f(x, y, 0);
    }
    glEnd();
    set_tree_mat();
    glBegin(GL_TRIANGLE_FAN);
    glNormal3f(0.0, -1.0, 0.0);  glTexCoord2f(0.5, 0);  glVertex3f(0, 0, 0);

    ind = 0;
    for (float angl = 0; angl <= 2 * PI + angle; angl += angle)
    {
        auto x = radius * cos(angl);
        auto y = radius * sin(angl);
        float u, v;
        if (ind % 2 == 0)
        {
            u = 0.3;
            v = 1.0;
        }
        else
        {
            u = 0.0;
            v = 1.0;
        }
        ind++;
        glNormal3f(0.0, -1.0, 0.0); glTexCoord2f(u, v); glVertex3f(x, y, 0);
    }
    glEnd();

    glDisable(GL_TEXTURE_2D);
    glPopMatrix();
}


//float tree_angle = 0.0f;
void DrawTree() {
    tree_angle += 0.1;

    set_tree_bottom_mat();

    glRotatef(tree_angle, 0.0, 1.0, 0.0);
    glRotatef(270.0, 1.0, 0.0, 0.0);
    gluCylinder(gluNewQuadric(), 10, 10, 40, 15, 15);

    glTranslatef(0.0, 0.0, 10.0);
    DrawTreeBalls(25);
    DrawChristmasTreeCone(25, 40, 10);


    glTranslatef(0.0, 0.0, 20.0);
    DrawTreeBalls(20);
    DrawChristmasTreeCone(20, 30, 10);


    glTranslatef(0.0, 0.0, 15.0);
    DrawTreeBalls(15);
    DrawChristmasTreeCone(15, 25, 10);

    glTranslatef(0.0, 0.0, 30);
    set_star_mat();
    DrawStar();
}

void DrawSpotlights() {
    //---------------------------------1---------------------------------------
    glPushMatrix();
    glTranslatef(-40, 40, -40);

    glPushMatrix();
    glRotatef(90.0, 1.0, 0.0, 0.0);
    set_spotlight_mat();
    gluCylinder(gluNewQuadric(), 1, 1, 40, 10, 10);
    set_star_mat();
    DrawStar();
    glPopMatrix();

    glEnable(GL_LIGHT1);

    GLfloat light_diffuse[] = { 1.0, 1.0, 1.0 };
    GLfloat light_position[] = { 0.0, 0.0, 0.0, 1.0 };
    GLfloat light_spot_direction[] = { 0.0, -1.0, 0.0 };
    GLfloat tttt[] = { 60.0f };
    glLightfv(GL_LIGHT1, GL_DIFFUSE, light_diffuse);
    glLightfv(GL_LIGHT1, GL_POSITION, light_position);
    glLightf(GL_LIGHT1, GL_SPOT_CUTOFF, 60);
    glLightfv(GL_LIGHT1, GL_SPOT_DIRECTION, light_spot_direction);
    glLightfv(GL_LIGHT1, GL_SPOT_EXPONENT, tttt);

    glutWireCube(7.5);
    glPopMatrix();
    //---------------------------------2---------------------------------------
    glPushMatrix();
    glTranslatef(-40, 40, 40);

    glPushMatrix();
    glRotatef(90.0, 1.0, 0.0, 0.0);
    set_spotlight_mat();
    gluCylinder(gluNewQuadric(), 1, 1, 40, 10, 10);
    set_star_mat();
    DrawStar();
    glPopMatrix();

    glEnable(GL_LIGHT2);
    glLightfv(GL_LIGHT2, GL_DIFFUSE, light_diffuse);
    glLightfv(GL_LIGHT2, GL_POSITION, light_position);
    glLightf(GL_LIGHT2, GL_SPOT_CUTOFF, 20);
    glLightfv(GL_LIGHT2, GL_SPOT_DIRECTION, light_spot_direction);
    glLightfv(GL_LIGHT2, GL_SPOT_EXPONENT, tttt);

    glutWireCube(7.5);
    glPopMatrix();
    //---------------------------------3---------------------------------------
    glPushMatrix();
    glTranslatef(40, 40, 40);

    glPushMatrix();
    glRotatef(90.0, 1.0, 0.0, 0.0);
    set_spotlight_mat();
    gluCylinder(gluNewQuadric(), 1, 1, 40, 10, 10);
    set_star_mat();
    DrawStar();
    glPopMatrix();

    glEnable(GL_LIGHT3);
    glLightfv(GL_LIGHT3, GL_DIFFUSE, light_diffuse);
    glLightfv(GL_LIGHT3, GL_POSITION, light_position);
    glLightf(GL_LIGHT3, GL_SPOT_CUTOFF, 20);
    glLightfv(GL_LIGHT3, GL_SPOT_DIRECTION, light_spot_direction);
    glLightfv(GL_LIGHT3, GL_SPOT_EXPONENT, tttt);

    glutWireCube(7.5);
    glPopMatrix();
    //---------------------------------4---------------------------------------
    glPushMatrix();
    glTranslatef(40, 40, -40);

    glPushMatrix();
    glRotatef(90.0, 1.0, 0.0, 0.0);
    set_spotlight_mat();
    gluCylinder(gluNewQuadric(), 1, 1, 40, 10, 10);
    set_star_mat();
    DrawStar();
    glPopMatrix();


    glEnable(GL_LIGHT4);
    glLightfv(GL_LIGHT4, GL_DIFFUSE, light_diffuse);
    glLightfv(GL_LIGHT4, GL_POSITION, light_position);
    glLightf(GL_LIGHT4, GL_SPOT_CUTOFF, 20);
    glLightfv(GL_LIGHT4, GL_SPOT_DIRECTION, light_spot_direction);
    glLightfv(GL_LIGHT4, GL_SPOT_EXPONENT, tttt);

    glutWireCube(7.5);
    glPopMatrix();
}

vec4 car_pos(0.0);
float car_angle = 0.0;

void DrawCar() {
    float red_mat_dif[] = { 1.0f, 0.0f, 0.0f }; 
    float black_mat_dif[] = { 0.0f, 0.0f, 0.0f };
    float darkred_mat_dif[] = { 0.6f, 0.0f, 0.0f }; 
    float yellow_mat_dif[] = { 1.0f, 1.0f, 0.5f }; 

    float mat1_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat1_spec[] = { 0.0f, 0.0f, 0.0f };
    float mat1_shininess = 0.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat1_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, red_mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat1_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat1_shininess);

    glTranslatef(car_pos.x, car_pos.y, car_pos.z);
    glRotatef(car_angle, 0.0, 1.0, 0.0);

    glBindTexture(GL_TEXTURE_2D, carTexture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_REPEAT);
    glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);

    glEnable(GL_TEXTURE_2D);

    glBegin(GL_QUADS);
    glNormal3f(0.0, 0.0, 1.0);  glTexCoord2f(0.0, 0.0); glVertex3f(6.25,-6.25, -6.35);// чтобы не перекрывалось с куском машины
    glNormal3f(0.0, 0.0, 1.0); glTexCoord2f(0.0, 1.0); glVertex3f(6.25,6.25, -6.35);
    glNormal3f(0.0, 0.0, 1.0); glTexCoord2f(1.0, 1.0); glVertex3f(-6.25, 6.25, -6.35);
    glNormal3f(0.0, 0.0, 1.0); glTexCoord2f(1.0, 0.0); glVertex3f(-6.25,-6.25, -6.35);
    
    glNormal3f(0.0, 0.0, -1.0); glTexCoord2f(0.0, 0.0); glVertex3f(-6.25, -6.25, 6.35);
    glNormal3f(0.0, 0.0, -1.0); glTexCoord2f(0.0, 1.0); glVertex3f(-6.25, 6.25, 6.35);
    glNormal3f(0.0, 0.0, -1.0); glTexCoord2f(1.0, 1.0); glVertex3f(6.25, 6.25, 6.35);
    glNormal3f(0.0, 0.0, -1.0); glTexCoord2f(1.0, 0.0); glVertex3f(6.25, -6.25, 6.35);
    
    glEnd();

    glDisable(GL_TEXTURE_2D);


    glutSolidCube(12.5);

    // wheels
    glMaterialfv(GL_FRONT, GL_DIFFUSE, black_mat_dif);

    glTranslatef(3.75, -6.25, 6.25);
    glutSolidTorus(0.5, 1.25, 15, 15);
    glTranslatef(-7.5, 0, 0);
    glutSolidTorus(0.5, 1.25, 15, 15);
    glTranslatef(0, 0, -12.5);
    glutSolidTorus(0.5, 1.25, 15, 15);
    glTranslatef(7.5, 0, 0);
    glutSolidTorus(0.5, 1.25, 15, 15);

    // front
    glMaterialfv(GL_FRONT, GL_DIFFUSE, darkred_mat_dif);

    glTranslatef(5, 2.5, 3.75);
    glutSolidCube(5);
    glTranslatef(0, 5, 0);
    glutSolidCube(5);
    glTranslatef(0, 0, 5);
    glutSolidCube(5);
    glTranslatef(0, -5, 0);
    glutSolidCube(5);

    // wheels front
    glMaterialfv(GL_FRONT, GL_DIFFUSE, black_mat_dif);

    glTranslatef(0, -2.5, 2.5);
    glutSolidTorus(0.5, 1.25, 15, 15);
    glTranslatef(0, 0, -10);
    glutSolidTorus(0.5, 1.25, 15, 15);

    // more front
    glMaterialfv(GL_FRONT, GL_DIFFUSE, darkred_mat_dif);

    glTranslatef(3.75, 2.5, 2.5);
    glutSolidCube(5);
    glTranslatef(0, 0, 5);
    glutSolidCube(5);


    //lights
    glMaterialfv(GL_FRONT, GL_DIFFUSE, yellow_mat_dif);
    glMaterialf(GL_FRONT, GL_EMISSION, 1.0);

    glTranslatef(2.5, 0, 1.25);
    glutSolidCube(0.5);

    GLfloat light5_diffuse[] = { 1.0, 1.0, 1.0 };
    GLfloat light5_position[] = { 0.0, 0.0, 1.0, 1.0 };
    GLfloat light5_spot_direction[] = { 1.0, -0.1, 0.0 };
    GLfloat tttt[] = { 20 };

    glEnable(GL_LIGHT5);

    glLightfv(GL_LIGHT5, GL_DIFFUSE, light5_diffuse);
    glLightfv(GL_LIGHT5, GL_POSITION, light5_position);
    glLightf(GL_LIGHT5, GL_SPOT_CUTOFF, 20);
    glLightf(GL_LIGHT5, GL_LINEAR_ATTENUATION, 0.04);
    glLightfv(GL_LIGHT5, GL_SPOT_DIRECTION, light5_spot_direction);
    glLightfv(GL_LIGHT5, GL_SPOT_EXPONENT, tttt);

    glTranslatef(0, 0, -7.5);
    glutSolidCube(0.5);

    glEnable(GL_LIGHT6);
    GLfloat light6_position[] = { 0.0, 0.0, 1.0, 1.0 };
    glLightfv(GL_LIGHT6, GL_DIFFUSE, light5_diffuse);
    glLightfv(GL_LIGHT6, GL_POSITION, light6_position);
    glLightf(GL_LIGHT6, GL_SPOT_CUTOFF, 20);
    glLightf(GL_LIGHT6, GL_LINEAR_ATTENUATION, 0.04);
    glLightfv(GL_LIGHT6, GL_SPOT_DIRECTION, light5_spot_direction);
    glLightfv(GL_LIGHT6, GL_SPOT_EXPONENT, tttt);

    glMaterialf(GL_FRONT, GL_EMISSION, 0.0);
    glPopMatrix();
}

void DrawObjects() {
    glPushMatrix();
    DrawSnow();
    glPopMatrix();

    glPushMatrix();
    DrawFloor();
    glPopMatrix();

    glPushMatrix();
    DrawTree();
    glPopMatrix();

    glPushMatrix();
    DrawSpotlights();
    glPopMatrix();

    /*glPushMatrix();
    DrawTree();
    glPopMatrix();*/

    glPushMatrix();
    glTranslatef(40.0, 10.0, 0.0);
    DrawCar();
    glPopMatrix();

    glutPostRedisplay();
}