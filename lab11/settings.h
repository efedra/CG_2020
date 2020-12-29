#include <SOIL.h>
#include <glew.h>
#include <freeglut.h>

#pragma once
void set_spotlight_mat() {
    float mat_dif[] = { 0.0f, 0.0f, 0.0f };
    float mat_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat_spec[] = { 0.0f, 0.0f, 0.0f };
    float mat_shininess = 0.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
}

void set_tree_bottom_mat() {
    float mat_dif[] = { 0.4f, 0.4f, 0.0f };
    float mat_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat_spec[] = { 0.0f, 0.0f, 0.0f };
    float mat_shininess = 0.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
}

void set_tree_mat() {
    float mat_dif[] = { 0.2f, 0.9f, 0.3f };
    float mat_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat_spec[] = { 0.0f, 0.0f, 0.0f };
    float mat_shininess = 0.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
}

void set_star_mat() {
    float mat_dif[] = { 0.9f, 0.9f, 0.5f };
    float mat_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat_spec[] = { 1.0f, 1.0f, 1.0f };
    float mat_shininess = 1.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
    glMaterialf(GL_FRONT, GL_EMISSION, 1.0);
}

void set_floor_mat() {
    float mat_dif[] = { 0.5f, 0.5f, 0.5f };
    float mat_amb[] = { 0.2f, 0.2f, 0.2f };
    float mat_spec[] = { 0.5f, 0.5f, 0.5f };
    float mat_shininess = 1.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
}

void set_snow_mat() {
    float mat_dif[] = { 1.0f, 1.0f, 1.0f };
    float mat_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat_spec[] = { 1.0f, 1.0f, 1.0f };
    float mat_shininess = 1.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat_shininess);
}

void set_ball_mat() {
    float mat1_dif[] = { 1.0f, 0.0f, 0.0f };
    float mat1_amb[] = { 1.0f, 1.0f, 1.0f };
    float mat1_spec[] = { 1.0f, 1.0f, 1.0f };

    float mat1_shininess = 1.0f * 128;

    glMaterialfv(GL_FRONT, GL_AMBIENT, mat1_amb);
    glMaterialfv(GL_FRONT, GL_DIFFUSE, mat1_dif);
    glMaterialfv(GL_FRONT, GL_SPECULAR, mat1_spec);
    glMaterialf(GL_FRONT, GL_SHININESS, mat1_shininess);
    glMaterialf(GL_FRONT, GL_EMISSION, 1.0);
}

unsigned int carTexture, ballTexture, earthTexture, treeTexture;

void loadTexture()
{
    auto textureFlags = SOIL_FLAG_MIPMAPS | SOIL_FLAG_INVERT_Y | SOIL_FLAG_NTSC_SAFE_RGB | SOIL_FLAG_COMPRESS_TO_DXT;
    carTexture = SOIL_load_OGL_texture("img/cola.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
    ballTexture = SOIL_load_OGL_texture("img/balls.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
    earthTexture = SOIL_load_OGL_texture("img/earth2.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);
    treeTexture = SOIL_load_OGL_texture("img/tree.jpg", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, textureFlags);


}
