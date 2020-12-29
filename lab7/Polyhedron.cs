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
        //// edges of polyhedron as the indexes of is points
        //List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
        // polygons of polyhedron as the indexes of is points
        List<List<int>> faces = new List<List<int>>();
        // central point of polyhedron (weights)
        public Point3D center_point = new Point3D();

        public Polyhedron(List<Point3D> vert, List<List<int>> faces_)
        {
            vertices = vert;
            //edges = edges_;
            faces = faces_;
            center_point = centerofGravity();
        }

        public Polyhedron()
        {
        }

        public void Clear()
        {
            vertices.Clear();
            //edges.Clear();
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

                    //case "e":
                    //    edges.Add(new Tuple<int, int>(int.Parse(ss[1]), int.Parse(ss[2])));
                    //    break;

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

            //foreach (Tuple<int, int> e in edges)
            //    sw.WriteLine("e" + '\t' + e.Item1 + " " + e.Item2);
            //sw.WriteLine();

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

        public List<Point3D> GetPoints()
        {
            return vertices;
        }

        //public List<Tuple<int, int>> GetEdges()
        //{
        //    return edges;
        //}

        public List<List<int>> GetFaces()
        {
            return faces;
        }

        //size- сторона куба в котором находится тетраэдр
        public void Tetrahedron(double size)
        {
            Clear();

            vertices.Add(new Point3D());
            vertices.Add(new Point3D(size, size, 0));
            vertices.Add(new Point3D(0, size, size));
            vertices.Add(new Point3D(size, 0, size));
            center_point = centerofGravity();

            //edges.Add(new Tuple<int, int>(0, 1));
            //edges.Add(new Tuple<int, int>(0, 2));
            //edges.Add(new Tuple<int, int>(0, 3));

            //edges.Add(new Tuple<int, int>(1, 2));
            //edges.Add(new Tuple<int, int>(1, 3));

            //edges.Add(new Tuple<int, int>(2, 3));

            faces.Add(new List<int> { 0, 1, 2 });
            faces.Add(new List<int> { 0, 2, 3 });
            faces.Add(new List<int> { 0, 3, 1 });
            faces.Add(new List<int> { 1, 2, 3 });
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
            //edges.Add(new Tuple<int, int>(0, 1));
            //edges.Add(new Tuple<int, int>(0, 2));
            //edges.Add(new Tuple<int, int>(0, 3));
            //edges.Add(new Tuple<int, int>(0, 4));

            //edges.Add(new Tuple<int, int>(5, 1));
            //edges.Add(new Tuple<int, int>(5, 2));
            //edges.Add(new Tuple<int, int>(5, 3));
            //edges.Add(new Tuple<int, int>(5, 4));

            //edges.Add(new Tuple<int, int>(1, 2));
            //edges.Add(new Tuple<int, int>(2, 3));
            //edges.Add(new Tuple<int, int>(3, 4));
            //edges.Add(new Tuple<int, int>(4, 1));

            faces.Add(new List<int> { 0, 1, 2 });
            faces.Add(new List<int> { 0, 2, 3 });
            faces.Add(new List<int> { 2, 3, 5 });
            faces.Add(new List<int> { 1, 2, 5 });
            faces.Add(new List<int> { 0, 1, 4 });
            faces.Add(new List<int> { 0, 3, 4 });
            faces.Add(new List<int> { 3, 4, 5 });
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

            //edges.Add(new Tuple<int, int>(0, 1));
            //edges.Add(new Tuple<int, int>(0, 2));
            //edges.Add(new Tuple<int, int>(3, 1));
            //edges.Add(new Tuple<int, int>(3, 2));

            //edges.Add(new Tuple<int, int>(4, 5));
            //edges.Add(new Tuple<int, int>(4, 6));
            //edges.Add(new Tuple<int, int>(7, 5));
            //edges.Add(new Tuple<int, int>(7, 6));

            //edges.Add(new Tuple<int, int>(0, 4));
            //edges.Add(new Tuple<int, int>(1, 5));
            //edges.Add(new Tuple<int, int>(2, 6));
            //edges.Add(new Tuple<int, int>(3, 7));

            faces.Add(new List<int> { 0, 1, 3, 2 });
            faces.Add(new List<int> { 1, 3, 7, 5 });
            faces.Add(new List<int> { 5, 7, 6, 4 });
            faces.Add(new List<int> { 4, 6, 2, 0 });
            faces.Add(new List<int> { 1, 5, 4, 0 });
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
            //30 ребер
            center_point = new Point3D(0, 0, 0);
            ////по бокам
            //for (int i = 0; i < 9; i++)
            //    edges.Add(new Tuple<int, int>(i, i + 1));
            //edges.Add(new Tuple<int, int>(9, 0));

            ////по окружностям 
            //for (int i = 0; i < 8; i++)
            //    edges.Add(new Tuple<int, int>(i, i + 2));
            //edges.Add(new Tuple<int, int>(8, 0));
            //edges.Add(new Tuple<int, int>(9, 1));

            ////верхние грани
            //for (int i = 0; i < 9; i += 2)
            //    edges.Add(new Tuple<int, int>(10, i));

            ////нижние грани
            //for (int i = 1; i < 10; i += 2)
            //    edges.Add(new Tuple<int, int>(11, i));
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

            //for (int i = 0; i < 9; i++)
            //    edges.Add(new Tuple<int, int>(i, i + 1));
            //edges.Add(new Tuple<int, int>(9, 0));

            //for (int i = 0; i < 5; i++)
            //    edges.Add(new Tuple<int, int>(i * 2, 10 + i));
            //for (int i = 0; i < 5; i++)
            //    edges.Add(new Tuple<int, int>((i * 2 + 1), 15 + i));

            //for (int i = 10; i < 14; i++)//верхние
            //    edges.Add(new Tuple<int, int>(i, i + 1));
            //edges.Add(new Tuple<int, int>(10, 14));

            //for (int i = 15; i < 19; i++)//нижние
            //    edges.Add(new Tuple<int, int>(i, i + 1));
            //edges.Add(new Tuple<int, int>(15, 19));
        }

        private Point3D centerofGravity(Point3D p1, Point3D p2, Point3D p3) => (p1 + p2 + p3) / 3;

        public Point3D centerofGravity()
        {
            Point3D p = new Point3D();
            foreach (Point3D p1 in vertices)
            {
                p = p + p1;
            }
            return p / vertices.Count();

        }
        public void Apply(AffineTransformation t)
        {
            foreach (var point in vertices)
                point.Apply(t);
            center_point.Apply(t);
        }

    }
}