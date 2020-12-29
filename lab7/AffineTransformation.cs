using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab7
{
    public class AffineTransformation
    {
        private double[,] matrix = new double[4, 4];

        public double[,] Matrix { get { return matrix; } }

        public AffineTransformation()
        {
            matrix = 
                new double[,] { //identity
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
           
        }

        public AffineTransformation(double[,] matrix)
        {
            this.matrix = matrix;
        }

        // shift (x,y) to dx and dy coordinate lines
        public static AffineTransformation Translate(double dx, double dy, double dz)
        {
            return new AffineTransformation(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                });
        }


        // rotate (x,y) in point p and angle phi
        public static AffineTransformation RotateX(double angle)
        {
            double phi = angle * Math.PI / 180;
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            return new AffineTransformation(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static AffineTransformation RotateY(double angle)
        {
            double phi = angle * Math.PI / 180;
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            return new AffineTransformation(
                new double[,]
                {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static AffineTransformation RotateZ(double angle)
        {
            double phi = angle * Math.PI / 180;
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            return new AffineTransformation(
                new double[,]
                {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }


        // scaling (x,y) with coef alpha, betta and in Point p
        public Point3D scale_point(double x, double y, double alpha, double beta, double gamma, Point3D p)
        {
            double[,] scaling_matr = {
                { alpha, 0, 0, 0 },
                { 0, beta, 0, 0 },
                { 0, 0, gamma, 0 },
                { 0, 0, 0, 1 } };
               // { (1-alpha) * p.X, (1-beta) *  p.Y, 1 } };

            double[,] point = { { p.X, p.Y, p.Z, 1 } };
            var c = mult_matr(point, scaling_matr);
            return new Point3D(c[0, 0], c[0, 1], c[0, 2]);
        }

        public static double[,] mult_matr(double[,] a, double[,] b)
        {
            (int a_row, int a_collumn) = (a.GetLength(0), a.GetLength(1));
            (int b_row, int b_collumn) = (b.GetLength(0), b.GetLength(1));

            if (a_collumn != b_row)
            {
                throw new Exception("Count column in first matrix and count of rows in second are not equal");
            }
            var c = new double[a_row, b_collumn];

            for (var i = 0; i < a_row; i++)
                for (var j = 0; j < b_collumn; j++)
                {
                    c[i, j] = 0;
                    for (var k = 0; k < a_collumn; k++)
                        c[i, j] += a[i, k] * b[k, j];
                }
            return c;
        }


        public static AffineTransformation Scale(double fx, double fy, double fz)
        {
            return new AffineTransformation(
                new double[,] {
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }
        public static AffineTransformation ReflectX()
        {
            return Scale(1, -1, -1);
        }

        public static AffineTransformation ReflectY()
        {
            return Scale(-1, 1, -1);
        }

        public static AffineTransformation ReflectZ()
        {
            return Scale(-1, -1, 1);
        }

        public static AffineTransformation RotateLineAngle(Point3D vec,double angle)
        {
            double l = vec.X;
            double m = vec.Y;
            double n = vec.Z;
            double phi = angle * Math.PI / 180;
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            return new AffineTransformation(
                new double[,] {
                    { l*l + cos*(1-l*l), l*(1-cos)*m + n*sin,l*(1-cos)*n - m*sin , 0 },
                    { l*(1-cos)*m - n*sin, m*m + cos*(1-m*m), m*(1-cos)*n + l*sin, 0 },
                    { l*(1-cos)*n + m*sin, m*(1-cos)*n -l*sin, n*n+ cos*(1-n*n), 0 },
                    { 0, 0, 0, 1 }
                });
        }
        public static AffineTransformation operator *(AffineTransformation t1, AffineTransformation t2)
        {
            double[,] matrix = new double[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    matrix[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        matrix[i, j] += t1.matrix[i, k] * t2.matrix[k, j];
                }
            return new AffineTransformation(matrix);
        }
    }
}
