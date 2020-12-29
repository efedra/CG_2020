
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_3D
{
    class view
    {
        public static void show(Polyhedron p, Bitmap bmp1, Bitmap bmp2, Bitmap bmp3, Bitmap bmp4, Bitmap bmp5)
        {
            Pen pen = new Pen(Color.Black);
            List<Point3D> pnts = p.GetPoints();
            show_axis(bmp1, bmp2, bmp3, bmp4, bmp5);


            for (int i = 0; i < p.GetEdges().Count; i++)
            {
                Point3D p1 = pnts[p.GetEdges()[i].Item1];
                Point3D p2 = pnts[p.GetEdges()[i].Item2];
                Wu(iso3Dto2D(p1, bmp1.Width, bmp1.Height), iso3Dto2D(p2, bmp1.Width, bmp1.Height), Color.Black, ref bmp1);
                Wu(ort3DXto2D(p1, bmp1.Width, bmp1.Height), ort3DXto2D(p2, bmp1.Width, bmp1.Height), Color.Black, ref bmp2);
                Wu(ort3DYto2D(p1, bmp1.Width, bmp1.Height), ort3DYto2D(p2, bmp1.Width, bmp1.Height), Color.Black, ref bmp3);
                Wu(ort3DZto2D(p1, bmp1.Width, bmp1.Height), ort3DZto2D(p2, bmp1.Width, bmp1.Height), Color.Black, ref bmp4);
                Wu(persp3Dto2D(p1, bmp1.Width, bmp1.Height), persp3Dto2D(p2, bmp1.Width, bmp1.Height), Color.Black, ref bmp5);
            }
        }

        public static void show_axis(Bitmap bmp1, Bitmap bmp2, Bitmap bmp3, Bitmap bmp4, Bitmap bmp5)
        {
            List<Point3D> axis = new List<Point3D> { new Point3D(0, 0, 0), new Point3D(400, 0, 0), new Point3D(0, 400, 0), new Point3D(0, 0, 400) };
            List<Color> colors = new List<Color> { Color.Black, Color.IndianRed, Color.LimeGreen, Color.SteelBlue };

            for (int i = 0; i < 4; i++)
            {

                Wu(iso3Dto2D(axis[0], bmp1.Width, bmp1.Height), iso3Dto2D(axis[i], bmp1.Width, bmp1.Height), colors[i], ref bmp1);
                Wu(ort3DXto2D(axis[0], bmp1.Width, bmp1.Height), ort3DXto2D(axis[i], bmp1.Width, bmp1.Height), colors[i], ref bmp2);
                Wu(ort3DYto2D(axis[0], bmp1.Width, bmp1.Height), ort3DYto2D(axis[i], bmp1.Width, bmp1.Height), colors[i], ref bmp3);
                Wu(ort3DZto2D(axis[0], bmp1.Width, bmp1.Height), ort3DZto2D(axis[i], bmp1.Width, bmp1.Height), colors[i], ref bmp4);
                Wu(persp3Dto2D(axis[0], bmp1.Width, bmp1.Height), persp3Dto2D(axis[i], bmp1.Width, bmp1.Height), colors[i], ref bmp5);
            }
        }

        public static Point persp3Dto2D(Point3D p, int width = 100, int height = 100)
        {
            int a = 200;
            int b = 200;
            int c = 200;

            double[,] persp_matr =
                { {1, 0, 0, -1 / a},
                { 0, 1, 0, -1 / b},
                { 0, 0, 1, -1 / c},
                { 0, 0, 0, 1} };

            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] res = AffineTransformation.mult_matr(persp_matr, point);
            return new Point(width / 2 + (int)(res[0, 0] * (1 - p.Z / c)), height / 2 - (int)(res[1, 0] * (1 - p.Z / c)));
            //return new Point(width / 2 + (int)((res[0, 0] - res[2, 0]) / Math.Sqrt(2)), height / 2 + (int)((res[0, 0] + 2 * res[1, 0] + res[2, 0]) / Math.Sqrt(6)));
        }

        public static Point ort3DXto2D(Point3D p, int width = 100, int height = 100)
        {
            double[,] ort_matrX =
            {
                { 0, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = AffineTransformation.mult_matr(ort_matrX, point);
            return new Point(width / 2 + (int)c[1, 0], height / 2 - (int)c[2, 0]);
        }

        public static Point ort3DYto2D(Point3D p, int width = 100, int height = 100)
        {
            double[,] ort_matrY =
            {
                { 1, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, 1 }
            };
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = AffineTransformation.mult_matr(ort_matrY, point);
            return new Point(width / 2 + (int)c[0, 0], height / 2 - (int)c[2, 0]);
        }

        public static Point ort3DZto2D(Point3D p, int width = 100, int height = 100)
        {
            double[,] ort_matrZ =
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 1 }
            };
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = AffineTransformation.mult_matr(ort_matrZ, point);
            return new Point(width / 2 + (int)c[0, 0], height / 2 - (int)c[1, 0]);
        }

        public static Point iso3Dto2D(Point3D p, int width = 100, int height = 100, double phi = 145, double xi = 45)
        {

            double radPhi = phi * Math.PI / 180;
            double radXi = xi * Math.PI / 180;

            double[,] m2 = {
                { Math.Cos(radXi), 0, -Math.Sin(radXi), 0},
                { 0, 1, 0, 0},
                { Math.Sin(radXi), 0, Math.Cos(radXi), 0},
                {0, 0, 0, 1 } };
            double[,] m1 = {
                { 1, 0, 0, 0},
                { 0, Math.Cos(radPhi), Math.Sin(radPhi) , 0},
                { 0, -Math.Sin(radPhi), Math.Cos(radPhi), 0},
                {0, 0, 0, 1 } };

            double[,] iso_matr = AffineTransformation.mult_matr(m1, m2);
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = AffineTransformation.mult_matr(iso_matr, point);
            return new Point(width / 2 + (int)c[0, 0], height / 2 + (int)c[1, 0]);
        }


        public static void Wu(Point p1, Point p2, Color color, ref Bitmap bmp)
        {
            int x1 = p1.X; int y1 = p1.Y;
            int x2 = p2.X; int y2 = p2.Y;
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
                        bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
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
                        bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
                        bmp.SetPixel((int)xi, (int)yi + step, Color.FromArgb(255 - c, color.R, color.G, color.B));
                    }
                    yi += gradient;
                }
            }
        }
        //check if point is out of the Picture Box
        public static bool out_of_PB(int x, int y, ref Bitmap bmp)
        {
            return x <= 0 || y <= 0 || x >= bmp.Width || y >= bmp.Height;
        }
    }
}