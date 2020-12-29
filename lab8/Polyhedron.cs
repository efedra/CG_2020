using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab7
{
    class Polyhedron
    {
        // points of polyhedron
        List<Point3D> vertices = new List<Point3D>();
        // faces of polyhedron
        List<List<int>> faces = new List<List<int>>();
        // central point of polyhedron (weights)
        public Point3D center_point = new Point3D();

        public Polyhedron(List<Point3D> vert, List<List<int>> faces_)
        {
            vertices = vert;
            faces = faces_;
            center_point = centerofGravity();
        }

        public Polyhedron() { }

        public void Clear()
        {
            vertices.Clear();
            faces.Clear();
        }

        public void FromFile(string filename)
        {
            Clear();

            StreamReader sr = File.OpenText(filename);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                    continue;
                line = line.Replace(',', '.');
                string[] ss = line.Split();
                ss = ss.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string type = ss[0];
                switch (type)
                {
                    case "v":
                        vertices.Add(new Point3D(double.Parse(ss[1], CultureInfo.InvariantCulture), 
                            double.Parse(ss[2], CultureInfo.InvariantCulture), 
                            double.Parse(ss[3], CultureInfo.InvariantCulture)));
                        break;

                    case "f":
                        faces.Add(new List<int>());
                        for (int i = 1; i < ss.Length; i++)
                        {
                            string[] sss = ss[i].Split('/');
                            faces[faces.Count - 1].Add(int.Parse(sss[0]) - 1);
                        }
                        break;

                    default:
                        break;
                }
            }

            sr.Close();
            center_point = centerofGravity();
        }

        public void ToFile(string filename)
        {
            StreamWriter sw = new StreamWriter(filename);
            foreach (Point3D v in vertices)
                sw.WriteLine("v" + '\t' + (decimal)v.X + " " + (decimal)v.Y + " " + (decimal)v.Z);
            sw.WriteLine();

            foreach (List<int> f in faces)
            {
                sw.Write("f" + '\t');
                for (int i = 0; i < f.Count; i++)
                {
                    sw.Write(f[i] + 1);
                    if (i != f.Count - 1)
                        sw.Write(" ");
                }
                sw.Write('\n');
            }
            sw.Close();
        }

        public List<Point3D> GetPoints() => vertices;

        public List<List<int>> GetFaces() => faces;

        //size- сторона куба в котором находится тетраэдр
        public void Tetrahedron(double size)
        {
            Clear();

            vertices.Add(new Point3D());
            vertices.Add(new Point3D(size, size, 0));
            vertices.Add(new Point3D(0, size, size));
            vertices.Add(new Point3D(size, 0, size));
            center_point = centerofGravity();

            faces.Add(new List<int> { 0, 1, 2 });
            faces.Add(new List<int> { 0, 2, 3 });
            faces.Add(new List<int> { 0, 3, 1 });
            faces.Add(new List<int> { 1, 3, 2 });
        }

        public void Octahedron(double size)
        {
            Clear();

            vertices.Add(new Point3D(size / 2, size / 2, 0));
            vertices.Add(new Point3D(0, size / 2, size / 2));
            vertices.Add(new Point3D(size / 2, 0, size / 2));
            vertices.Add(new Point3D(size, size / 2, size / 2));
            vertices.Add(new Point3D(size / 2, size, size / 2));
            vertices.Add(new Point3D(size / 2, size / 2, size));

            center_point = new Point3D(size / 2, size / 2, size / 2);

            faces.Add(new List<int> { 0, 1, 2 });
            faces.Add(new List<int> { 0, 2, 3 });
            faces.Add(new List<int> { 2, 5, 3 });
            faces.Add(new List<int> { 1, 5, 2 });
            faces.Add(new List<int> { 1, 0, 4 });
            faces.Add(new List<int> { 0, 3, 4 });
            faces.Add(new List<int> { 4, 3, 5 });
            faces.Add(new List<int> { 1, 4, 5 });
        }

        public void Hexahedron(double size)
        {
            Clear();

            center_point = new Point3D(size / 2, size / 2, size / 2);
            vertices.Add(new Point3D(0, 0, 0));
            vertices.Add(new Point3D(size, 0, 0));
            vertices.Add(new Point3D(0, size, 0));
            vertices.Add(new Point3D(size, size, 0));

            vertices.Add(new Point3D(0, 0, size));
            vertices.Add(new Point3D(size, 0, size));
            vertices.Add(new Point3D(0, size, size));
            vertices.Add(new Point3D(size, size, size));

            faces.Add(new List<int> { 0, 1, 3, 2 });
            faces.Add(new List<int> { 1, 5, 7, 3 });
            faces.Add(new List<int> { 5, 4, 6, 7 });
            faces.Add(new List<int> { 4, 5, 1, 0 });
            faces.Add(new List<int> { 2, 6, 4, 0 });
            faces.Add(new List<int> { 2, 3, 7, 6 });
        }

        //size- радиус цилиндра - на рисунке он единичный
        public void Icosahedron(double size)
        {
            Clear();

            double height = size * 0.5;
            double degree = 0;
            for (int i = 0; i < 10; i++)
            {
                double phi = Math.PI * degree / 180;
                vertices.Add(new Point3D(Math.Sin(phi) * size, height, Math.Cos(phi) * size));
                degree += 36;
                height *= -1;
            }

            vertices.Add(new Point3D(0, Math.Sqrt(5) * height, 0));
            vertices.Add(new Point3D(0, -Math.Sqrt(5) * height, 0));
            center_point = new Point3D(0, 0, 0);
        }

        public void Dodecahedron(double size)
        {
            Clear();

            List<Point3D> points_icosa = new List<Point3D>();
            double height = size * 0.5;
            double degree = 0;
            for (int i = 0; i < 10; i++)
            {
                double phi = Math.PI * degree / 180;
                points_icosa.Add(new Point3D(Math.Sin(phi) * size, height, Math.Cos(phi) * size));
                degree += 36;
                height *= -1;
            }

            points_icosa.Add(new Point3D(0, Math.Sqrt(5) * height, 0));
            points_icosa.Add(new Point3D(0, -Math.Sqrt(5) * height, 0));
            center_point = new Point3D(0, 0, 0);

            for (int i = 0; i < 8; i++)//боковвые
                vertices.Add(centerofGravity(points_icosa[i], points_icosa[i + 1], points_icosa[i + 2]));
            vertices.Add(centerofGravity(points_icosa[8], points_icosa[9], points_icosa[0]));
            vertices.Add(centerofGravity(points_icosa[9], points_icosa[0], points_icosa[1]));

            for (int i = 0; i < 7; i += 2)//верхние
                vertices.Add(centerofGravity(points_icosa[10], points_icosa[i], points_icosa[i + 2]));
            vertices.Add(centerofGravity(points_icosa[10], points_icosa[8], points_icosa[0]));

            for (int i = 1; i < 8; i += 2)//нижние
                vertices.Add(centerofGravity(points_icosa[11], points_icosa[i], points_icosa[i + 2]));
            vertices.Add(centerofGravity(points_icosa[11], points_icosa[9], points_icosa[1]));
        }

        private Point3D centerofGravity(Point3D p1, Point3D p2, Point3D p3) => (p1 + p2 + p3) / 3;

        public Point3D centerofGravity()
        {
            Point3D p = new Point3D();
            foreach (Point3D p1 in vertices)
                p += p1;
            return p / vertices.Count();
        }

        //--------------------------------------------------------------------------------------------------
        //---------------------------------------- lab 8 --- task 1 ----------------------------------------
        //--------------------------------------------------------------------------------------------------
        public void Move(double dx, double dy, double dz)
        {
            Matrix matrix = Matrix.Move(dx, dy, dz);
            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void Rotate(double angleX, double angleY, double angleZ)
        {
            Matrix matrix = Matrix.Rotate(angleX, angleY, angleZ);
            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void Scale(double fx, double fy, double fz)
        {
            Matrix matrix = Matrix.Scale(fx, fy, fz);
            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void ReflectX() => Scale(1, -1, -1);

        public void ReflectY() => Scale(-1, 1, -1);

        public void ReflectZ() => Scale(-1, -1, 1);

        public void RotateLineAngle(Point3D vec, double angle)
        {
            Matrix matrix = Matrix.RotateLineAngle(vec, angle);
            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }
        public void Parallel_rotate(Point3D vec, double angle, Point3D center)
        {
            Matrix mv = Matrix.Move(-center.X, -center.Y, -center.Z);
            Matrix rt = Matrix.RotateLineAngle(vec, angle);
            Matrix mv_back = Matrix.Move( center.X,  center.Y,  center.Z);
            Matrix matrix = mv * rt * mv_back;
            
            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void Scale_center(double fx, double fy, double fz, Point3D center)
        {
            Matrix mv = Matrix.Move(-center.X, -center.Y, -center.Z);
            Matrix scl = Matrix.Scale(fx, fy, fz);
            Matrix mv_back = Matrix.Move(center.X, center.Y, center.Z);
            Matrix matrix = mv * scl * mv_back;

            foreach (Point3D point in vertices)
                point.Apply(matrix);
            center_point.Apply(matrix);
        }
        //--------------------------------------------------------------------------------------------------
        //---------------------------------------- lab 8 --- task 1 ----------------------------------------
        //--------------------------------------------------------------------------------------------------
        public Point3D normal_of_face(List<int> face)
        {
            var p1 = vertices[face[0]];
            var p2 = vertices[face[1]];
            var p3 = vertices[face[2]];

            var vec1 = p2 - p1;
            var vec2 = p1 - p3;

            return new Point3D(vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                        vec1.Z * vec2.X - vec1.X * vec2.Z,
                        vec1.X * vec2.Y - vec1.Y * vec2.X);
        }

        public static double Distance(Point3D vec) => Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);

        public static double Distance(Point3D p1, Point3D p2) => Distance(p2 - p1);

        public static bool angleIsObtuse(Point3D vec1, Point3D vec2)
        {
            var mult_vec = vec1 * vec2;
            var scalarProd = mult_vec.X + mult_vec.Y + mult_vec.Z;

            return scalarProd / (Distance(vec1) * Distance(vec2)) > 0;
        }

        public List<List<int>> front_faces(Point3D direction_of_sight)
        {
            var front_faces = new List<List<int>>();

            foreach (var face in GetFaces())
                if (angleIsObtuse(normal_of_face(face), direction_of_sight))
                    front_faces.Add(face);

            return front_faces;
        }
        
        //--------------------------------------------------------------------------------------------------
        //---------------------------------------- lab 8 --- task 1 ----------------------------------------
        //--------------------------------------------------------------------------------------------------
    }
}