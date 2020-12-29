using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_3D
{
    public partial class Form1 : Form
    {
        Bitmap bmp_iso, bmp_ortX, bmp_ortY, bmp_ortZ, bmp_pers;

        Polyhedron figures;
        public Form1()
        {
            InitializeComponent();

            bmp_iso = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp_ortX = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            bmp_ortY= new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmp_ortZ = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            bmp_pers = new Bitmap(pictureBox5.Width, pictureBox5.Height);

            figures = new Polyhedron();
            figures.Hexahedron(100);
            redraw();

        }
        int dx=0, dy=0, dz=0;
        //move x
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            dx = trackBar1.Value;
            label6.Text = "X=" + dx;
        }

        //move y
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            dy = trackBar2.Value ;
            label7.Text = "Y=" + dy;
        }

        //move z
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            dz = trackBar3.Value;
            label8.Text = "Z=" + dz;

        }

        //move
        private void button1_Click(object sender, EventArgs e)
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
        private void button4_Click(object sender, EventArgs e)
        {
            AffineTransformation reflection= new AffineTransformation();
            figures.Apply(reflection.ReflectX());

            redraw();

        }


        //отражение относительно OY
        private void button5_Click(object sender, EventArgs e)
        {
            AffineTransformation reflection = new AffineTransformation();
            figures.Apply(reflection.ReflectY());

            redraw();
        }


        //отражение относительно OZ
        private void button6_Click(object sender, EventArgs e)
        {
            AffineTransformation reflection = new AffineTransformation();
            figures.Apply(reflection.ReflectZ());

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
            //Point3D p = figures.center_point;
            Point3D center = new Point3D(figures.center_point.X, figures.center_point.Y, figures.center_point.Z);
            figures.Apply(AffineTransformation.Translate(-figures.center_point.X, -figures.center_point.Y, -figures.center_point.Z));
            figures.Apply(AffineTransformation.RotateLineAngle(vec, angle));
            figures.Apply(AffineTransformation.Translate(center.X + figures.center_point.X, center.Y + figures.center_point.Y, center.Z + figures.center_point.Z));
            redraw();
        }



        //clear
        private void button14_Click(object sender, EventArgs e)
        {
            
            ChooseFigure(sender);
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            var text = textBox3.Text;
            left_z = (text == "" ? 0 : int.Parse(text));
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            var text = textBox5.Text;
            right_y = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            var text = textBox2.Text;
            left_y = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            left_x = (text == "" ? 0 : int.Parse(text));
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            var text = textBox4.Text;
            right_x = (text == "" ? 0 : int.Parse(text));
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            var text = textBox7.Text;
            angle_tb = text == "" ? 0: double.Parse(text);
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

        //rotate
        private void button2_Click(object sender, EventArgs e)
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
        private void button3_Click(object sender, EventArgs e)
        {
            trackBar9.Value = 0;
            trackBar8.Value = 0;
            trackBar7.Value = 0;
            Point3D center = new Point3D(figures.center_point.X, figures.center_point.Y, figures.center_point.Z);
            figures.Apply(AffineTransformation.Translate(-figures.center_point.X, -figures.center_point.Y, -figures.center_point.Z));
            figures.Apply(AffineTransformation.Scale(scale_x / 100.0, scale_y / 100.0, scale_z / 100.0));
            figures.Apply(AffineTransformation.Translate(center.X + figures.center_point.X,center.Y+ figures.center_point.Y,center.Z+ figures.center_point.Z));

            scale_x = 100;
            scale_y = 100;
            scale_z = 100;
            label14.Text = scale_x + " %";
            label13.Text = scale_y + " %";
            label12.Text = scale_z + " %";
            redraw();
        }

        int scale_x = 100, scale_y = 100, scale_z = 100;




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

            figures.Apply(AffineTransformation.Scale(scale_x/100.0, scale_y/100.0, scale_z/100.0));

            scale_x = 100;
            scale_y = 100;
            scale_z = 100;
            label14.Text = scale_x + " %";
            label13.Text = scale_y + " %";
            label12.Text = scale_z + " %";

            redraw();

        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            figures.Hexahedron(100);
            redraw();
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            figures.Octahedron(100);
            redraw();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            figures.Tetrahedron(100);
            redraw();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            figures.Icosahedron(100);
            redraw();
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            
            figures.Dodecahedron(100);
            redraw();
        }


        private void ChooseFigure(object sender)
        {
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
            switch (val)
            {
                case 1: radioButton1_CheckedChanged(sender, null); break;
                case 2: radioButton2_CheckedChanged(sender, null); break;
                case 3: radioButton3_CheckedChanged(sender, null); break;
                case 4: radioButton4_CheckedChanged(sender, null); break;
                case 5: radioButton5_CheckedChanged(sender, null); break;

            }
        }
        public void redraw()
        {

            bmp_iso.Dispose();
            bmp_ortX.Dispose();
            bmp_ortY.Dispose();
            bmp_ortZ.Dispose();
            bmp_pers.Dispose();

            bmp_iso = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            bmp_ortX = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            bmp_ortY = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            bmp_ortZ = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            bmp_pers = new Bitmap(pictureBox5.Width, pictureBox5.Height);

            view.show(figures, bmp_iso, bmp_ortX, bmp_ortY, bmp_ortZ, bmp_pers);

            pictureBox1.Image = bmp_iso;
            pictureBox2.Image = bmp_ortX;
            pictureBox3.Image = bmp_ortY;
            pictureBox4.Image = bmp_ortZ;
            pictureBox5.Image = bmp_pers;
            pictureBox1.Invalidate();
            pictureBox2.Invalidate();
            pictureBox3.Invalidate();
            pictureBox4.Invalidate();
            pictureBox5.Invalidate();
        }
    }
}
