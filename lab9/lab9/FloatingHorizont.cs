using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class FloatingHorizont
    {
        public delegate double delFunct(double x, double y);
        public static delFunct[] DicFun = new delFunct[] { (x, y) => Math.Sin(x + y), (x, y) => Math.Cos(x*x+y*y)/(x*x+y*y+1),
                                                     (x, y) => Math.Sin(x)*Math.Cos(y)};


        public static Bitmap DrawHorizont(int width, int height, int IndBox, double xStart, double xEnd, double xStep, double yStart, double yEnd)
        {
            Bitmap newImg = new Bitmap(width, height);
            delFunct fun = DicFun[IndBox];

            var max = new double[width];
            var min = new double[width];
            for (int i = 0; i < width; i++)
            {
                max[i] = double.MinValue;
                min[i] = double.MaxValue;
            }
            var dx = width / 2;
            var dy = height / 2;

            var angle = -Form1.y_angle;
            var dangle = -Form1.x_angle;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var dcos = Math.Cos(dangle);
            var dsin = Math.Sin(dangle);
            for (double i = yEnd; i > yStart; i -= xStep)
            {
                for (int bmp_po_x = -width / 2; bmp_po_x < width / 2; bmp_po_x++)
                {
                    var x = (bmp_po_x + xStart) / Form1.dist;

                    var rotatei = cos * i - sin * x;
                    var rotatej = sin * i + cos * x;

                    if ((int)bmp_po_x + dx >= width || (int)bmp_po_x + dx < 0)
                        continue;

                    var z_proect = dcos * rotatei + dsin * fun(rotatei, rotatej);

                    if (min[(int)bmp_po_x + dx] > z_proect)
                    {
                        min[(int)bmp_po_x + dx] = z_proect;

                        if ((int)bmp_po_x + dx < width && (int)bmp_po_x + dx >= 0 && (int)(z_proect * Form1.dist) + dy >= 0 && (int)(z_proect * Form1.dist) + dy < height)
                            newImg.SetPixel((int)bmp_po_x + dx, (int)(z_proect * Form1.dist) + dy, Color.Cyan);
                    }

                    if (max[(int)bmp_po_x + dx] < z_proect)
                    {
                        max[(int)bmp_po_x + dx] = z_proect;

                        if ((int)bmp_po_x + dx < width && (int)bmp_po_x + dx >= 0 && (int)(z_proect * Form1.dist) + dy >= 0 && (int)(z_proect * Form1.dist) + dy < height)

                            newImg.SetPixel((int)bmp_po_x + dx, (int)(z_proect * Form1.dist) + dy, Color.White);

                    }

                }

            }
            return newImg;
        }
    }
}

