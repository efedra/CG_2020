#include "drawing.h"
vec4 camera_pos; // as point
vec4 camera_dir; // as vector

vec4 car_move;

//keys
void regularKeys(unsigned char key, int x, int y) {
    mat4 rotationMat(1); // Creates a identity matrix
    switch (key) {
    case 'w':
        camera_pos += camera_dir + camera_dir;
        break;
    case 's':
        camera_pos -= camera_dir + camera_dir;
        break;
    case 'a':
        camera_pos -= vec4(cross(vec3(camera_dir), vec3(0.0, 1.0, 0.0)), 1.0)
            + vec4(cross(vec3(camera_dir), vec3(0.0, 1.0, 0.0)), 1.0);
        break;
    case 'd':
        camera_pos += vec4(cross(vec3(camera_dir), vec3(0.0, 1.0, 0.0)), 1.0)
            + vec4(cross(vec3(camera_dir), vec3(0.0, 1.0, 0.0)), 1.0);
        break;
    case 'q':
        mat4 RotationMat(1);
        RotationMat = rotate(RotationMat, radians(5.0f), vec3(0.0, 1.0, 0.0));
        camera_dir = RotationMat * camera_dir;
        break;
    case 'e':
        mat4 RotationMat2(1);
        RotationMat2 = rotate(RotationMat2, radians(-5.0f), vec3(0.0, 1.0, 0.0));
        camera_dir = RotationMat2 * camera_dir;
        break;
    case 'f':
        mat4 RotationMat3(1);
        RotationMat3 = rotate(RotationMat3, radians(5.0f), cross(vec3(0.0, 1.0, 0.0), vec3(camera_dir)));
        camera_dir = RotationMat3 * camera_dir;
        break;
    case 'r':
        mat4 RotationMat4(1);
        RotationMat4 = rotate(RotationMat4, radians(-5.0f), cross(vec3(0.0, 1.0, 0.0), vec3(camera_dir)));
        camera_dir = RotationMat4 * camera_dir;
        break;
    case 'x':
        balls_are_shiny = !balls_are_shiny;
        break;
    }
    glutPostRedisplay();
}

void specialKeys(int key, int x, int y) {
    switch (key) {
    case GLUT_KEY_UP: car_pos += car_move; break;
    case GLUT_KEY_DOWN: car_pos -= car_move; break;
    case GLUT_KEY_LEFT:
        car_angle += 5;
        mat4 RotationMat(1);
        RotationMat = rotate(RotationMat, radians(5.0f), vec3(0.0, 1.0, 0.0));
        car_move = RotationMat * car_move;
        break;
    case GLUT_KEY_RIGHT:
        car_angle -= 5;
        mat4 RotationMat2(1);
        RotationMat2 = rotate(RotationMat2, radians(-5.0f), vec3(0.0, 1.0, 0.0));
        car_move = RotationMat2 * car_move;
        break;

        //case GLUT_KEY_PAGE_UP:; break;
        //case GLUT_KEY_PAGE_DOWN:; break;
    }
    glutPostRedisplay();
}

//on resize
void Reshape(int width, int height) {
    glViewport(0, 0, width, width);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    gluPerspective(65.0f, width / width, 1.0f, 1000.0f);
}
//before main cycle
void Init(void) {
    glClearColor(0.0f, 0.2f, 0.3f, 1.0f);
    camera_pos = vec4(70.0, 70.0, 70.0, 1.0);
    camera_dir = vec4(vec3(-1.0), 1.0);
    car_move = vec4(5.0, 0.0, 0.0, 1.0);

    glEnable(GL_DEPTH_TEST); //z buffer 
    glEnable(GL_LIGHTING); //enabling light
    glLightModeli(GL_LIGHT_MODEL_TWO_SIDE, GL_TRUE);

    glEnable(GL_LIGHT0); 
    GLfloat light0_ambient[] = { 0.0, 0.0, 0.0, 1.0 };
    GLfloat light0_diffuse[] = { 0.7, 0.7, 0.7, 1.0 };
    GLfloat light0_specular[] = { 0.1, 0.1, 0.1, 1.0 };
    GLfloat light0_position[] = { 0.0, 1.0, 0.0, 1.0 };
    glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
    glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
    glLightfv(GL_LIGHT0, GL_SPECULAR, light0_specular);
    glLightfv(GL_LIGHT0, GL_POSITION, light0_position);

    glPointSize(2.0f);

    loadTexture();
}

void Update(void) {
    glMatrixMode(GL_MODELVIEW);
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT); //clear color & depth buffers

    glLoadIdentity();

    gluLookAt(camera_pos.x, camera_pos.y, camera_pos.z,
        camera_dir.x + camera_pos.x, camera_dir.y + camera_pos.y, camera_dir.z + camera_pos.z,
        0.0f, 1.0f, 0.0f);

    DrawObjects();

    glFlush();
    glutSwapBuffers();
}

int main(int argc, char* argv[]) {
    glutInit(&argc, argv); //initializing glut
    glutInitWindowPosition(200, 100); //set position   
    glutInitWindowSize(800, 600); //set size
    glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE | GLUT_DEPTH); //double buff & rgba colors
    glutCreateWindow("OpenGL Christmas task"); //create window with name

    glutIdleFunc(Update); //set function for every frame
    glutDisplayFunc(Update); //set func for drawing every frame
    glutReshapeFunc(Reshape); //set func on resize

    glutKeyboardFunc(regularKeys); //set func for pressing regular keys
    glutSpecialFunc(specialKeys); //set func for pressing special keys
    Init();

    glutMainLoop(); //get into the main cycle
    return 0;
}