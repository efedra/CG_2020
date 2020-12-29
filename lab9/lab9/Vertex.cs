using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class Vertex
    {
        public double X;
        public double Y;
        public double Z;
        public double W;


        public Vertex() { new Vertex(0, 0, 0, 1); }

        public Vertex(double x, double y, double z, double w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vertex(Vertex copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Z = copy.Z;
            this.W = copy.W;
        }

        public void Apply(Matrix t)
        {
            double[] coords = { X, Y, Z, W };
            double[] newCoords = new double[4];
            for (int i = 0; i < 4; ++i)
            {
                newCoords[i] = 0;
                for (int j = 0; j < 4; ++j)
                    newCoords[i] += coords[j] * t.data[j, i];
            }
            X = newCoords[0];
            Y = newCoords[1];
            Z = newCoords[2];
            W = newCoords[3];
        }

        public void perspective(double dist, double x_ang, double y_ang)
        {
            if (Z == dist)
                return;
            this.Apply(Matrix.PERSP(dist, x_ang, y_ang));
            X /= W;
            Y /= W;
            Z /= W;
            W /= W;
        }

        public double length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        static public bool operator ==(Vertex p1, Vertex p2) => !(p1 != p2);
        static public bool operator !=(Vertex p1, Vertex p2) => p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z;
        static public Vertex operator -(Vertex p1, Vertex p2) => new Vertex(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        static public Vertex operator +(Vertex p1, Vertex p2) => new Vertex(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        static public Vertex operator -(Vertex p) => new Vertex(-p.X, -p.Y, -p.Z);
        static public Vertex operator *(Vertex p, double x) => new Vertex(x * p.X, x * p.Y, x * p.Z);
        static public Vertex operator *(Vertex p1, Vertex p2) => new Vertex(p1.X * p2.X, p1.Y * p2.Y, p1.Z * p2.Z);
        static public Vertex operator /(Vertex p, double x) => new Vertex(p.X / x, p.Y / x, p.Z / x);
    }
}
