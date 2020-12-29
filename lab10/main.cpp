#include <iostream>
#include <gl\freeglut.h>
#include <GL/glut.h> 


using namespace std;

double w = 500, h = 500;

//-----------------Задание 1---------------------------------


//void Init(void)// вызываемая перед вхождением в главный цикл
//{
//	glClearColor(0.0f, 0.0f, 1.0f, 1.0f);// цвет очистки экрана
//}
//
//void Update(void)// каждый раз при пересоке окна очищает окно цветом, который мы задали в инит
//{
//	glClear(GL_COLOR_BUFFER_BIT);
//	glutSwapBuffers();// меняются буферы местами Один на экране, а во втором прорисовка
//}
//
//void Reshape(int width, int height) {// вызывается при изменении размеров окна
//	w = width; h = height;
//}
//
//int main(int argc, char * argv[]) 
//{
//	//Инициализировать сам glut
//	glutInit(&argc, argv);
//	//Установить начальное положение окна
//	glutInitWindowPosition(100, 100);
//	//Установить начальные размеры окна
//	glutInitWindowSize(800, 600);
//	//Установить параметры окна - двойная буфферизация
//	// и поддержка цвета RGBA
//	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE);
//	//Создать окно с заголовком OpenGL
//	glutCreateWindow("OpenGL");
//	// Функция, которая будет вызываться каждый кадр
//	glutIdleFunc(Update);
//	//Укажем glut функцию, которая будет рисовать каждый кадр
//	glutDisplayFunc(Update);
//	//Укажем glut функцию, которая будет вызываться при
//	// изменении размера окна приложения
//	glutReshapeFunc(Reshape);
//	Init();
//	//Войти в главный цикл приложения
//	glutMainLoop(); // ну и дальше запускаются наши обработчики 
//	return 0;
//}



//----------------------Задание 2-----------------------

//double rotate_x = 0;
//double rotate_y = 0;
//double rotate_z = 0;
//
//// Функция вызываемая перед вхождением в главный цикл
//void Init(void) {
//	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
//}
//
////Функця вызываемая при изменении размеров окна
//void Reshape(int width, int height) {
//	w = width; h = height;
//}
//
//void renderRectangle()
//{
//	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//	glLoadIdentity();//загружает единичную матрицу
//
//	//угол, и вектор вокруг которого произведен поворот
//	glRotatef(rotate_z, 0.0, 0.0, 1.0);//все домнажается на ед матрицу слева
//	glBegin(GL_QUADS);
//	glColor3f(1.0, 0.0, 0.0); glVertex2f(-0.5f, -0.5f);
//	glColor3f(0.0, 1.0, 0.0); glVertex2f(-0.5f, 0.5f);
//	glColor3f(0.0, 0.0, 1.0); glVertex2f(0.5f, 0.5f);
//	glColor3f(1.0, 1.0, 1.0); glVertex2f(0.5f, -0.5f);
//	glEnd();
//	glFlush(); glutSwapBuffers();
//}
//
//void specialKeys1(int key, int x, int y) {
//	switch(key) {
//case GLUT_KEY_UP: rotate_z += 10; break;
//case GLUT_KEY_DOWN: rotate_z -= 10; break;
//	}
//	glutPostRedisplay();
//}
//
//
//int main(int argc, char** argv) {
//	glutInit(&argc, argv);
//	glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);
//	glutInitWindowPosition(100, 100);
//	glutInitWindowSize(600, 600);
//	glutCreateWindow(" OpenGL ");
//
//	glutSpecialFunc(specialKeys1);
//	glutDisplayFunc(renderRectangle);
//
//	//Укажем glut функцию, которая будет вызываться при
//	// изменении размера окна приложения
//	glutReshapeFunc(Reshape);
//	Init();
//	glutMainLoop();
//	return 0;
//}


//-----------------------Задание 3------------------

//double rotate_x = 0;
//double rotate_y = 0;
//double rotate_z = 0;
//
//void Init(void)
//{
//	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
//}
//
//void specialKeys(int key, int x, int y) {
//		switch(key) {
//		case GLUT_KEY_UP: rotate_x += 5; break;
//		case GLUT_KEY_DOWN: rotate_x -= 5; break;
//		case GLUT_KEY_RIGHT: rotate_y += 5; break;
//		case GLUT_KEY_LEFT: rotate_y -= 5; break;
//		case GLUT_KEY_PAGE_UP: rotate_z += 5; break;
//		case GLUT_KEY_PAGE_DOWN: rotate_z -= 5; break;
//		}
//		glutPostRedisplay();
//}
//
//void Reshape(int width, int height)
//{
//	w = width; h = height;
//}
//
//void renderWireCube2d() {
//	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//	glLoadIdentity();
//	glRotatef(rotate_x, 1.0, 0.0, 0.0);
//	glRotatef(rotate_y, 0.0, 1.0, 0.0);
//	glRotatef(rotate_z, 0.0, 0.0, 1.0);
//
//	glutWireCube(1);//соединеняем
//
//	glFlush();
//	glutSwapBuffers();
//}
//
//void renderWireCube3d() {
//	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//	glLoadIdentity();
//	glRotatef(rotate_x, 1.0, 0.0, 0.0);
//	glRotatef(rotate_y, 0.0, 1.0, 0.0);
//	glRotatef(rotate_z, 0.0, 0.0, 1.0);
//
//	glutSolidCube(1);//соединяем
//
//	glFlush();
//	glutSwapBuffers();
//}
//
//
//int main(int argc, char** argv)
//{
//	glutInit(&argc, argv);
//	glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);
//	glutInitWindowPosition(100, 100);
//	glutInitWindowSize(600, 600);
//	glutCreateWindow("Task 3");
//
//	glutSpecialFunc(specialKeys);
//	glutReshapeFunc(Reshape);
//	//glutDisplayFunc(renderWireCube2d);
//	glutDisplayFunc(renderWireCube3d);
//	Init();
//	glutMainLoop();
//	return 0;
//}

//-------------------Задание 4-----------------------

//double rotate_x = 0;
//double rotate_y = 0;
//double rotate_z = 0;
//double Angle = 0;
//
//void Init(void)
//{
//	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
//}
//
//void Reshape(int wid, int hei)
//{
//	w = wid;
//	h = hei;
//	//добавление перспективы. Настройка матрицы перспективы 
//	glViewport(0, 0, w, h);
//	glMatrixMode(GL_PROJECTION);
//	glLoadIdentity();
//
//	gluPerspective(65.0f, w / h, 1.0f, 1000.0f); //для камеры , угол обзора
//}
//
//void specialKeys(int key, int x, int y) {
//			switch(key) {
//			case GLUT_KEY_UP: rotate_x += 30; break;
//			case GLUT_KEY_DOWN: rotate_x -= 30; break;
//			case GLUT_KEY_RIGHT: rotate_y += 30; break;
//			case GLUT_KEY_LEFT: rotate_y -= 30; break;
//			case GLUT_KEY_PAGE_UP: rotate_z += 30; break;
//			case GLUT_KEY_PAGE_DOWN: rotate_z -= 30; break;
//			}
//			glutPostRedisplay();
//}
//
//void renderSphere()
//{
//	//Текущая - матрица видового преобразования
//	glMatrixMode(GL_MODELVIEW);
//	Angle += 0.3f; //Увеличиваем угол поворота
//	glClear(GL_COLOR_BUFFER_BIT); //Очистим буфер цвета
//	glLoadIdentity(); //Загрузим едматрицу. вида
//	//Зададим позицию камеры, точку наблюд. и вектор вверх
//	gluLookAt(100.0f, 100.0f, 100.0f,
//		0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
//	//Применим матрицу поворота на текущую матрицу
//	glRotatef(Angle, 0.0f, 1.0f, 0.0f);
//	//Отрисуем сферу радиусом 50 ед, размером 10 x10 полигонов
//	glutWireSphere(50.0f, 10,10);
//	//Сбросить все данные на обработку в конвейер преобразования
//	// OpenGL без ожидания завершения предыдущих инструкций
//	glFlush();
//	glutSwapBuffers();
//
//}
//
//void renderTeapot(void)
//{
//	//Текущая - матрица видового преобразования
//	glMatrixMode(GL_MODELVIEW);
//	Angle += 0.3f; //Увеличиваем угол поворота
//	glClear(GL_COLOR_BUFFER_BIT); //Очистим буфер цвета
//	glLoadIdentity(); //Загрузим едматрицу. вида
//	//Зададим позицию камеры, точку наблюд. и вектор вверх
//	gluLookAt(100.0f, 100.0f, 100.0f,
//		0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
//	//Применим матрицу поворота на текущую матрицу
//	glRotatef(Angle, 0.0f, 1.0f, 0.0f);
//
//	glutWireTeapot(40);
//
//	//Сбросить все данные на обработку в конвейер преобразования
//	// OpenGL без ожидания завершения предыдущих инструкций
//	glFlush();
//	glutSwapBuffers();
//}
//
//int main(int argc, char** argv) {
//	glutInit(&argc, argv);
//	glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);
//	glutInitWindowSize(600, 600);
//	glutInitWindowPosition(100, 100);
//	glutCreateWindow("Task 4");
//
//
//	glutSpecialFunc(specialKeys);
//	glutReshapeFunc(Reshape);
//	//glutDisplayFunc(renderSphere);
//	glutDisplayFunc(renderTeapot);
//	Init();
//	glutMainLoop();
//	return 0;
// }


//-------------------------Задание 5---------------------------

//void Init(void)
//{
//	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
//	glPointSize(10.0f);
//}
//
//void Reshape(int width, int height)
//{
//	w = width; h = height;
//	glViewport(0, 0, w, h);
//	glMatrixMode(GL_PROJECTION);
//	glLoadIdentity();
//	gluPerspective(65.0f, w / h, 1.0f, 1000.0f);
//}
//
//void renderPoint()
//{
//	//Текущая - матрица видового преобразования
//	glMatrixMode(GL_MODELVIEW);
//	
//	glClear(GL_COLOR_BUFFER_BIT); //Очистим буфер цвета
//	glLoadIdentity(); //Загрузим едматрицу. вида
//	//Зададим позицию камеры, точку наблюд. и вектор вверх
//	gluLookAt(100.0f, 100.0f, 100.0f,
//		0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
//	glBegin(GL_POINTS);
//	glVertex3f(-50.0f, -50.0f, -50.0f);
//	glVertex3f(-50.0f, -50.0f, 50.0f);
//	glVertex3f(-50.0f, 50.0f, -50.0f);
//	glVertex3f(-50.0f, 50.0f, 50.0f);
//	glVertex3f(50.0f, -50.0f, -50.0f);
//	glVertex3f(50.0f, -50.0f, 50.0f);
//	glVertex3f(50.0f, 50.0f, -50.0f);
//	glVertex3f(50.0f, 50.0f, 50.0f);
//	glEnd();
//
//	//Сбросить все данные на обработку в конвейер преобразования
//	// OpenGL без ожидания завершения предыдущих инструкций
//	glFlush();
//	glutSwapBuffers();
//}
//int main(int argc, char** argv) {
//	glutInit(&argc, argv);
//		glutInitDisplayMode(GLUT_DEPTH | GLUT_DOUBLE | GLUT_RGBA);
//		glutInitWindowSize(600, 600);
//		glutInitWindowPosition(100, 100);
//		glutCreateWindow("Task 5");
//
//		glutReshapeFunc(Reshape);
//		glutDisplayFunc(renderPoint);
//		Init();
//		glutMainLoop();
//		return 0;
//}

//-------------------------Задание 6---------------------------

void Reshape(int width, int height)
{
	w = width; h = height;
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(65.0f, w / h, 1.0f, 1000.0f);
}

void Init(void)
{
	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
}

void renderTwoTriangles()
{
	glMatrixMode(GL_MODELVIEW);
	glClear(GL_COLOR_BUFFER_BIT);
	glLoadIdentity();
	gluLookAt(100.0f, 100.0f, 100.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);

	glBegin(GL_TRIANGLES);
	//Текущим - делаем красный цвет по( RGB)
	glColor3f(1.0f, 0.0f, 0.0f);
	//Первый красный треугольник
	glVertex3f(-75.0f, 0.0f, -50.0f);
	glVertex3f(-75.0f, 0.0f, 50.0f);
	glVertex3f(75.0f, 0.0f, 50.0f);
	//Текущим - делаем синий цвет по( RGB)
	glColor3f(0.0f, 0.0f, 1.0f);
	//Рисуем второй синий треугольник
	glVertex3f(-75.0f, 0.0f, -50.0f);
	glVertex3f(75.0f, 0.0f, -50.0f);
	glVertex3f(75.0f, 0.0f, 50.0f);
	glEnd();

	glFlush();
	glutSwapBuffers();
}

int main(int argc, char** argv) {
	glutInit(&argc, argv);
	glutInitWindowPosition(100, 100);
	glutInitWindowSize(600, 600);
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE);
	glutCreateWindow("Task 6");


	glutDisplayFunc(renderTwoTriangles);
	glutReshapeFunc(Reshape);
	Init();

	glutMainLoop();
	return 0;
}


//-------------------------Задание 7---------------------------

//double rotate_x = 0;
//double rotate_y = 0;
//double rotate_z = 0;
//
//void renderTriangle(void)
//{
//	glMatrixMode(GL_MODELVIEW);
//	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
//	glLoadIdentity();
//	gluLookAt(100.0f, 100.0f, 100.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
//
//	glRotatef(rotate_x, 1.0, 0.0, 0.0);
//	glRotatef(rotate_y, 0.0, 1.0, 0.0);
//	glRotatef(rotate_z, 0.0, 0.0, 1.0);
//
//	
//	glBegin(GL_TRIANGLES);
//	glColor3f(1.0, 0.0, 0.0); /* красный */
//	glVertex3f(0.0, 0.0, 0.0);
//	glColor3f(0.0, 1.0, 0); /* зеленый */
//	glVertex3f(75.0, 0.0, 0.0);
//	glColor3f(0.0,0.0,1.0); /* синий */
//	glVertex3f(75.0, 75.0, 0.0);
//	glEnd();
//
//	glFlush(); glutSwapBuffers();
//}
//void Reshape(int width, int height)
//{
//	w = width; h = height;
//	glViewport(0, 0, w, h);
//	glMatrixMode(GL_PROJECTION);
//	glLoadIdentity();
//	gluPerspective(65.0f, w / h, 1.0f, 1000.0f);
//}
//
//void Init(void)
//{
//	glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
//}
//
//void specialKeys(int key, int x, int y)
//{
//	switch (key)
//	{
//	case GLUT_KEY_UP: rotate_x += 15; break;
//	case GLUT_KEY_DOWN: rotate_x -= 15; break;
//	case GLUT_KEY_RIGHT: rotate_y += 15; break;
//	case GLUT_KEY_LEFT: rotate_y -= 15; break;
//	case GLUT_KEY_PAGE_UP: rotate_z += 15; break;
//	case GLUT_KEY_PAGE_DOWN: rotate_z -= 15; break;
//	}
//	glutPostRedisplay();
//}
//
//int main(int argc, char** argv) {
//
//	//Инициализировать сам glut
//	glutInit(&argc, argv);
//	//Установить начальное положение окна
//	glutInitWindowPosition(100, 100);
//	//Установить начальные размеры окна
//	glutInitWindowSize(800, 600);
//	//Установить параметры окна - двойная буфферизация
//	// и поддержка цвета RGBA
//	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE);
//	//Создать окно с заголовком OpenGL
//	glutCreateWindow("Task 7");
//
//	glutDisplayFunc(renderTriangle);
//	glutSpecialFunc(specialKeys);
//	glutReshapeFunc(Reshape);
//	Init();
//
//	//Войти в главный цикл приложения
//	glutMainLoop();
//	return 0;
//
//}