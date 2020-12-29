//#include "GLShader.h"
//#include <iostream>
//
//
////! ���������� � ����������������� ID
////! ID ��������� ���������
//GLuint Program;
////! ID ������� ���������� �����
//GLint unif_rotation, attr_coord;
//float rotate_x = 0;
//
////! �������� ������ OpenGL, ���� ���� �� ����� � ������� ��� ������
//void checkOpenGLerror() {
//    GLenum errCode;
//    if ((errCode = glGetError()) != GL_NO_ERROR)
//        std::cout << "OpenGl error! - " << gluErrorString(errCode);
//}
////! ������������� ��������
//void initShader()
//{
//    //! �������� ��� ��������
//    const char* vsSource =
//        "uniform vec3 rotation;\n"
//        "attribute vec2 coord;\n"
//        "mat3 rot(in float a) {return mat3(1,0,0,0,cos(a), -sin(a),0, sin(a),cos(a));}\n"
//        "varying vec4 v_color;\n"
//        "void main() {\n"
//        "vec3 pos = rot(rotation.x)*vec3(coord.xy, 0.0);\n"
//        " gl_Position = vec4(pos, 1.0);\n"
//        " v_color =gl_Color+ vec4(1.0,0.0,0.0,1.0);\n"
//        "}\n";
//    const char* fsSource =
//        "varying vec4 v_color;\n"
//        "void main() {\n"
//        " if (mod(gl_FragCoord.x, 10)<5.0) \n"
//        " gl_FragColor = v_color;\n"
//        " else\n"
//        " gl_FragColor = vec4(1.0,1.0,1.0,0.0);\n"
//        "}\n";
//    //! ���������� ��� �������� ��������������� ��������
//    GLuint vShader, fShader;
//
//    //! ������� ��������� ������
//    vShader = glCreateShader(GL_VERTEX_SHADER);
//    //! �������� �������� ���
//    glShaderSource(vShader, 1, &vsSource, NULL);
//    //! ����������� ������
//    glCompileShader(vShader);
//
//    //! ������� ����������� ������
//    fShader = glCreateShader(GL_FRAGMENT_SHADER);
//    //! �������� �������� ���
//    glShaderSource(fShader, 1, &fsSource, NULL);
//    //! ����������� ������
//    glCompileShader(fShader);
//    //! ������� ��������� � ����������� ������� � ���
//    Program = glCreateProgram();
//    glAttachShader(Program, vShader);
//    glAttachShader(Program, fShader);
//    //! ������� ��������� ���������
//    glLinkProgram(Program);
//    //! ��������� ������ ������
//    int link_ok;
//    glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
//    if (!link_ok)
//    {
//        std::cout << "error attach shaders \n";
//        return;
//    }
//    //! ���������� ID �������
//    const char* unif_name = "rotation";
//    unif_rotation = glGetUniformLocation(Program, unif_name);
//    if (unif_rotation == -1)
//    {
//        std::cout << "could not bind uniform " << unif_name << std::endl;
//        return;
//    }
//
//    const char* attr_name = "coord";
//    attr_coord = glGetAttribLocation(Program, attr_name);
//    if (attr_coord == -1)
//    {
//        std::cout << "could not bind attr " << attr_name << std::endl;
//        return;
//    }
//
//    checkOpenGLerror();
//}
////! ������������ ��������
//void freeShader()
//{
//    //! ��������� ����, �� ��������� �������� ���������
//    glUseProgram(0);
//    //! ������� ��������� ���������
//    glDeleteProgram(Program);
//}
//void resizeWindow(int width, int height)
//{
//    glViewport(0, 0, width, height);
//}
////! ���������
//void render2()
//{
//    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//    glLoadIdentity();
//    //! ������������� ��������� ��������� �������
//    glUseProgram(Program);
//    //! �������� ������� � ������
//    float rotation[3] = {rotate_x,0.0,0.0};
//    glUniform3fv(unif_rotation,1, rotation);
//    glBegin(GL_TRIANGLES);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(-0.5f, -0.5f);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(-0.5f, 0.5f);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(0.5f, 0.5f);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(-0.5f, -0.5f);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(0.5f, 0.5f);
//    glColor4f(1.0, 0.0, 0.0, 1.0); glVertex2f(0.5f, -0.5f);
//    glEnd();
//    glFlush();
//    //! ��������� ��������� ���������
//    glUseProgram(0);
//    checkOpenGLerror();
//    glutSwapBuffers();
//}
//void specialKeys(int key, int x, int y) {
//
//    switch (key) {
//    case GLUT_KEY_PAGE_UP: rotate_x += 5; break;
//    case GLUT_KEY_PAGE_DOWN: rotate_x -= 5; break;
//    }
//    glutPostRedisplay();
//}
//int main(int argc, char** argv)
//{
//    glutInit(&argc, argv);
//    glutInitDisplayMode(GLUT_DEPTH | GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
//    glutInitWindowSize(800, 800);
//    glutCreateWindow("Simple shaders");
//    glClearColor(0, 0, 1, 0);
//    //! ����������� ����� �������������� ��������
//    GLenum glew_status = glewInit();
//    if (GLEW_OK != glew_status)
//    {
//        //! GLEW �� ���������������������
//        std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
//        return 1;
//    }
//    //! ��������� ����������� OpenGL 2.0
//    if (!GLEW_VERSION_2_0)
//    {
//        //! OpenGl 2.0 ��������� �� ��������
//        std::cout << "No support for OpenGL 2.0 found\n";
//        return 1;
//    }
//    //! ������������� ��������
//    initShader();
//    glutReshapeFunc(resizeWindow);
//    glutDisplayFunc(render2);
//    glutSpecialFunc(specialKeys);
//    glutMainLoop();
//    //! ������������ ��������
//    freeShader();
//}