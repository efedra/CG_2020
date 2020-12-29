using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace lab6
{
    class OldFunctions
    {

        int width = 0;
        int height = 0;
        public OldFunctions(int w, int h)
        {
            width = w;
            height = h;
        }

        public PointF scale_shift_point(double x, double y, double dx, double dy, double alpha, double beta, PointF p)
        {
            double[,] scale_shift_matr = {
                { alpha, 0, 0 },
                { 0, beta, 0 },
                { (1-alpha)*p.X+dx,(1-beta)*p.Y +dy, 1 }
            };

            double[,] point = { { x, y, 1 } };
            var c = mult_matr(point, scale_shift_matr);
            return new PointF((float)c[0, 0], (float)c[0, 1]);
        }

        // shift (x,y) to dx and dy coordinate lines
        public Point shift_point(double x, double y, double dx, double dy)
        {
            double[,] shift_matr = {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { dx, dy, 1 }
            };

            double[,] point = { { x, y, 1 } };
            var c = mult_matr(point, shift_matr);
            return new Point((int)c[0, 0], (int)c[0, 1]);
        }


        // rotate (x,y) in point p and angle phi
        public Point rotate_point(double x, double y, Point p, double degree)
        {
            double phi = Math.PI * degree / 180;
            double[,] rotation_matr = { {Math.Cos(phi), Math.Sin(phi), 0 },
                { -Math.Sin(phi), Math.Cos(phi), 0},
                { -p.X *Math.Cos(phi)+ p.Y*Math.Sin(phi)+ p.X, -p.X * Math.Sin(phi)- p.Y *Math.Cos(phi) + p.Y, 1 } };

            double[,] point = { { x, y, 1 } };
            var c = mult_matr(point, rotation_matr);
            return new Point((int)c[0, 0], (int)c[0, 1]);
        }

        // scaling (x,y) with coef alpha, betta and in Point p
        public PointF scale_point(double x, double y, double alpha, double beta, PointF p)
        {
            double[,] scaling_matr = {
                { alpha, 0, 0 },
                { 0, beta, 0 },
                { (1-alpha) * p.X, (1-beta) *  p.Y, 1 } };

            double[,] point = { { x, y, 1 } };
            var c = mult_matr(point, scaling_matr);
            return new Point((int)c[0, 0], (int)c[0, 1]);
        }

        private static double[,] mult_matr(double[,] a, double[,] b)
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

        //расстояние от точки до прямой
        public static double Distance(Point p1, Point p2, Point point)
        {
            double length = Math.Sqrt((p2.Y - p1.Y) * (p2.Y - p1.Y) + (p2.X - p1.X) * (p2.X - p1.X));
            return ((p2.Y - p1.Y) * point.X + (p2.X - p1.X) * point.Y + p2.X * p1.Y - p2.Y * p1.X) / length;
        }
        //distance between points
        public static double distance(PointF p1, PointF p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        //draw huge colored button
        public static void drawFancy(Point pnt, Color clr1, ref Bitmap bmp)
        {
            if (!out_of_PB(pnt.X, pnt.Y, ref bmp))
                bmp.SetPixel(pnt.X, pnt.Y, clr1);
            if (!out_of_PB(pnt.X - 1, pnt.Y, ref bmp))
                bmp.SetPixel(pnt.X - 1, pnt.Y, clr1);
            if (!out_of_PB(pnt.X, pnt.Y - 1, ref bmp))
                bmp.SetPixel(pnt.X, pnt.Y - 1, clr1);
            if (!out_of_PB(pnt.X + 1, pnt.Y, ref bmp))
                bmp.SetPixel(pnt.X + 1, pnt.Y, clr1);
            if (!out_of_PB(pnt.X, pnt.Y + 1, ref bmp))
                bmp.SetPixel(pnt.X, pnt.Y + 1, clr1);

            if (!out_of_PB(pnt.X - 1, pnt.Y - 1, ref bmp))
                bmp.SetPixel(pnt.X - 1, pnt.Y - 1, clr1);
            if (!out_of_PB(pnt.X + 1, pnt.Y - 1, ref bmp))
                bmp.SetPixel(pnt.X + 1, pnt.Y - 1, clr1);
            if (!out_of_PB(pnt.X + 1, pnt.Y + 1, ref bmp))
                bmp.SetPixel(pnt.X + 1, pnt.Y + 1, clr1);
            if (!out_of_PB(pnt.X - 1, pnt.Y + 1, ref bmp))
                bmp.SetPixel(pnt.X - 1, pnt.Y + 1, clr1);
        }//draw huge colored button
        public static void drawVeryFancy(Point pnt, Color clr1, ref Bitmap bmp)
        {
            Point p1 = new Point(pnt.X - 3, pnt.Y - 3);
            Point p2 = new Point(pnt.X - 3, pnt.Y + 3);
            Point p3 = new Point(pnt.X + 3, pnt.Y + 3);
            Point p4 = new Point(pnt.X + 3, pnt.Y - 3);
            Wu(p1, p2, clr1, ref bmp);
            Wu(p2, p3, clr1, ref bmp);
            Wu(p3, p4, clr1, ref bmp);
            Wu(p4, p1, clr1, ref bmp);
        }

        //check if point is out of the Picture Box
        public static bool out_of_PB(int x, int y, ref Bitmap bmp)
        {
            return x <= 0 || y <= 0 || x >= bmp.Width || y >= bmp.Height;
        }


        public static void Wu(PointF p1, PointF p2, Color color, ref Bitmap bmp)
        {
            Wu((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, color, ref bmp);
        }
        //draw smooth line
        public static void Wu(int x1, int y1, int x2, int y2, Color color, ref Bitmap bmp)
        {
            //use only 4 right octants
            if (x1 > x2)
            {
                int t = y1;
                y1 = y2;
                y2 = t;
                t = x1;
                x1 = x2;
                x2 = t;
            }
            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient = dx == 0 ? 1 : dy / (double)dx;
            int step = 1;
            double xi = x1;
            double yi = y1;

            //1 & 4 octants
            if (Math.Abs(gradient) > 1 || dx == 0)
            {
                gradient = dx == 0 ? 0 : 1 / gradient;
                //4 octant
                if (dy < 0)
                {
                    xi = x2;
                    step = -1;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }

                for (yi = y1; yi <= y2; yi += 1)
                {
                    if (!out_of_PB((int)xi, (int)yi, ref bmp) && !out_of_PB((int)xi + step, (int)yi, ref bmp))
                    {
                        int c = gradient < 0 ? (int)(255 * Math.Abs(xi - (int)xi)) : 255 - (int)(255 * Math.Abs(xi - (int)xi));
                        if (c != 0)
                            bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
                        if (255 - c != 0)
                            bmp.SetPixel((int)xi + step, (int)yi, Color.FromArgb(255 - c, color.R, color.G, color.B));
                    }
                    xi += gradient;
                }
            }
            //2 & 3 octants
            else
            {
                //3 octant
                if (gradient < 0)
                {
                    step = -1;
                }

                for (xi = x1; xi <= x2; xi += 1)
                {
                    if (!out_of_PB((int)xi, (int)yi + step, ref bmp) && !out_of_PB((int)xi, (int)yi, ref bmp))
                    {
                        int c = gradient < 0 ? (int)(255 * Math.Abs(yi - (int)yi)) : 255 - (int)(255 * Math.Abs(yi - (int)yi));
                        if (c != 0)
                            bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
                        if (255 - c != 0)
                            bmp.SetPixel((int)xi, (int)yi + step, Color.FromArgb(255 - c, color.R, color.G, color.B));
                    }
                    yi += gradient;
                }
            }
        }

        //центр описанной окружности
        public static Point circle_center(List<Point> line, Point point)
        {
            int x1 = line[0].X;
            int y1 = line[0].Y;
            int x2 = line[1].X;
            int y2 = line[1].Y;
            int x3 = point.X;
            int y3 = point.Y;
            int d = 2 * ((x1 - x2) * (y3 - y1) - (y1 - y2) * (x3 - x1));
            int x = (y1 - y2) * (x3 * x3 + y3 * y3) + (y2 - y3) * (x1 * x1 + y1 * y1) + (y3 - y1) * (x2 * x2 + y2 * y2);
            x = d == 0 ? x : x / (-d);
            int y = (x1 - x2) * (x3 * x3 + y3 * y3) + (x2 - x3) * (x1 * x1 + y1 * y1) + (x3 - x1) * (x2 * x2 + y2 * y2);
            y = d == 0 ? y : y / d;
            return new Point(x, y);
        }

        //рисование окружности
        public static void draw_circle(int x, int y, int r, Color color, ref Bitmap bmp)
        {
            const double PI = 3.1415926535;
            double i, angle, x1, y1;

            for (i = 0; i < 360; i += 0.1)
            {
                angle = i;
                x1 = r * Math.Cos(angle * PI / 180);
                y1 = r * Math.Sin(angle * PI / 180);
                if (!out_of_PB(x + (int)x1, y + (int)y1, ref bmp))
                    bmp.SetPixel(x + (int)x1, y + (int)y1, color);
            }
        }
    }
}
