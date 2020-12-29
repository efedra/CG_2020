using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{

    public partial class Form1 : Form
    {
        private int height;
        private int width;
        public Form1()
        {
            InitializeComponent();
            height = Height;
            width = Width;
        }
       
        private bool redraw = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                MessageBox.Show("Не выбрана функция :(", "Ошибка", MessageBoxButtons.OK);
            else
            {
                double from = double.Parse(textBox2.Text);
                double to = double.Parse(textBox3.Text);
                if (from >= to)
                {
                    MessageBox.Show("Неправильный интервал :(", "Ошибка", MessageBoxButtons.OK);
                }
              
                if (comboBox1.SelectedIndex == 0)
                {
                    draw_function((x) => Math.Sin(x));
                    redraw = true;
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    draw_function((x) => Math.Pow(x, 2));
                    redraw = true;
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    draw_function((x) => Math.Cos(x));
                    redraw = true;
                }
              
            }
        }
        private Bitmap graph;
        private void draw_function(Func<double,double> fun)
        {
            //нахождение минимального и максимального значения функции
            double Max = int.MinValue;
            double Min = int.MaxValue;
            double from = double.Parse(textBox2.Text);
            double to = double.Parse(textBox3.Text);
            for (double i = from; i <= to; i += 0.1)
            {
                double res = fun(i);
                if (res < Min)
                    Min = res;
                if (res > Max)
                    Max = res;
            }

            double intervals = 400;
            double step = Math.Abs(to - from) / intervals;
            double scaleX = (pictureBox1.Size.Width / Math.Abs(to - from));
            double scaleY = (pictureBox1.Size.Height / Math.Abs(Max - Min));

            graph = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(graph))
            {
                g.Clear(Color.White);
               
                Point p1 = new Point(0, pictureBox1.Size.Height - (int)Math.Truncate((fun(from) - Min) * scaleY));
                for (double i = from; i <= to; i += step)
                {
                    
                    Pen pen = new Pen(Color.Red, 2);
                    double f = fun(i);
                    Point p2 = new Point((int)Math.Truncate((i - from) * scaleX), pictureBox1.Size.Height - (int)Math.Truncate((f - Min) * scaleY));
                    g.DrawLine(pen, p1, p2);
                    p1 = p2;
                }
            }
            pictureBox1.Image = graph;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }

}

