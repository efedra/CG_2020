using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab6
{
    public partial class Form2 : Form
    {
       
        Graphics g;
        Pen pen;
        PointF left, right;
        Pen clear;

        Thread thr;
        public Form2(Form form1)
        {
            
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pen = new Pen(Color.Black, 3);
            clear = new Pen(Color.White, 3);
            g = Graphics.FromImage(pictureBox1.Image);
            left = new PointF(0, pictureBox1.Height - pictureBox1.Height / 4);
            right = new PointF(pictureBox1.Width - 1, pictureBox1.Height - pictureBox1.Height / 4);
            g.DrawLine(pen, left, right);
        }

        private void build_mountain()
        {
            Thread.Sleep(100);
        }
        private void button_start_Click(object sender, EventArgs e)
        {
            double hl = double.Parse(text_box_HL.Text);
            double hr = double.Parse(text_box_HR.Text);
            double R = double.Parse(textBox_R.Text);

            g.DrawLine(clear, left, right);
            Random rnd = new Random();

            Queue<Tuple<Tuple<PointF, double>, Tuple<PointF, double>>> q =
                new Queue<Tuple<Tuple<PointF, double>, Tuple<PointF, double>>>();
            q.Enqueue(new Tuple<Tuple<PointF, double>, Tuple<PointF, double>>
                (new Tuple<PointF, double>(left, hl), new Tuple<PointF, double>(right, hr)));
            while (q.Count > 0)
            {
                var cur_segment = q.Dequeue();
                double cur_len = (double)(cur_segment.Item2.Item1.X - cur_segment.Item1.Item1.X);
                if (cur_len <= 5)
                    continue;
                PointF middle = new PointF((float)((cur_segment.Item1.Item1.X + cur_segment.Item2.Item1.X) / 2.0), left.Y);
                double h_middle = (cur_segment.Item1.Item2 + cur_segment.Item2.Item2) / 2.0 +
                    (-R * cur_len + (rnd.NextDouble() * 2) * cur_len * R);

                g.DrawLine(clear, new PointF(cur_segment.Item1.Item1.X, cur_segment.Item1.Item1.Y - (float)cur_segment.Item1.Item2),
                    new PointF(cur_segment.Item2.Item1.X, cur_segment.Item2.Item1.Y - (float)cur_segment.Item2.Item2));
                g.DrawLine(pen, new PointF(cur_segment.Item1.Item1.X, cur_segment.Item1.Item1.Y - (float)cur_segment.Item1.Item2),
                    new PointF(middle.X, middle.Y - (float)h_middle));
                g.DrawLine(pen, new PointF(middle.X, middle.Y - (float)h_middle),
                    new PointF(cur_segment.Item2.Item1.X, cur_segment.Item2.Item1.Y - (float)cur_segment.Item2.Item2));
                pictureBox1.Invalidate();
                thr = new System.Threading.Thread(build_mountain);
                thr.Start();
                thr.Join();
                thr.Abort();
                var mid = new Tuple<PointF, double>(middle, h_middle);
                q.Enqueue(new Tuple<Tuple<PointF, double>, Tuple<PointF, double>>(cur_segment.Item1, mid));
                q.Enqueue(new Tuple<Tuple<PointF, double>, Tuple<PointF, double>>(mid, cur_segment.Item2));
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            g.Clear(pictureBox1.BackColor);
            g.DrawLine(pen, left, right);
            pictureBox1.Invalidate();
        }
    }
}
