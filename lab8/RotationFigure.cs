using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CG_lab7
{
    class RotationFigure
    {
        private static Point3D findCenterRotationFigure(List<Point3D> points, int axis)
        {
            double sum = 0;
            foreach (var p in points)
            {
                switch (axis)
                {
                    case 1: sum += p.X; break;
                    case 2: sum += p.Y; break;
                    case 3: sum += p.Z; break;
                }
            }
            switch (axis)
            {
                case 1: return new Point3D(sum / points.Count, 0, 0);
                case 2: return new Point3D(0, sum / points.Count, 0);
                case 3: return new Point3D(0, 0, sum / points.Count);
            }
            return new Point3D();
        }
        public static List<Point3D> Copy(List<Point3D> points)
        {
            var l = new List<Point3D>();
            foreach (var p in points)
                l.Add(new Point3D(p.X, p.Y, p.Z));
            return l;
        }

        private static List<Point3D> to3DPoints(List<Point> points, int axis, int width,int height, out bool up, out bool down)
        {
            List<Point3D> points3D = new List<Point3D>();
            up = (points[0].X - width / 3) != 0;
            down = (points[points.Count()-1].X - width / 3) != 0;
            switch (axis)
            {
                case 1:
                    foreach (var p in points)
                        points3D.Add(new Point3D((height - p.Y) / 2, 0, (p.X - width / 3) / 2));
                    break;
                case 2:
                    foreach (var p in points)
                        points3D.Add(new Point3D(0, (height - p.Y) / 2, (p.X - width / 3) / 2));
                    break;
                case 3:
                    foreach (var p in points)
                        points3D.Add(new Point3D((p.X - width / 3) / 2, 0, (height - p.Y) / 2));
                    break;
            }
            return points3D;
        }


        public static Polyhedron DrawRotationFigure(List<Point> points_rotation, int axis,int count_split, int width, int height)
        {
            bool up, down;
            var points3D = to3DPoints(points_rotation, axis,width,height, out up, out down);
            Point3D vec = new Point3D();
            Point3D center = findCenterRotationFigure(points3D, axis);
            switch (axis)
            {
                case 1:
                    vec = new Point3D(1, 0, 0);
                    break;
                case 2:
                    vec = new Point3D(0, 1, 0);
                    break;
                case 3:
                    vec = new Point3D(0, 0, 1);
                    break;
            }
            int angle = 360 / count_split;
            int sum = angle;

            List<Point3D> temp1 = Copy(points3D);
            List<Point3D> all_points = Copy(points3D);
            int last_ind = all_points.Count() - 1;
            List<List<int>> polygons = new List<List<int>>();
            for (int k = 1; k < count_split; k++)
            {
                for (int i = 0; i < temp1.Count(); i++)
                {
                    temp1[i].Apply(Matrix.Move(-center.X, -center.Y, -center.Z));
                    temp1[i].Apply(Matrix.RotateLineAngle(vec, -sum));
                    temp1[i].Apply(Matrix.Move(center.X, center.Y, center.Z));
                    all_points.Add(temp1[i]);
                }
                last_ind = all_points.Count() - 1;

                for (int i = 0; i < temp1.Count() - 1; i++)
                    polygons.Add(new List<int> { last_ind - i, last_ind - i - 1, last_ind - i - 1 - temp1.Count(), last_ind - i - temp1.Count() });
                temp1 = Copy(points3D);
                sum += angle;
            }

            last_ind = all_points.Count() - 1;
            for (int i = 0; i < temp1.Count() - 1; i++)
                polygons.Add(new List<int> { temp1.Count() - 1 - i,temp1.Count() - 2 - i, last_ind - i - 1,last_ind - i  });

            if (up)
            {
                var l = new List<int>();
                for (int i = 0; i < count_split; i++)
                    l.Add(i * temp1.Count());
                polygons.Add(l);
            }
            if (down)
            {
                var l = new List<int>();
                for (int i = count_split - 1; i >= 0; i--)
                    l.Add(i * temp1.Count() + temp1.Count() - 1);
                polygons.Add(l);
            }
            temp1.Clear();
            return new Polyhedron(all_points, polygons);
        }
        
    }
}
