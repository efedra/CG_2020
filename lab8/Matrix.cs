using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_lab7
{
    public class Matrix
    {
        private double[,] data = new double[4, 4];

        public double[,] Data { get { return data; } }

        public Matrix()
        {
            data = 
                new double[,] { 
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public Matrix(double[,] matrix)
        {
            this.data = matrix;
        }

        public static Matrix Move(double dx, double dy, double dz)
        {
            return new Matrix(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                });
        }

        public static Matrix Rotate(double angleX, double angleY, double angleZ)
        {
            double cosX = Math.Cos(angleX * Math.PI / 180);
            double sinX = Math.Sin(angleX * Math.PI / 180);
            double cosY = Math.Cos(angleY * Math.PI / 180);
            double sinY = Math.Sin(angleY * Math.PI / 180);
            double cosZ = Math.Cos(angleZ * Math.PI / 180);
            double sinZ = Math.Sin(angleZ * Math.PI / 180);
            double[,] x =
                {
                    { 1, 0, 0, 0 },
                    { 0, cosX, -sinX, 0 },
                    { 0, sinX, cosX, 0 },
                    { 0, 0, 0, 1 }
                };
            double[,] y =
                {
                    { cosY, 0, sinY, 0 },
                    { 0, 1, 0, 0 },
                    { -sinY, 0, cosY, 0 },
                    { 0, 0, 0, 1 }
                };
            double[,] z =
                {
                    { cosZ, -sinZ, 0, 0 },
                    { sinZ, cosZ, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
            if (angleX == 0 && angleY == 0)
                return new Matrix(z);
            if (angleX == 0 && angleZ == 0)
                return new Matrix(y);
            if (angleY == 0 && angleZ == 0)
                return new Matrix(x);

            if (angleX == 0)
                return new Matrix(mult_matr(y, z));
            if (angleY == 0)
                return new Matrix(mult_matr(x, z));
            if (angleZ == 0)
                return new Matrix(mult_matr(x, y));

            return new Matrix(mult_matr(mult_matr(x, y), z));
        }

        public Point3D Scale_point(double x, double y, double alpha, double beta, double gamma, Point3D p)
        {
            double[,] scaling_matr = {
                { alpha, 0, 0, 0 },
                { 0, beta, 0, 0 },
                { 0, 0, gamma, 0 },
                { 0, 0, 0, 1 } };

            double[,] point = { { p.X, p.Y, p.Z, 1 } };
            double[,] c = mult_matr(point, scaling_matr);
            return new Point3D(c[0, 0], c[0, 1], c[0, 2]);
        }

        public static Matrix Scale(double fx, double fy, double fz)
        {
            return new Matrix(
                new double[,] {
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }
        public static Matrix ReflectX() => Scale(1, -1, -1);

        public static Matrix ReflectY() => Scale(-1, 1, -1);

        public static Matrix ReflectZ() => Scale(-1, -1, 1);

        public static Matrix RotateLineAngle(Point3D vec,double angle)
        {
            double l = vec.X;
            double m = vec.Y;
            double n = vec.Z;
            double phi = angle * Math.PI / 180;
            double cos = Math.Cos(phi);
            double sin = Math.Sin(phi);
            return new Matrix(
                new double[,] {
                    { l*l + cos*(1-l*l), l*(1-cos)*m + n*sin,l*(1-cos)*n - m*sin , 0 },
                    { l*(1-cos)*m - n*sin, m*m + cos*(1-m*m), m*(1-cos)*n + l*sin, 0 },
                    { l*(1-cos)*n + m*sin, m*(1-cos)*n -l*sin, n*n+ cos*(1-n*n), 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix PERSP(double y_angle, double x_angle, double dist)
        {
            double cosy = Math.Cos(y_angle);
            double siny = Math.Sin(y_angle);
            double cosx = Math.Cos(x_angle);
            double sinx = Math.Sin(x_angle);
            double[,] rotate_y =
                {
                    { cosy, 0, siny, 0 },
                    { 0, 1, 0, 0 },
                    { -siny, 0, cosy, 0 },
                    { 0, 0, 0, 1 }
                };

            double[,] rotate_x =
                {
                    { 1, 0, 0, 0 },
                    { 0, cosx, -sinx, 0 },
                    { 0, sinx, cosx, 0 },
                    { 0, 0, 0, 1 }
                };
            double[,] persp_matr =
                {
                    { 1, 0, 0, dist == 0 ? 0 : (-1 / dist)},
                    { 0, 1, 0, dist == 0 ? 0 : (-1 / dist)},
                    { 0, 0, 1, dist == 0 ? 0 : (-1 / dist)},
                    { 0, 0, 0, 1}
                };
            return new Matrix(mult_matr(mult_matr(rotate_y, rotate_x), persp_matr));
        }


        public static double[,] mult_matr(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0))
            {
                throw new Exception("Count column in first matrix and count of rows in second are not equal");
            }

            var c = new double[a.GetLength(0), b.GetLength(1)];
            for (var i = 0; i < a.GetLength(0); i++)
                for (var j = 0; j < b.GetLength(1); j++)
                {
                    c[i, j] = 0;
                    for (var k = 0; k < a.GetLength(1); k++)
                        c[i, j] += a[i, k] * b[k, j];
                }
            return c;
        }

        public static Matrix operator *(Matrix t1, Matrix t2)
        {
            double[,] matrix = new double[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    matrix[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        matrix[i, j] += t1.data[i, k] * t2.data[k, j];
                }
            return new Matrix(matrix);
        }
    }
}
