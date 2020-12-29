using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace lab6
{
    public partial class Form1 : Form
    {
        private Form2 form2;
        Bitmap bmp;
        int iteration=0;
        
        string fname = "curve_kokh.txt";

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            form2 = new Form2(this) { Visible = false };
            old_bmp = new Bitmap(bmp);
            pictureBox1.Image = bmp;
        }
        
        //Clear button
        private void button6_Click(object sender, EventArgs e)
        {
            bmp.Dispose();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            PointF curr_point = new Point(0, 0);
            curve.Clear();
            curve = new CurveBeziers();
            move = false;
            delete = false;
            add = true;
            choosen_point = new Point(0, 0);
            old_bmp = new Bitmap(bmp);
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!     1     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Draw Lsystem
        private void button1_Click(object sender, EventArgs e)
        {
            var l = new Lsystem(fname,iteration,pictureBox1.Width,pictureBox1.Height);
            l.Draw(ref bmp,false);
            pictureBox1.Image = bmp;
        }
        
        //Choose file with aksioms
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT(*.txt)|*.TXT|All Files(*.*)| *.*";

            if (ofd.ShowDialog() == DialogResult.OK) 
                try
                {
                    fname = ofd.FileName;
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            ofd.Dispose();
        }

       

        //Draw Tree
        private void button8_Click(object sender, EventArgs e)
        {
            fname = "trees1_rand.txt";
            var l = new Lsystem(fname, iteration, pictureBox1.Width, pictureBox1.Height);
            l.DrawTree(ref bmp);
            pictureBox1.Image = bmp;
        }

        //Draw Random
        private void button9_Click(object sender, EventArgs e)
        {
            var l = new Lsystem(fname, iteration, pictureBox1.Width, pictureBox1.Height);
            l.Draw(ref bmp, true);
            pictureBox1.Image = bmp;
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!     2     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Change r
        

        //Draw midpoint displacement
        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            form2.Visible = true;
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!     3     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        CurveBeziers curve = new CurveBeziers();
        PointF click_point = new Point(0, 0);
        PointF choosen_point = new Point(0, 0);
        LinkedListNode<PointF> choosen_nodePoint = null;
        Bitmap old_bmp;

        bool move = false;
        bool delete = false;
        bool add = true;

        //Add button
        private void button4_Click(object sender, EventArgs e)
        {
            add = true;
            move = false;
            delete = false;
        }

        //Move button
        private void button7_Click(object sender, EventArgs e)
        {
            if (!delete && !move)
            {
                old_bmp.Dispose();
                old_bmp = new Bitmap(bmp);
            }
            move = true;
            add = false;
            if (choosen_nodePoint != null && !space)
            {
                choosen_point = choosen_nodePoint.Value;
                bmp.Dispose();
                bmp = new Bitmap(old_bmp);
                pictureBox1.Image = bmp;
                move = false;
                curve.MovePoint(choosen_point, click_point);
                bmp.SetPixel((int)click_point.X, (int)click_point.Y, Color.Black);
                bmp.SetPixel((int)choosen_point.X, (int)choosen_point.Y, pictureBox1.BackColor);
                curve.Draw(ref bmp);
                pictureBox1.Image = bmp;
                choosen_nodePoint = null;
            }
            space = false;
        }

        bool space = false;
        //Delete button
        private void button5_Click(object sender, EventArgs e)
        {
            if (!delete && !move)
            {
                old_bmp.Dispose();
                old_bmp = new Bitmap(bmp);
            }
            add = false;
            delete = true;
            if (choosen_nodePoint != null && !space)
            {
                choosen_point = choosen_nodePoint.Value;
                bmp.Dispose();
                bmp = new Bitmap(old_bmp);
                pictureBox1.Image = bmp;
                delete = false;
                curve.DeletePoint(choosen_point, ref bmp);
                bmp.SetPixel((int)choosen_point.X, (int)choosen_point.Y, pictureBox1.BackColor);
                curve.Draw(ref bmp);
                pictureBox1.Image = bmp;
                choosen_nodePoint = null;
            }
            space = false;
        }

        //выбор новой точки
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            click_point = e.Location;
            if (e.Button == MouseButtons.Right)
                curve.closed = true;
            if (add)
            {
                curve.AddPoint(click_point, ref bmp);
                pictureBox1.Image = bmp;
            }
        }

        //Перебор нажатий на space
        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (choosen_nodePoint == null || choosen_nodePoint.Next == null)
                    choosen_nodePoint = curve.Points.First;
                else
                    choosen_nodePoint = choosen_nodePoint.Next;
                choosen_point = choosen_nodePoint.Value;
                bmp.Dispose();
                bmp = new Bitmap(old_bmp);
                OldFunctions.drawVeryFancy(new Point((int)choosen_point.X, (int)choosen_point.Y), Color.LimeGreen, ref bmp);
                pictureBox1.Image = bmp;
                space = true;
            }
        }
        //Change number of iteration
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                iteration = int.Parse(textBox1.Text);
            else iteration = 0;
        }
    }

}
