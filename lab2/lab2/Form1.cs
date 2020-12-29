using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2
{
    public partial class Form1 : Form
    {
        private Bitmap bmp;
        private Bitmap bmpHSV;

        public Form1()
        {
            InitializeComponent();
        }

        //gray (PAL & NTSC)
        private int gray1(Color color)
        {
            return (int)(0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
        }

        //gray (HDTV)
        private int gray2(Color color)
        {
            return (int)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
        }

        //count minimum of gray 
        private int gray3(Bitmap bmp)
        {
            int res = 0;
            //getting the minimal difference
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    int g1 = gray1(bmp.GetPixel(x, y));
                    int g2 = gray2(bmp.GetPixel(x, y));
                    if (g1 - g2 < res)
                    {
                        res = g1 - g2;
                    }
                }
            }
            return res;
        }

        //setting color for each pixel depending on clicked radio button
        private void setPixelColor(Bitmap bmp, int x, int y, int graySub = 0)
        {
            Color color = bmp.GetPixel(x, y);
            int a = color.A;
            int r = color.R;
            int g = color.G;
            int b = color.B;

            //gettting the number of the radio button from its name
            RadioButton radioBtn = this.Controls.OfType<RadioButton>().Where(z => z.Checked).FirstOrDefault();
            string str = radioBtn.Name;
            string res = string.Empty;
            int val = 10;
            for (int j = 0; j < str.Length; j++)
            {
                if (Char.IsDigit(str[j]))
                    res += str[j];
            }
            if (res.Length > 0)
                val = int.Parse(res);

            //switching buttons
            switch (val)
            {
                //red
                case 1:
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, 0, 0));
                    break;
                //green
                case 2:
                    bmp.SetPixel(x, y, Color.FromArgb(a, 0, g, 0));
                    break;
                //blue
                case 3:
                    bmp.SetPixel(x, y, Color.FromArgb(a, 0, 0, b));
                    break;
                //cyan
                case 4:
                    bmp.SetPixel(x, y, Color.FromArgb(a, 0, 255 - r, 255 - r));
                    break;
                //magenta
                case 5:
                    bmp.SetPixel(x, y, Color.FromArgb(a, 255 - g, 0, 255 - g));
                    break;
                //yellow
                case 6:
                    bmp.SetPixel(x, y, Color.FromArgb(a, 255 - b, 255 - b, 0));
                    break;
                //gray (PAL & NTSC)
                case 7:
                    int g1 = gray1(color);
                    bmp.SetPixel(x, y, Color.FromArgb(a, g1, g1, g1));
                    break;
                //gray (HDTV)
                case 8:
                    int g2 = gray2(color);
                    bmp.SetPixel(x, y, Color.FromArgb(a, g2, g2, g2));
                    break;
                //gray (substracted)
                case 10:
                    int gr1 = gray1(color);
                    int gr2 = gray2(color);
                    bmp.SetPixel(x, y, Color.FromArgb(a, gr1 - gr2 - graySub, gr1 - gr2 - graySub, gr1 - gr2 - graySub));
                    break;
                //default image
                default:
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                    break;
            }
        }

        //hystogram RGB
        private void doHystogramRGB(Bitmap bmp)
        {
            //hystrogram
            chart1.Series.Clear();
            chart1.Name = "RGB Channels";

            chart1.Series.Add("RED");
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series.Add("GREEN");
            chart1.Series[1].Color = Color.Green;
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series.Add("BLUE");
            chart1.Series[2].Color = Color.Blue;
            chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            Dictionary<int, int> countR = new Dictionary<int, int>();
            Dictionary<int, int> countG = new Dictionary<int, int>();
            Dictionary<int, int> countB = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                countR[i] = 0;
                countG[i] = 0;
                countB[i] = 0;
            }
            for (int x = 1; x < bmp.Width; x++)
            {
                for (int y = 1; y < bmp.Height; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    countR[color.R] += 1;
                    countG[color.G] += 1;
                    countB[color.B] += 1;
                }
            }
            for (int i = 1; i < 256; i++)
            {
                chart1.Series["RED"].Points.AddXY(i, countR[i]);
                chart1.Series["GREEN"].Points.AddXY(i, countG[i]);
                chart1.Series["BLUE"].Points.AddXY(i, countB[i]);
            }
        }

        //hystogram GRAY
        private void doHystogramGRAY(Bitmap bmp)
        {
            //hystrogram
            chart2.Series.Clear();
            chart2.Name = "Gray Channels";

            chart2.Series.Add("Gray PAL and NTSC");
            chart2.Series[0].Color = Color.Cyan;
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart2.Series.Add("Gray HDTV");
            chart2.Series[1].Color = Color.Magenta;
            chart2.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            Dictionary<int, int> countG1 = new Dictionary<int, int>();
            Dictionary<int, int> countG2 = new Dictionary<int, int>();
            for (int i = 0; i < 256; i++)
            {
                countG1[i] = 0;
                countG2[i] = 0;
            }
            for (int x = 1; x < bmp.Width; x++)
            {
                for (int y = 1; y < bmp.Height; y++)
                {
                    Color color = bmp.GetPixel(x, y);
                    countG1[gray1(color)] += 1;
                    countG2[gray2(color)] += 1;
                }
            }
            for (int i = 1; i < 256; i++)
            {
                chart2.Series["Gray PAL and NTSC"].Points.AddXY(i, countG1[i]);
                chart2.Series["Gray HDTV"].Points.AddXY(i, countG2[i]);
            }
        }

        //choose image
        private void button1_Click(object sender, EventArgs e)
        {
            string imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox1.ImageLocation = imageLocation;
                    pictureBox2.ImageLocation = imageLocation;
                    pictureBox3.ImageLocation = imageLocation;
                    bmp = new Bitmap(pictureBox1.ImageLocation);
                    bmpHSV = new Bitmap(pictureBox1.ImageLocation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //GO (tasks 1 & 2)
        private void button2_Click(object sender, EventArgs e)
        {
            bmp.Dispose();
            bmp = new Bitmap(pictureBox1.ImageLocation);

            int gr = gray3(bmp);
            //changing the second picture
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    setPixelColor(bmp, x, y, gr);
                }
            }
            pictureBox2.Image = bmp;
            doHystogramRGB(bmp);
            doHystogramGRAY(bmp);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int a = trackBar1.Value;
            int b = trackBar2.Value;
            int c = trackBar3.Value;
            HSV(a, b, c);
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            int a = trackBar1.Value;
            int b = trackBar2.Value;
            int c = trackBar3.Value;
            HSV(a, b, c);
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            int a = trackBar1.Value;
            int b = trackBar2.Value;
            int c = trackBar3.Value;
            HSV(a, b, c);
        }

        //task 3
        private void HSV(int a, int b, int c)
        {
            double h, s, v;
            for (int y = 0; y < bmpHSV.Height; y++)
                for (int x = 0; x < bmpHSV.Width; x++)
                {
                    rgb2hsv(bmp.GetPixel(x, y), out h, out s, out v);
                    h = ((int)(h + a * 10 + 180)) % 360;
                    int s1 = ((int)((s * 100) + b * 10 - 100));
                    if (s1 > 100)
                        s = 1;
                    else if (s1 < 0)
                        s = 0;
                    else s = s1 * 0.01;

                    int v1 = ((int)((v * 100) + c * 10 - 100));
                    if (v1 > 100)
                        v = 1;
                    else if (v1 < 0)
                        v = 0;
                    else v = v1 * 0.01;
                    //v = (((int)((v * 100) + trackBar3.Value*10 + 50)) % 100) * 0.01;
                    bmpHSV.SetPixel(x, y, hsv2rgb(h, s, v));
                }
            pictureBox3.Image = bmpHSV;

        }

        private void rgb2hsv(Color c, out double h, out double s, out double v)
        {
            double r = c.R / (255 * 1.0);
            double g = c.G / (255 * 1.0);
            double b = c.B / (255 * 1.0);
            double min_ = Math.Min(r, Math.Min(g, b));
            double max_ = Math.Max(r, Math.Max(g, b));
            //H
            if (max_ == min_)
                h = 0;
            else if (max_ == r)
            {
                h = 60 * (g - b) / (max_ - min_);
                if (g < b)
                    h += 360;
            }
            else if (max_ == g)
                h = 60 * (b - r) / (max_ - min_) + 120;
            else
                h = 60 * (r - g) / (max_ - min_) + 240;
            //S
            s = max_ == 0 ? 0 : 1 - (min_ / max_);
            //V
            v = max_;
        }

        private Color hsv2rgb(double h, double s, double v)
        {
            int h_i = ((int)Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);
            int p = (int)(v * (1 - s) * 255);
            int q = (int)(v * (1 - f * s) * 255);
            int t = (int)(v * (1 - (1 - f) * s) * 255);
            int V = (int)(v * 255);


            switch (h_i)
            {
                case 0: return Color.FromArgb(V, t, p);
                case 1: return Color.FromArgb(q, V, p);
                case 2: return Color.FromArgb(p, V, t);
                case 3: return Color.FromArgb(p, q, V);
                case 4: return Color.FromArgb(t, p, V);
                default: return Color.FromArgb(V, p, q);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            pictureBox1.Image.Save(pictureBox1.ImageLocation);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save(pictureBox1.ImageLocation.Replace(".", "(1)."));
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox3.Image.Save(pictureBox1.ImageLocation.Replace(".", "(2)."));
        }

    }
}
