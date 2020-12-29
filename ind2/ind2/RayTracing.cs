using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ind2
{
    enum ShapeType { Ball, Face, Wall, };
    enum Material { Matte, Mirror, Transparent };

    class Shape { }

    class Sphere : Shape
    {
        public Point3D Center;
        public double Radius;
        public Sphere(Point3D c, double r)
        {
            Center = c;
            Radius = r;
        }
    }

    class Face : Shape
    {
        public Point3D MinPoint;
        public Point3D MaxPoint;
        public Point3D Normal;
        public Face(Point3D p1, Point3D p2, Point3D norm)
        {
            MinPoint = p1;
            MaxPoint = p2;
            Normal = norm;
        }
    }

    class SceneShape
    {
        public ShapeType Type;
        public Shape Shape;
        public Color Ambient;
        public Material Material;
        public SceneShape(ShapeType type, Shape shape, Color color, Material material)
        {
            Type = type;
            Shape = shape;
            Ambient = color;
            Material = material;
        }
    }

    class LightSource
    {
        public Point3D Position;
        public double Intens;
        public LightSource(Point3D pos, double intens)
        {
            Position = pos;
            Intens = intens;
        }
    }

    class RayTracing
    {
        /// <summary>
        /// Источники света
        /// </summary>
        static List<LightSource> lights;

        /// <summary>
        /// Сцена
        /// </summary>
        static List<SceneShape> scene;

        // <summary>
        /// Скалярное произведение
        /// </summary>
        private static double Scalar(Point3D v1, Point3D v2) => (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);

        /// <summary>
        /// Нормирование
        /// </summary>
        private static Point3D Normalize(Point3D v)
        {
            double len = Math.Sqrt((v.x * v.x) + (v.y * v.y) + (v.z * v.z));
            return new Point3D(v.x / len, v.y / len, v.z / len);
        }
        /// <summary>
        /// Получения цвета каждого пикселя bitmap
        /// </summary>
        public static Bitmap GetImage(int width, int heigh, List<SceneShape> _scene, List<LightSource> _lights, Form1 form)
        {
            lights = _lights;
            scene = _scene;
            Point3D eye = new Point3D(0, -5, -35);
            Bitmap newImg = new Bitmap(width, heigh);
            Color[,] colors = new Color[width, heigh];
            for (int i = 0; i < width; i++)
            {
                form.progressBar1.Value = (i + 1) * 100 / width;
                for (int j = 0; j < heigh; j++)
                {
                    Point3D point = Convert2DTo3D(i, j, width, heigh);
                    Color color = RayTrace(eye, point, 10);
                    colors[i, j] = color;
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < heigh; j++)
                    newImg.SetPixel(i, j, colors[i, j]);
            }
            return newImg;
        }

        /// <summary>
        /// Трассировка луча
        /// </summary>
        private static Color RayTrace(Point3D eye, Point3D ray, int depth)
        {
            // Ищем ближайший объект, пересекающийся с лучом
            (SceneShape nearest, double t) = NearestElem(eye, ray);

            // Если луч ушел в никуда, то черный цвет
            if (nearest == null)
                return Color.Black;

            // Точка пересечения с ближайшим объектом
            Point3D pointIntersec = eye + t * ray;

            Point3D normal;
            if (nearest.Type == ShapeType.Ball)
            {
                Sphere sphere = nearest.Shape as Sphere;
                normal = Normalize(pointIntersec - sphere.Center);
            }
            else
            {
                Face face = nearest.Shape as Face;
                normal = face.Normal;
            }
            // Фоновый цвет
            Color amb = nearest.Ambient;

            // Интенсивность
            double intens = CalcIntensity(pointIntersec, normal);

            // Рассчитаный цвет
            Color color =
                Color.FromArgb(Math.Min((int)(amb.R * intens), 255), Math.Min((int)(amb.G * intens), 255), Math.Min((int)(amb.B * intens), 255));
            // Если дошли до макс. глубины рекурсии или объект матовый, то возвращаем рассчитанное значение
            if (depth == 0 || nearest.Material == Material.Matte)
            {
                return color;
            }

            // Отраженный/преломленный луч
            Point3D reRay = new Point3D();
            // Если зеркало, то рассчитываем отраженный луч
            if (nearest.Material == Material.Mirror)
            {
                reRay = Normalize(Reflect(ray, normal));
            }
            // Если прозрачный, то рассчитываем преломленный луч
            else if (nearest.Material == Material.Transparent)
            {
                reRay = Normalize(Refract(ray, normal));
            }
            // Запускаем рекурсивную трассировку луча
            Color reColor = RayTrace(pointIntersec, reRay, depth - 1);

            //цвет зеркал и  цвет прозрачности
            double k1 = 0.20;//цвет собсвтенный
            double k2 = 1 - k1; // цвет отраженного 
            return Color.FromArgb(
                (int)(color.R * k1 + reColor.R * k2),
                (int)(color.G * k1 + reColor.G * k2),
                (int)(color.B * k1 + reColor.B * k2));
        }
        /// <summary>
        /// Находит ближайший объект к лучу и расстояние до него
        /// </summary>
        private static (SceneShape, double) NearestElem(Point3D eye, Point3D ray)
        {
            (SceneShape nearest, double min) = (null, double.PositiveInfinity);

            foreach (SceneShape sceneElement in scene)
            {
                double t = Intersection(eye, ray, sceneElement);
                // Обновить минимум, если пересечение существует и оно меньше текущего значения
                if (!double.IsPositiveInfinity(t) && (t < min))
                    (nearest, min) = (sceneElement, t);
            }

            return (nearest, min);
        }

        /// <summary>
        /// Расчет интенсивности
        /// </summary>
        private static double CalcIntensity(Point3D point, Point3D normal)
        {
            double intensity = 0;
            foreach (var light in lights)
            {
                Point3D l = light.Position - point;

                (SceneShape shape, _) = NearestElem(point, l);

                if (shape.Type != ShapeType.Wall || (shape == null))
                    continue;

                double scalar = Scalar(normal, l);// направление света 
                if (scalar > 0)
                    intensity += (light.Intens * scalar) / (Length(normal) * Length(l));
            }
            return intensity;
        }
        private static double eps = 1e-10;
        /// <summary>
        /// Находит пересечение луча с объектом сцены (грань или сфера)
        /// </summary>
        private static double Intersection(Point3D eye, Point3D ray, SceneShape element)
        {
            if (element.Type == ShapeType.Ball)
            {
                Sphere sphere = element.Shape as Sphere;
                Point3D EC = eye - sphere.Center; //луч с началом в Center и направленный на глаз Eye

                double a = Scalar(ray, ray); //d^2
                double b = Scalar(EC, ray); //(d,s)
                double c = Scalar(EC, EC) - sphere.Radius * sphere.Radius; //s^2 -r^2 

                double D = b * b - a * c;
                // Нет корней - нет пересечения
                if (D < eps)
                    return double.PositiveInfinity;

                double t1 = (-b + Math.Sqrt(D)) / a;
                double t2 = (-b - Math.Sqrt(D)) / a;

                if (Math.Max(t1, t2) < eps)
                    return double.PositiveInfinity;
                //Наименьшее положительное значение t, если оно существует, дает ответ задачи
                return t2 > eps ? t2 : t1;
            }
            else if (element.Type == ShapeType.Face || element.Type == ShapeType.Wall)
            {
                Face plain = element.Shape as Face;

                Point3D normal = Normalize(plain.Normal);
                Point3D v = eye - plain.MaxPoint;

                double scalar1 = Scalar(v, normal);//(a,n)
                double scalar2 = Scalar(ray, normal); //(d,n)
                double t = -scalar1 / scalar2;

                if (t < eps)
                    return double.PositiveInfinity;

                Point3D interPoint = eye + t * ray;//это находится точка пересечение через параметрическое уравнение. t — это параметр уравнения. Параметрическим уравнением задается луч
                return PointInPlane(plain, interPoint) ? t : double.PositiveInfinity;
            }
            return double.PositiveInfinity;
        }

        /// <summary>
        /// Расчет отраженного луча
        /// </summary>
        private static Point3D Reflect(Point3D ray, Point3D normal) => ray - 2 * Scalar(ray, normal) * normal;

        /// <summary>
        /// Расчет преломленного луча
        /// </summary>
        private static Point3D Refract(Point3D ray, Point3D normal)
        {
            const double n1 = 1.1;
            const double n2 = 1;
            Point3D sn = Normalize(Scalar(ray, normal) < 0 ? normal : new Point3D(-normal.x, -normal.y, -normal.z));
            Point3D rd = Normalize(ray);

            double inC1 = -Scalar(sn, rd);
            double inN = inC1 > 0 ? n1 / n2 : n2 / n1;
            double inC2 = Math.Sqrt(Math.Max(1 - inN * inN * (1 - inC1 * inC1), 0));

            return new Point3D(ray.x * inN + normal.x * (inN * inC1 - inC2),
            ray.y * inN + normal.y * (inN * inC1 - inC2),
            ray.z * inN + normal.z * (inN * inC1 - inC2));
        }
        /// <summary>
        /// Длина вектора
        /// </summary>
        private static double Length(Point3D point) => Math.Sqrt(point.x * point.x + point.y * point.y + point.z * point.z);

        /// <summary>
        /// Рассчет 3D-координат по координатам bitmap (x, y)
        /// </summary>
        private static Point3D Convert2DTo3D(int x, int y, int width, int heigh)//координаты масштабируется с учётом размеров пикчербокса
        {
            double x3D = (x - width / 2) * (10.0 / width);
            double y3D = -(y - heigh / 2) * (10.0 / heigh);
            return new Point3D(y3D, x3D, 10);
        }
        /// <summary>
        /// Принадлежность точки грани
        /// </summary>
        private static bool PointInPlane(Face plain, Point3D point) =>
            (point.x < Math.Max(plain.MaxPoint.x, plain.MinPoint.x) + eps) && (point.x > Math.Min(plain.MaxPoint.x, plain.MinPoint.x) - eps) &&
            (point.y < Math.Max(plain.MaxPoint.y, plain.MinPoint.y) + eps) && (point.y > Math.Min(plain.MaxPoint.y, plain.MinPoint.y) - eps) &&
            (point.z < Math.Max(plain.MaxPoint.z, plain.MinPoint.z) + eps) && (point.z > Math.Min(plain.MaxPoint.z, plain.MinPoint.z) - eps);


    }
}
