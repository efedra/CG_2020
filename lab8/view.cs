
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CG_lab7
{
    class view
    {
        public delegate Point Projection(Point3D p, int width = 100, int height = 100);
        public static void show(Polyhedron p, Bitmap bmp, int proj, bool front_face = false)
        {
            camera = new Point3D(0, 0, dist, 1);
            camera.Apply(Matrix.Rotate(-(x_angle / Math.PI) * 180, -(y_angle / Math.PI) * 180, 0));

            List<Point3D> pnts = p.GetPoints();
            show_axis(bmp, proj);

            var faces = front_face? p.front_faces(camera) : p.GetFaces();
            switch (proj)
            {
                case 0:
                    for (int i = 0; i < faces.Count; i++)
                    {
                        for (int j = 0; j < faces[i].Count - 1; j++)
                        {
                            Point3D p1 = pnts[faces[i][j]];
                            Point3D p2 = pnts[faces[i][j + 1]];
                            Wu(iso3Dto2D(p1, bmp.Width, bmp.Height), iso3Dto2D(p2, bmp.Width, bmp.Height), Color.Black, ref bmp);
                        }
                        Wu(iso3Dto2D(pnts[faces[i][faces[i].Count - 1]], bmp.Width, bmp.Height),
                            iso3Dto2D(pnts[faces[i][0]], bmp.Width, bmp.Height), Color.Black, ref bmp);
                    }
                    break;
                default:
                    Projection projection = view.ort3DXto2D;
                    switch (proj)
                    {
                        case 2: projection = view.ort3DYto2D; break;
                        case 3: projection = view.ort3DZto2D; break;
                        case 4: projection = view.persp3Dto2D; break;
                    }

                    for (int i = 0; i < faces.Count; i++)
                    {
                        for (int j = 0; j < faces[i].Count - 1; j++)
                        {
                            Point3D p1 = pnts[faces[i][j]];
                            Point3D p2 = pnts[faces[i][j + 1]];
                            Wu(projection(p1, bmp.Width, bmp.Height), projection(p2, bmp.Width, bmp.Height), Color.Black, ref bmp);
                        }
                        Wu(projection(pnts[faces[i][faces[i].Count - 1]], bmp.Width, bmp.Height),
                            projection(pnts[faces[i][0]], bmp.Width, bmp.Height), Color.Black, ref bmp);
                    }
                    break;
            }
        }

        public static void show_axis(Bitmap bmp, int proj)
        {
            List<Point3D> axis = new List<Point3D> { new Point3D(0, 0, 0), new Point3D(400, 0, 0), new Point3D(0, 400, 0), new Point3D(0, 0, 400) };
            List<Color> colors = new List<Color> { Color.Black, Color.IndianRed, Color.LimeGreen, Color.SteelBlue };

            switch (proj)
            {
                case 0:
                    for (int i = 0; i < 4; i++)
                        Wu(iso3Dto2D(axis[0], bmp.Width, bmp.Height), iso3Dto2D(axis[i], bmp.Width, bmp.Height), colors[i], ref bmp);
                    break;
                default:
                    Projection projection = view.ort3DXto2D;
                    switch (proj)
                    {
                        case 2: projection = view.ort3DYto2D; break;
                        case 3: projection = view.ort3DZto2D; break;
                        case 4: projection = view.persp3Dto2D; break;
                    }
                    for (int i = 0; i < 4; i++)
                        Wu(projection(axis[0], bmp.Width, bmp.Height), projection(axis[i], bmp.Width, bmp.Height), colors[i], ref bmp);
                    break;
            }
                
        }

        public static Point3D camera;

        public static double y_angle = 0;
        public static double x_angle = 0;
        public static double dist = 200;

        public static Point persp3Dto2D(Point3D p, int width = 100, int height = 100)
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

            double[,] res = { {p.X, p.Y, p.Z, 1 } };
            res = Matrix.mult_matr(res, Matrix.mult_matr(Matrix.mult_matr(rotate_y, rotate_x), persp_matr));

            if (res[0, 2] > dist)
                res[0, 2] = -res[0, 2];
            return new Point(width / 2 + (int)(res[0, 0] / (1- res[0, 2] / dist)), 
                height / 2 - (int)(res[0, 1] / (1- res[0, 2] / dist)));
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
            double[,] c = Matrix.mult_matr(ort_matrX, point);
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
            double[,] c = Matrix.mult_matr(ort_matrY, point);
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
            double[,] c = Matrix.mult_matr(ort_matrZ, point);
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

            double[,] iso_matr = Matrix.mult_matr(m1, m2);
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = Matrix.mult_matr(iso_matr, point);
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
                        if (c > 0)
                            bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
                        if (c < 255)
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
                        if (c > 0)
                            bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(c, color.R, color.G, color.B));
                        if (c < 255)
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

        public static Point3D persp3Dto2DForZB(Point3D p, int width = 100, int height = 100)
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

            double[,] res = { { p.X, p.Y, p.Z, 1 } };
            res = Matrix.mult_matr(res, Matrix.mult_matr(Matrix.mult_matr(rotate_y, rotate_x), persp_matr));

            if (res[0, 2] > dist)
                res[0, 2] = -res[0, 2];
            return new Point3D(width / 2 + (int)(res[0, 0] / (1 - res[0, 2] / dist)),
                height / 2 - (int)(res[0, 1] / (1 - res[0, 2] / dist)), res[0, 2]);
        }


        public static Point3D iso3Dto2DForZB(Point3D p, int width = 100, int height = 100, double phi = 145, double xi = 45)
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

            double[,] iso_matr = Matrix.mult_matr(m1, m2);
            double[,] point = { { p.X }, { p.Y }, { p.Z }, { 1 } };
            double[,] c = Matrix.mult_matr(iso_matr, point);
            return new Point3D(width / 2 + (int)c[0, 0], height / 2 + (int)c[1, 0], p.Z);
        }
    }
}