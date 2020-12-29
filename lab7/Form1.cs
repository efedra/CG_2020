using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_lab7
{
    public partial class Form1 : Form
    {

        Bitmap bmp, bmp_rotation;

        Polyhedron figures;
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp_rotation = new Bitmap(pictureBox2.Width, pictureBox2.Height);

            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp_rotation;
            figures = new Polyhedron();
        }


        private void ChoosePojection()
        {
            RadioButton radioBtn = panel4.Controls.OfType<RadioButton>().Where(z => z.Checked).FirstOrDefault();
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
            switch (val)
            {
                case 15: radioButton15_CheckedChanged(null, null); break;
                case 16: radioButton16_CheckedChanged(null, null); break;
                case 18: radioButton18_CheckedChanged(null, null); break;
                case 17: radioButton17_CheckedChanged(null, null); break;
                case 19: radioButton19_CheckedChanged(null, null); break;
            }
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            view.Projection_iso f = view.iso3Dto2D;
            clear();
            view.show(figures, bmp, f);
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }
        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            view.Projection f = view.ort3DXto2D;
            clear();
            view.show(figures,bmp,f);
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }
        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            view.Projection f = view.ort3DYto2D;
            clear();
            Form1_Load(null, null);
            view.show(figures, bmp, f);
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();

        }
        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            view.Projection f = view.ort3DZto2D;
            clear();
            view.show(figures, bmp, f);
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();

        }
        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            view.Projection f = view.persp3Dto2D;
            clear();
            view.show(figures, bmp, f);
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

        private void clear()
        {
            bmp.Dispose();
            bmp_rotation.Dispose();
            draw = false;
            points_rotation.Clear();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp_rotation = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp_rotation;
            Form1_Load(null, null);
        }
        
        //button IMPORT
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFileDialog1.Title = "Open Object File";
            openFileDialog1.Filter = "OBJ files|*.obj";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                figures.FromFile(selectedFileName);
            }

            redraw();
        }

        //button SAVE
        private void button2_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            saveFileDialog1.Title = "Save object File";
            saveFileDialog1.CheckFileExists = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "obj";
            saveFileDialog1.Filter = "Object files (*.obj)|*.obj|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                figures.ToFile(saveFileDialog1.FileName);
            }
        }

        //button CLEAR
        private void button4_Click(object sender, EventArgs e)
        {
            figures.Clear();
            clear();
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }


        private List<Point3D> to3DPoints(List<Point> points, int axis)
        {
            List<Point3D> points3D = new List<Point3D>();
            (int height, int width) = (bmp_rotation.Height, bmp_rotation.Width);
            switch (axis)
            {
                case 1: 
                    foreach(var p in points)
                        points3D.Add(new Point3D((height - p.Y)/2,0,(p.X-width/3)/2 ));
                    break;
                case 2:
                    foreach (var p in points)
                        points3D.Add(new Point3D(0,(height - p.Y)/2, (p.X - width / 3)/2)); 
                    break;
                case 3:
                    foreach (var p in points)
                        points3D.Add(new Point3D((p.X - width /3)/2 ,0,(height - p.Y)/2)); 
                    break;
            }
            return points3D;
        }

        private int ChooseAxis()
        {
            RadioButton radioBtn = panel1.Controls.OfType<RadioButton>().Where(z => z.Checked).FirstOrDefault();
            string str = radioBtn.Name;
            string res = string.Empty;
            for (int j = 0; j < str.Length; j++)
            {
                if (Char.IsDigit(str[j]))
                    res += str[j];
            }
            return res.Length > 0 ? int.Parse(res) : 10;
        }


        //button DRAW
        private void button1_Click(object sender, EventArgs e)
        {
            int axis = ChooseAxis();
            var points3D = to3DPoints(points_rotation,axis);
            points_rotation.Clear();
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
            int last_ind = all_points.Count()-1;
            //List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
            List<List<int>> polygons = new List<List<int>>();
            //for (int i = 0; i < temp1.Count() - 1; i++)
            //    edges.Add(new Tuple<int, int>(last_ind - i, last_ind - i - 1));
            for (int k=1; k < count_split;k++)
            {
                for (int i = 0; i < temp1.Count(); i++)
                {
                    temp1[i].Apply(AffineTransformation.Translate(-center.X, -center.Y, -center.Z));
                    temp1[i].Apply(AffineTransformation.RotateLineAngle(vec, -sum));
                    temp1[i].Apply(AffineTransformation.Translate(center.X, center.Y, center.Z));
                    all_points.Add(temp1[i]);
                    last_ind = all_points.Count() - 1;
                    //edges.Add(new Tuple<int, int>(last_ind, last_ind - temp1.Count()));
                }
                last_ind = all_points.Count() - 1;

                for (int i = 0; i < temp1.Count() - 1; i++)
                {
                    //edges.Add(new Tuple<int, int>(last_ind - i, last_ind - i - 1));
                    polygons.Add(new List<int> { last_ind - i, last_ind - i - 1, last_ind - i -1- temp1.Count(), last_ind - i - temp1.Count() });

                }
                temp1 = Copy(points3D);
                sum +=angle;
            }

            last_ind = all_points.Count() - 1;
            //for (int i = 0; i < temp1.Count(); i++)
            //    edges.Add(new Tuple<int, int>(last_ind - i, temp1.Count()-1 -i));
            for (int i = 0; i < temp1.Count()-1; i++)
                polygons.Add(new List<int> { last_ind - i, last_ind - i - 1, temp1.Count() - 2 - i, temp1.Count() -1-i });

            figures = new Polyhedron(all_points, polygons);
            redraw();
        }

        private Point3D findCenterRotationFigure(List <Point3D> points,int axis)
        {
            double sum = 0;
            foreach(var p in points)
            {
                switch(axis)
                {
                    case 1: sum += p.X; break;
                    case 2: sum += p.Y; break;
                    case 3: sum += p.Z; break;
                }
            }
            switch (axis)
            {
                case 1: return new Point3D(sum/points.Count,0,0);
                case 2: return new Point3D(0,sum / points.Count, 0);
                case 3: return new Point3D(0,0,sum / points.Count);
            }
            return new Point3D();
        }


        private List<Point3D> Copy(List<Point3D> points)
        {
            var l = new List<Point3D>();
            foreach (var p in points)
                l.Add(new Point3D(p.X, p.Y, p.Z));
            return l;
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!AFFINE TRANSFORM

        int dx = 0, dy = 0, dz = 0;
        //move x
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            dx = trackBar1.Value;
            label6.Text = "X=" + dx;
        }

        //move y
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            dy = trackBar2.Value;
            label7.Text = "Y=" + dy;
        }

        //move z
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            dz = trackBar3.Value;
            label8.Text = "Z=" + dz;
        }

        //move
        private void button19_Click(object sender, EventArgs e)
        {
            figures.Apply(AffineTransformation.Translate(dx, dy, dz));
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            label6.Text = "X=0";
            label7.Text = "Y=0";
            label8.Text = "Z=0";
            dx = 0;
            dy = 0;
            dz = 0;
            redraw();
        }



        //отражение относительно OX
        private void reflectionX_Click(object sender, EventArgs e)
        {
            figures.Apply(AffineTransformation.ReflectX());
            redraw();
        }


        //отражение относительно OY
        private void button5_Click(object sender, EventArgs e)
        {
            figures.Apply(AffineTransformation.ReflectY());
            redraw();
        }


        //отражение относительно OZ
        private void button6_Click(object sender, EventArgs e)
        {
            figures.Apply(AffineTransformation.ReflectZ());
            redraw();
        }

        int angle_rotqate = 36;


        //вращение вокруг прямой через центр параллеьной OX <
        private void button8_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(1, 0, 0);
            Parallel_rotate(vec, angle_rotqate);
        }

        //вращение вокруг прямой через центр параллеьной OX >
        private void button9_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(1, 0, 0);
            Parallel_rotate(vec, -angle_rotqate);
        }

        //вращение вокруг прямой через центр параллеьной OY <
        private void button11_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(0, 1, 0);
            Parallel_rotate(vec, angle_rotqate);
        }

        //вращение вокруг прямой через центр параллеьной OY >
        private void button10_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(0, 1, 0);
            Parallel_rotate(vec, -angle_rotqate);
        }

        //вращение вокруг прямой через центр параллеьной OZ <
        private void button13_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(0, 0, 1);
            Parallel_rotate(vec, angle_rotqate);
        }

        //вращение вокруг прямой через центр параллеьной OZ >
        private void button12_Click(object sender, EventArgs e)
        {
            Point3D vec = new Point3D(0, 0, 1);
            Parallel_rotate(vec, -angle_rotqate);
        }

        private void Parallel_rotate(Point3D vec, double angle)
        {
            Point3D center = new Point3D(figures.center_point.X, figures.center_point.Y, figures.center_point.Z);
            figures.Apply(AffineTransformation.Translate(-figures.center_point.X, -figures.center_point.Y, -figures.center_point.Z));
            figures.Apply(AffineTransformation.RotateLineAngle(vec, angle));
            figures.Apply(AffineTransformation.Translate(center.X + figures.center_point.X, center.Y + figures.center_point.Y, center.Z + figures.center_point.Z));
            redraw();

        }

        double angle_tb = 0;

        double left_x = 0, right_x = 0;
        double left_y = 0, right_y = 0;
        double left_z = 0, right_z = 0;

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            var text = textBox6.Text;
            right_z = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            var text = textBox3.Text;
            left_z = (text == "" ? 0 : int.Parse(text));
        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            var text = textBox5.Text;
            right_y = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            var text = textBox2.Text;
            left_y = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            left_x = (text == "" ? 0 : int.Parse(text));
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            var text = textBox4.Text;
            right_x = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            var text = textBox7.Text;
            angle_tb = text == "" ? 0 : double.Parse(text);
        }


        //находит единичный вектор вектора
        private Point3D normal_vect(Point3D p)
        {
            Point3D norm = new Point3D();
            double dist = Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
            if (dist == 0)
                return norm;
            return p / dist;
        }

        private void Parallel_rotate_Point(Point3D vec, double angle)
        {
            figures.Apply(AffineTransformation.RotateLineAngle(vec, angle));
            redraw();
        }

        //вращение произв прямой по Z <
        private void button16_Click(object sender, EventArgs e)
        {
            Point3D first = new Point3D(left_x, left_y, left_z);
            Point3D last = new Point3D(right_x, right_y, right_z);
            Point3D vec = last - first;
            Parallel_rotate_Point(normal_vect(vec), angle_tb);
        }

        int deg_x = 0, deg_y = 0, deg_z = 0;
        //rotate x
        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            deg_x = -360 * trackBar6.Value / 20;
            label11.Text = "deg_X= " + -deg_x;
        }

        //rotate y
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            deg_y = -360 * trackBar5.Value / 20;
            label10.Text = "deg_Y= " + -deg_y;
        }

        //rotate z
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            deg_z = -360 * trackBar4.Value / 20;
            label9.Text = "deg_Z= " + -deg_z;
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!GRAPHICS

        int count_split = 10;
        //number of splits
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            count_split = (text == "" ? 0 : int.Parse(text));
        }

        List<Point> points_rotation = new List<Point>();
        bool draw = false;
        Point tmp;
        bool first = false;

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (!draw)
            {
                draw = true;
                tmp = e.Location;
                first = true;
            }
            else
            {
                if (tmp.X < bmp_rotation.Width / 3 && e.Location.X < bmp_rotation.Width / 3)
                {
                    tmp = e.Location;
                    return;
                }
                if (first)
                {
                    if (tmp.X < bmp_rotation.Width / 3)
                    {
                        if (e.Location.X > bmp_rotation.Width / 3)
                        {
                            tmp = CrossPoint(tmp, e.Location, new Point(bmp_rotation.Width / 3, 0), new Point(bmp_rotation.Width / 3, bmp_rotation.Height));
                            first = false;
                        }else
                        {
                            tmp = e.Location;
                            return;
                        }
                    }
                    points_rotation.Add(tmp);
                    
                }
                Point second = e.Location;
                if (e.X < bmp_rotation.Width / 3)
                    second = CrossPoint(tmp,second,new Point( bmp_rotation.Width / 3,0), new Point(bmp_rotation.Width / 3, bmp_rotation.Height));
                points_rotation.Add(second);
                view.Wu(second, tmp, Color.Black, ref bmp_rotation);
                pictureBox2.Image = bmp_rotation;
                pictureBox2.Invalidate();
                tmp = second;

            }
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                points_rotation.Add(points_rotation[0]);
                view.Wu(points_rotation[0], tmp, Color.Black,ref bmp_rotation);
                pictureBox2.Image = bmp_rotation;
                pictureBox2.Invalidate();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Point first = new Point(bmp_rotation.Width / 3, 0);
            Point last = new Point(bmp_rotation.Width / 3, bmp_rotation.Height);

            view.Wu(first, last, Color.Red, ref bmp_rotation);
            pictureBox2.Image = bmp_rotation;
            pictureBox2.Invalidate();
        }

        //rotate
        private void rotate_Click(object sender, EventArgs e)
        {
            trackBar6.Value = 0;
            trackBar5.Value = 0;
            trackBar4.Value = 0;
            figures.Apply(AffineTransformation.RotateX(deg_x) * AffineTransformation.RotateY(deg_y) * AffineTransformation.RotateZ(deg_z));
            deg_x = 0;
            deg_y = 0;
            deg_z = 0;
            label9.Text = "deg_Z=0";
            label10.Text = "deg_Y=0";
            label11.Text = "deg_X=0";

            redraw();
        }

        //scale center - масштабирование отностильно центра многогранника
        private void scaleCENTER_Click(object sender, EventArgs e)
        {
            trackBar9.Value = 0;
            trackBar8.Value = 0;
            trackBar7.Value = 0;
            Point3D center = new Point3D(figures.center_point.X, figures.center_point.Y, figures.center_point.Z);
            figures.Apply(AffineTransformation.Translate(-figures.center_point.X, -figures.center_point.Y, -figures.center_point.Z));
            figures.Apply(AffineTransformation.Scale(scale_x / 100.0, scale_y / 100.0, scale_z / 100.0));
            figures.Apply(AffineTransformation.Translate(center.X + figures.center_point.X, center.Y + figures.center_point.Y, center.Z + figures.center_point.Z));

            scale_x = 100;
            scale_y = 100;
            scale_z = 100;
            label14.Text = scale_x + " %";
            label13.Text = scale_y + " %";
            label12.Text = scale_z + " %";
            redraw();

        }

        int scale_x = 100, scale_y = 100, scale_z = 100;


        public delegate double delFunct(double x, double y);
        private delFunct[] DicFun = new delFunct[] { (x, y) => Math.Sin(x + y), (x, y) => x * x + x * y + y * y,
                                                     (x, y) => 0.5 * (x * x + y * y)};
        private void button14_Click(object sender, EventArgs e)
        {
            delFunct fun = DicFun[comboBox1.SelectedIndex];
            double step = (double)count_split;

            List<List<int>> faces = new List<List<int>>();
            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();

            int x0 = Int32.Parse(textBox2.Text);
            int x1 = Int32.Parse(textBox4.Text);
            int y0 = Int32.Parse(textBox3.Text);
            int y1 = Int32.Parse(textBox5.Text);
            double dx = (x1 - x0) / step;
            double dy = (y1 - y0) / step;

            List<Point3D> points = new List<Point3D>();


            double curX = x0;
            double curY = y0;

            for (int i = 0; i < step + 1; ++i)
            {
                curY = y0;
                for (int j = 0; j < step + 1; ++j)
                {

                    points.Add(new Point3D((curX - x0) * 10, (curY - y0) * 10, fun(curX, curY) * 10));
                    if (i > 0)
                    {
                        edges.Add(new Tuple<int, int>((i - 1) * (count_split + 1) + j, i * (count_split + 1) + j));
                    }
                    if (j > 0)
                    {
                        edges.Add(new Tuple<int, int>(i * (count_split + 1) + j - 1, i * (count_split + 1) + j));
                    }
                    if (i > 0 && j > 0)
                    {
                        faces.Add(new List<int> { (i - 1) * (count_split + 1) + j - 1, (i - 1) * (count_split + 1) + j, (i) * (count_split + 1) + j, (i) * (count_split + 1) + j - 1 });
                    }
                    curY += dy;
                }
                curX += dx;
            }


            figures = new Polyhedron(points, faces);

            redraw();
            pictureBox1.Image = bmp;
            pictureBox1.Invalidate();
        }

        //scale x
        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            scale_x = 200 * trackBar9.Value / 20 + 100;
            label14.Text = scale_x + " %";
        }

        //scale y
        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            scale_y = 200 * trackBar8.Value / 20 + 100;
            label13.Text = scale_y + " %";
        }

        //scale z
        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            scale_z = 200 * trackBar7.Value / 20 + 100;
            label12.Text = scale_z + " %";
        }

        //scale - масштабирование относительно начала координат
        private void button7_Click(object sender, EventArgs e)
        {
            trackBar9.Value = 0;
            trackBar8.Value = 0;
            trackBar7.Value = 0;

            figures.Apply(AffineTransformation.Scale(scale_x / 100.0, scale_y / 100.0, scale_z / 100.0));

            scale_x = 100;
            scale_y = 100;
            scale_z = 100;
            label14.Text = scale_x + " %";
            label13.Text = scale_y + " %";
            label12.Text = scale_z + " %";

            redraw();
        }

        //пересечения ребер
        // Уравнение прямой, проходящей через две заданные точки (x1,y1) и (x2,y2):
        // (y1 - y2)x + (x2 - x1)y + (x1y2 - x2y1) = 0
        private void lineEquation(Point e0, Point e1, out float A, out float B, out float C)
        {
            A = e0.Y - e1.Y;
            B = e1.X - e0.X;
            C = e0.X * e1.Y - e1.X * e0.Y;
        }


        // Зная уравнения прямых : A1x + B1y + C1 = 0 и A2x + B2y + C2 = 0, 
        // получим точку пересечения по формуле Крамера:
        // x = - (C1B2 - C2B1)/(A1B2 - A2B1)
        // y = - (A1C2 - A2C1)/(A1B2 - A2B1)
        private Point findPoint(float A1, float A2, float B1, float B2, float C1, float C2, out float x, out float y)
        {
            var denom = A1 * B2 - A2 * B1;
            x = -1 * (C1 * B2 - C2 * B1) / denom;
            y = -1 * (A1 * C2 - A2 * C1) / denom;
            return (new Point((int)x, (int)y));

        }
        private Point CrossPoint(Point e1_0, Point e1_1, Point e2_0, Point e2_1)
        {
            float A1, B1, C1, A2, B2, C2, xRes, yRes;
            lineEquation(e1_0, e1_1, out A1, out B1, out C1);
            lineEquation(e2_0, e2_1, out A2, out B2, out C2);
            var p = findPoint(A1, A2, B1, B2, C1, C2, out xRes, out yRes);

            if ((p.X >= Math.Min(e1_0.X, e1_1.X)) && (p.X <= Math.Max(e1_0.X, e1_1.X)) &&
                (p.X >= Math.Min(e2_0.X, e2_1.X)) && (p.X <= Math.Max(e2_0.X, e2_1.X)) &&
                (p.Y >= Math.Min(e1_0.Y, e1_1.Y)) && (p.Y <= Math.Max(e1_0.Y, e1_1.Y)) &&
                (p.Y >= Math.Min(e2_0.Y, e2_1.Y)) && (p.Y <= Math.Max(e2_0.Y, e2_1.Y)))
            {
                return p;
            }
            else
                return new Point(-1, -1);
        }

        public void redraw()
        {

            bmp.Dispose();
            bmp_rotation.Dispose();
            draw = false;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp_rotation = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            pictureBox1.Image = bmp;
            pictureBox2.Image = bmp_rotation;
            Form1_Load(null, null);

            ChoosePojection();
            
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
        }

    }
}
