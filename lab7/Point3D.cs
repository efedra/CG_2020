using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab7
{
    public class Point3D
    {
        private double[] coords = new double[] { 0, 0, 0, 1 };
        public double X { get { return coords[0]; } set { coords[0] = value; } }
        public double Y { get { return coords[1]; } set { coords[1] = value; } }
        public double Z { get { return coords[2]; } set { coords[2] = value; } }


        public Point3D() { new Point3D(0, 0, 0); }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public void Apply(AffineTransformation t)
        {
            double[] newCoords = new double[4];
            for (int i = 0; i < 4; ++i)
            {
                newCoords[i] = 0;
                for (int j = 0; j < 4; ++j)
                    newCoords[i] += coords[j] * t.Matrix[j, i];
            }
            coords = newCoords;
        }

        static public bool operator ==(Point3D p1, Point3D p2) => !(p1 != p2);
        static public bool operator !=(Point3D p1, Point3D p2) => p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z;
        static public Point3D operator -(Point3D p1, Point3D p2) => new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        static public Point3D operator +(Point3D p1, Point3D p2) => new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        static public Point3D operator -(Point3D p)=> new Point3D( -p.X, -p.Y, - p.Z);
        static public Point3D operator *(Point3D p, double x) => new Point3D( x * p.X, x * p.Y, x * p.Z);
        static public Point3D operator *(Point3D p1, Point3D p2) => new Point3D(p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
        static public Point3D operator /(Point3D p, double x) => new Point3D(p.X / x, p.Y / x, p.Z / x);
    }
}
