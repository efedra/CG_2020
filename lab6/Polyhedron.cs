using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_3D
{
    class Polyhedron
    {

        //current points of polyhedron
        List<Point3D> points = new List<Point3D>();
        List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
        public Point3D center_point = new Point3D();

        public List<Point3D> GetPoints()
        {
            return points;
        }

        public List<Tuple<int, int>> GetEdges()
        {
            return edges;
        }

        //size- сторона куба в котором находится тетраэдр
        public void Tetrahedron(double size)
        {
            points.Clear();
            edges.Clear();
            points.Add(new Point3D());
            points.Add(new Point3D(size, size, 0));
            points.Add(new Point3D(0, size, size));
            points.Add(new Point3D(size, 0, size));
            center_point = centerofGravity(points);

            edges.Add(new Tuple<int, int>(0, 1));
            edges.Add(new Tuple<int, int>(0, 2));
            edges.Add(new Tuple<int, int>(0, 3));

            edges.Add(new Tuple<int, int>(1, 2));
            edges.Add(new Tuple<int, int>(1, 3));

            edges.Add(new Tuple<int, int>(2, 3));
        }

        public void Octahedron(double size)
        {
            points.Clear();
            edges.Clear();

            points.Add(new Point3D(size / 2, size / 2, 0));
            points.Add(new Point3D(0, size / 2, size / 2));
            points.Add(new Point3D(size / 2, 0, size / 2));
            points.Add(new Point3D(size, size / 2, size / 2));
            points.Add(new Point3D(size / 2, size, size / 2));
            points.Add(new Point3D(size / 2, size / 2, size));

            center_point = new Point3D(size / 2, size / 2, size / 2);
            edges.Add(new Tuple<int, int>(0, 1));
            edges.Add(new Tuple<int, int>(0, 2));
            edges.Add(new Tuple<int, int>(0, 3));
            edges.Add(new Tuple<int, int>(0, 4));

            edges.Add(new Tuple<int, int>(5, 1));
            edges.Add(new Tuple<int, int>(5, 2));
            edges.Add(new Tuple<int, int>(5, 3));
            edges.Add(new Tuple<int, int>(5, 4));

            edges.Add(new Tuple<int, int>(1, 2));
            edges.Add(new Tuple<int, int>(2, 3));
            edges.Add(new Tuple<int, int>(3, 4));
            edges.Add(new Tuple<int, int>(4, 1));

        }

        public void Hexahedron(double size)
        {
            points.Clear();
            edges.Clear();
            center_point = new Point3D(size / 2, size / 2, size / 2);
            points.Add(new Point3D(0, 0, 0));
            points.Add(new Point3D(size, 0, 0));
            points.Add(new Point3D(0, size, 0));
            points.Add(new Point3D(size, size, 0));

            points.Add(new Point3D(0, 0, size));
            points.Add(new Point3D(size, 0, size));
            points.Add(new Point3D(0, size, size));
            points.Add(new Point3D(size, size, size));

            edges.Add(new Tuple<int, int>(0, 1));
            edges.Add(new Tuple<int, int>(0, 2));
            edges.Add(new Tuple<int, int>(3, 1));
            edges.Add(new Tuple<int, int>(3, 2));

            edges.Add(new Tuple<int, int>(4, 5));
            edges.Add(new Tuple<int, int>(4, 6));
            edges.Add(new Tuple<int, int>(7, 5));
            edges.Add(new Tuple<int, int>(7, 6));

            edges.Add(new Tuple<int, int>(0, 4));
            edges.Add(new Tuple<int, int>(1, 5));
            edges.Add(new Tuple<int, int>(2, 6));
            edges.Add(new Tuple<int, int>(3, 7));
        }

        //size- радиус цилиндра - на рисунке он единичный
        public void Icosahedron(double size)
        {
            points.Clear();
            edges.Clear();

            double height = size * 0.5;
            double degree = 0;
            for (int i = 0; i < 10; i++)
            {
                double phi = Math.PI * degree / 180;
                points.Add(new Point3D(Math.Sin(phi) * size, height, Math.Cos(phi) * size));
                degree += 36;
                height *= -1;
            }

            points.Add(new Point3D(0, Math.Sqrt(5) * height, 0));
            points.Add(new Point3D(0, -Math.Sqrt(5) * height, 0));
            //30 ребер
            center_point = new Point3D(0, 0, 0);
            //по бокам
            for (int i = 0; i < 9; i++)
                edges.Add(new Tuple<int, int>(i, i + 1));
            edges.Add(new Tuple<int, int>(9, 0));

            //по окружностям 
            for (int i = 0; i < 8; i++)
                edges.Add(new Tuple<int, int>(i, i + 2));
            edges.Add(new Tuple<int, int>(8, 0));
            edges.Add(new Tuple<int, int>(9, 1));

            //верхние грани
            for (int i = 0; i < 9; i += 2)
                edges.Add(new Tuple<int, int>(10, i));

            //нижние грани
            for (int i = 1; i < 10; i += 2)
                edges.Add(new Tuple<int, int>(11, i));
        }

        public void Dodecahedron(double size)
        {
            points.Clear();
            edges.Clear();
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
                points.Add(centerofGravity(points_icosa[i], points_icosa[i + 1], points_icosa[i + 2]));
            points.Add(centerofGravity(points_icosa[8], points_icosa[9], points_icosa[0]));
            points.Add(centerofGravity(points_icosa[9], points_icosa[0], points_icosa[1]));

            for (int i = 0; i < 7; i += 2)//верхние
                points.Add(centerofGravity(points_icosa[10], points_icosa[i], points_icosa[i + 2]));
            points.Add(centerofGravity(points_icosa[10], points_icosa[8], points_icosa[0]));

            for (int i = 1; i < 8; i += 2)//нижние
                points.Add(centerofGravity(points_icosa[11], points_icosa[i], points_icosa[i + 2]));
            points.Add(centerofGravity(points_icosa[11], points_icosa[9], points_icosa[1]));

            for (int i = 0; i < 9; i++)
                edges.Add(new Tuple<int, int>(i, i + 1));
            edges.Add(new Tuple<int, int>(9, 0));

            for (int i = 0; i < 5; i++)
                edges.Add(new Tuple<int, int>(i * 2, 10 + i));
            for (int i = 0; i < 5; i++)
                edges.Add(new Tuple<int, int>((i * 2 + 1), 15 + i));

            for (int i = 10; i < 14; i++)//верхние
                edges.Add(new Tuple<int, int>(i, i + 1));
            edges.Add(new Tuple<int, int>(10, 14));

            for (int i = 15; i < 19; i++)//нижние
                edges.Add(new Tuple<int, int>(i, i + 1));
            edges.Add(new Tuple<int, int>(15, 19));
        }

        private Point3D centerofGravity(Point3D p1, Point3D p2, Point3D p3) => (p1 + p2 + p3) / 3;

        public Point3D centerofGravity(List<Point3D> l)
        {
            Point3D p = new Point3D();
            foreach (Point3D p1 in l)
            {
                p = p + p1;
            }
            return p / l.Count();

        }
        public void Apply(AffineTransformation t)
        {
            foreach (var point in points)
                point.Apply(t);
            center_point.Apply(t);
        }

    }
}