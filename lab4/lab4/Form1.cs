using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace lab4
{
    public partial class Form1 : Form
    {
        public Bitmap bmp;
        List<List<Point>> polygons = new List<List<Point>>();

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
        }

        /////////////////////////////////////////////////////////////Affine transformation

        // shift (x,y) to dx and dy coordinate lines
        private Point shift_point(double x, double y, double dx, double dy)
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
        private Point rotate_point(double x, double y, Point p, double degree)
        {
            double phi =  Math.PI * degree / 180;
            double [,] rotation_matr = { {Math.Cos(phi), Math.Sin(phi), 0 },
                { -Math.Sin(phi), Math.Cos(phi), 0},
                { -p.X *Math.Cos(phi)+ p.Y*Math.Sin(phi)+ p.X, -p.X * Math.Sin(phi)- p.Y *Math.Cos(phi) + p.Y, 1 } };
           
            double [,] point = { { x, y, 1 } };
            var c = mult_matr(point, rotation_matr);
            return new Point((int)c[0,0], (int)c[0,1]);
        }

        // scaling (x,y) with coef alpha, betta and in Point p
        private Point scale_point(double x, double y, double alpha,double beta, Point p)
        {
            double[,] scaling_matr = { 
                { alpha, 0, 0 },
                { 0, beta, 0 },
                { (1-alpha) * p.X, (1-beta) *  p.Y, 1 } };

            double[,] point = { { x, y, 1 } };
            var c = mult_matr(point, scaling_matr);
            return new Point((int)c[0, 0], (int)c[0, 1]);
        }

        private static double[,] mult_matr(double [,] a, double [,] b)
        {
            (int a_row, int a_collumn)  = (a.GetLength(0), a.GetLength(1));
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


        // Get the point P(t) = a + t * (b-a)
        private Point parametric_equation(Point a, Point b, double t)
        {
            return new Point((int) (a.X + t*(b.X-a.X)),(int) (a.Y + t * (b.Y - a.Y)));
        }


        //--------------------1--------------------

        //boolean to wait for user to enter coordinates & index for the view list
        int point_index = 0;
        bool set_line = false; int line_index = 0;
        bool set_rectangle = false; int rectangle_index = 0;
        bool set_star = false; int star_index = 0;
        bool set_star5 = false; int star5_index = 0;
        bool set_heart = false; int heart_index = 0;
        //coordinates  of first point in case of line or rectangle
        Point tmp;


        //draw huge colored button
        private void drawFancy(Point pnt, Color clr1)
        {
            if (!out_of_PB(pnt.X,pnt.Y))
                bmp.SetPixel(pnt.X, pnt.Y, clr1);
            if (!out_of_PB(pnt.X-1, pnt.Y))
                bmp.SetPixel(pnt.X - 1, pnt.Y, clr1);
            if (!out_of_PB(pnt.X, pnt.Y-1))
                bmp.SetPixel(pnt.X, pnt.Y - 1, clr1);
            if (!out_of_PB(pnt.X+1, pnt.Y))
                bmp.SetPixel(pnt.X + 1, pnt.Y, clr1);
            if (!out_of_PB(pnt.X, pnt.Y+1))
                bmp.SetPixel(pnt.X, pnt.Y + 1, clr1);
            pictureBox1.Invalidate();
        }

        //selecting
        int selected_index1 = -1;
        int selected_index2 = -1;
        int selecting_next = 1;

        //when poly is selected, show it by highlighting its points
        private void selectFancy(List<Point> poly)
        {
            foreach (Point pnt in poly)
                drawFancy(pnt, Color.Red);
        }


        //any click on a pictureBox
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (set_rotate)
            {
                drawFancy(rotate_pnt, Form1.DefaultBackColor);
                rotate_pnt = e.Location;
                drawFancy(rotate_pnt, Color.Orange);
                set_rotate = false;
                return;
            }
            if (set_scale)
            {
                drawFancy(rotate_pnt, Form1.DefaultBackColor);
                scale_pnt = e.Location;
                drawFancy(scale_pnt, Color.BlueViolet);
                set_scale = false;
                return;
            }
            //setting second point of line
            if (set_line)
            {
                Wu(tmp.X, tmp.Y, e.X, e.Y);
                set_line = false;
                pictureBox1.Invalidate();

                //adding to the list of drawn primitives
                polygons.Add(new List<Point>{ new Point(tmp.X, tmp.Y), new Point(e.X, e.Y) });
                //showing all the drawn primtives
                checkedListBox1.Items.Add("line" + line_index++);
                return;
            }
            //setting second point of rectangle
            if (set_rectangle)
            {
                set_rectangle = false;
                Draw_rectangle(tmp, new Point(e.X, e.Y));////contain polygon.Add()
                //showing all the drawn primtives
                checkedListBox1.Items.Add("rectangle" + rectangle_index++);
                return;
            }
            if (set_star)
            {
                Draw_star(tmp, new Point(e.X, e.Y));////contain polygon.Add()
                set_star = false;
                checkedListBox1.Items.Add("star" + star_index++);
                return;
            }

            if (set_star5)
            {
                Draw_star5(tmp, new Point(e.X, e.Y));//contain polygon.Add()
                set_star5 = false;
                checkedListBox1.Items.Add("star5_" + star5_index++);
                return;
            }
            if (set_heart)
            {
                Draw_heart(tmp, new Point(e.X, e.Y));//contain polygon.Add()
                set_heart = false;
                checkedListBox1.Items.Add("heart" + heart_index++);
                return;
            }

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
                //Point
                case 1:
                    bmp.SetPixel(e.X, e.Y, Color.Black);
                    pictureBox1.Invalidate();
                    //adding to the list of drawn primitives
                    polygons.Add(new List<Point> { new Point(e.X, e.Y) });
                    //showing all the drawn primtives
                     checkedListBox1.Items.Add("point" + point_index++);
                    break;
                //Line
                case 2:
                    set_line = true;
                    tmp = new Point(e.X, e.Y);
                    break;
                //Square
                case 3:
                    set_rectangle = true;
                    tmp = new Point(e.X, e.Y);
                    break;
                //Star
                case 4:
                    set_star = true;
                    tmp = new Point(e.X, e.Y);
                    break;
                //Star5
                case 5:
                    set_star5 = true;
                    tmp = new Point(e.X, e.Y);
                    break;
                //Heart
                case 6:
                    set_heart = true;
                    tmp = new Point(e.X, e.Y);
                    break;
            }
        }


        private void get_border_point(Point p1, Point p2, out int left_x, out int right_x, out int up_y, out int down_y)
        {
            left_x = p1.X;
            right_x = p2.X;
            if (p1.X > p2.X)
            {
                left_x = right_x;
                right_x = p1.X;
            }
            up_y = p1.Y;
            down_y = p2.Y;
            if (p1.Y > p2.Y)
            {
                up_y = down_y;
                down_y = p1.Y;
            }
        }

        // draw_rectangle in clockwise and add to list :first - left lower point
        private void Draw_rectangle(Point p1, Point p2)
        {
            get_border_point(p1, p2,out int left_x, out int right_x, out int up_y, out int down_y);
            var l = new List<Point> { new Point(left_x, down_y), new Point(left_x, up_y), new Point(right_x, up_y), new Point(right_x, down_y)};
            polygons.Add(l);

            for (int i = 1; i < l.Count; i++)
                Wu(l[i - 1].X, l[i - 1].Y, l[i].X, l[i].Y);
            Wu(l[l.Count - 1].X, l[l.Count - 1].Y, l[0].X, l[0].Y);
            pictureBox1.Invalidate();
        }

        // draw_star in clockwise and add to list :first - lower point
        private void Draw_star(Point p1, Point p2)
        {
            get_border_point(p1, p2,out int left_x, out int right_x, out int up_y, out int down_y);
            int diff_x = right_x - left_x;
            int diff_y = down_y - up_y;

            var l = new List<Point> {new Point(left_x + diff_x / 2, down_y), new Point(left_x + diff_x / 3, down_y - diff_y / 3), 
                new Point(left_x, down_y - diff_y / 2), new Point( left_x + diff_x / 3, up_y + diff_y / 3),
                new Point(left_x + diff_x / 2, up_y),new Point(right_x - diff_x / 3, up_y + diff_y / 3),
                new Point(right_x, up_y + diff_y / 2), new Point(right_x - diff_x / 3, down_y - diff_y / 3)};
            polygons.Add(l);

            for (int i = 1; i < l.Count; i++)
                Wu(l[i - 1].X, l[i - 1].Y, l[i].X, l[i].Y);
            Wu(l[l.Count - 1].X, l[l.Count - 1].Y, l[0].X, l[0].Y);
            pictureBox1.Invalidate();
        }

        // draw_star5 in clockwise and add to list :first - left lower point
        private void Draw_star5(Point p1, Point p2)
        {
            get_border_point(p1, p2,out int left_x, out int right_x, out int up_y, out int down_y);
            int diff_x = right_x - left_x;
            int diff_y = down_y - up_y;

            var l = new List<Point> { new Point(left_x + diff_x/6,down_y), new Point(left_x + diff_x/3,down_y -2* diff_y/5), new Point(left_x, up_y + 2* diff_y/5),
                new Point(left_x + diff_x/3, up_y + 2* diff_y/5), new Point(left_x + diff_x/2, up_y), new Point(right_x -diff_x/3,up_y + 2* diff_y/5),
                new Point(right_x, up_y+ 2*diff_y/5), new Point(right_x - diff_x/3, down_y -2* diff_y/5 ), new Point(right_x - diff_x/6, down_y),
                new Point(left_x + diff_x/2,down_y - diff_y/6)};
            polygons.Add(l);
                       for (int i = 1; i < l.Count; i++)
                Wu(l[i - 1].X, l[i - 1].Y, l[i].X, l[i].Y);
            Wu(l[l.Count - 1].X, l[l.Count - 1].Y, l[0].X, l[0].Y);
            pictureBox1.Invalidate();
        }

        // draw_heart in clockwise and add to list :first - lower point
        private void Draw_heart(Point p1, Point p2)
        {
            get_border_point(p1, p2,out int left_x, out int right_x, out int up_y, out int down_y);
            int diff_x = right_x - left_x;
            int diff_y = down_y - up_y;
            
            var l = new List<Point> { new Point(left_x + diff_x / 2, down_y), new Point(left_x, up_y + diff_y / 4),
                new Point(left_x + diff_x / 4, up_y), new Point(left_x + 2 * diff_x / 4, up_y + diff_y / 4),
                new Point(right_x - diff_x / 4, up_y), new Point(right_x, up_y + diff_y / 4) };
            polygons.Add(l);


            for( int i = 1;i<l.Count;i++)
                Wu(l[i - 1].X, l[i - 1].Y, l[i].X, l[i].Y);
            Wu(l[l.Count - 1].X, l[l.Count - 1].Y, l[0].X, l[0].Y);
            pictureBox1.Invalidate();
        }

        //--------------------2--------------------

        //clearing picture box but leaving the fancy control buttons
        private void clearPB()
        {
            bmp.Dispose();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            drawFancy(rotate_pnt, Color.Orange);
            drawFancy(scale_pnt, Color.BlueViolet);
        }

        //clear picturebox and delete all polygons
        private void button1_Click(object sender, EventArgs e)
        {
            clearPB();
            polygons.Clear();
            checkedListBox1.Items.Clear();
            star5_index = 0;
            star_index = 0;
            heart_index = 0;
            line_index = 0;
            rectangle_index = 0;
            point_index = 0;
        }

        //--------------------3-1--------------------

        //move
        private void button13_Click(object sender, EventArgs e)
        {
            clearPB();
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            label1.Text = "x = 0";
            label2.Text = "y = 0";

            foreach (List<Point> point_list in polygons)
            {
                if (point_list.Count == 1)
                {
                    point_list[0] = shift_point(point_list[0].X, point_list[0].Y, dx, dy);
                    if (!out_of_PB(point_list[0].X, point_list[0].Y))
                        bmp.SetPixel(point_list[0].X, point_list[0].Y, Color.Black);
                }
                else
                {
                    point_list[0] = shift_point(point_list[0].X, point_list[0].Y, dx, dy);
                    for (int i = 1; i < point_list.Count; i++)
                    {
                        point_list[i] = shift_point(point_list[i].X, point_list[i].Y, dx, dy);
                        Wu(point_list[i - 1].X, point_list[i - 1].Y, point_list[i].X, point_list[i].Y);
                    }
                    Wu(point_list[point_list.Count - 1].X, point_list[point_list.Count - 1].Y, point_list[0].X, point_list[0].Y);
                }
            }
            dx = 0;
            dy = 0;
            pictureBox1.Invalidate();
        }
        //get distance to move
        int dx, dy;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            dx = pictureBox1.Width * trackBar1.Value / 20;
            label1.Text = "x = " + dx;
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            dy = -pictureBox1.Height * trackBar2.Value / 20;
            label2.Text = "y = " + -dy;
        }

        //--------------------3-2--------------------

        //rotate
        private void button4_Click(object sender, EventArgs e)
        {
            clearPB();
            trackBar3.Value = 0;
            label3.Text = "deg = 0";
            foreach (List<Point> poly in polygons)
            {
                if (poly.Count == 1)
                {
                    poly[0] = rotate_point(poly[0].X, poly[0].Y, rotate_pnt, deg);
                    if (!out_of_PB(poly[0].X, poly[0].Y))
                        bmp.SetPixel(poly[0].X, poly[0].Y, Color.Black);
                }
                else
                {
                    poly[0] = rotate_point(poly[0].X, poly[0].Y, rotate_pnt, deg);
                    for (int i = 1; i < poly.Count; i++)
                    {
                        poly[i] = rotate_point(poly[i].X, poly[i].Y, rotate_pnt, deg);
                        Wu(poly[i - 1].X, poly[i - 1].Y, poly[i].X, poly[i].Y);
                    }
                    Wu(poly[poly.Count - 1].X, poly[poly.Count - 1].Y, poly[0].X, poly[0].Y);
                }
            }
            pictureBox1.Invalidate();
        }

        //get rotating degree
        Point rotate_pnt;
        bool set_rotate;
        int deg;
        private void button3_Click(object sender, EventArgs e)
        {
            set_rotate = true;
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            deg = -360 * trackBar3.Value / 20;
            label3.Text = "deg = " + -deg;
        }

        //--------------------3-3-------------------

        //scale
        private void button5_Click(object sender, EventArgs e)
        {
            clearPB();
            trackBar4.Value = 0;
            trackBar5.Value = 0;

            foreach (List<Point> point_list in polygons)
            {
                if (point_list.Count == 1)
                {
                    point_list[0] = scale_point(point_list[0].X, point_list[0].Y, scale_x / 100.0, scale_y / 100.0, scale_pnt);
                    if (!out_of_PB(point_list[0].X, point_list[0].Y))
                        bmp.SetPixel(point_list[0].X, point_list[0].Y, Color.Black);
                }
                else
                {
                    point_list[0] = scale_point(point_list[0].X, point_list[0].Y, scale_x / 100.0, scale_y / 100.0, scale_pnt);
                    for (int i = 1; i < point_list.Count; i++)
                    {
                        point_list[i] = scale_point(point_list[i].X, point_list[i].Y, scale_x / 100.0, scale_y / 100.0, scale_pnt);
                        Wu(point_list[i - 1].X, point_list[i - 1].Y, point_list[i].X, point_list[i].Y);
                    }
                    Wu(point_list[point_list.Count - 1].X, point_list[point_list.Count - 1].Y, point_list[0].X, point_list[0].Y);
                }
            }
            scale_x = 100;
            scale_y = 100;
            label4.Text = scale_x + " %";
            label5.Text = scale_x + " %";
            pictureBox1.Invalidate();
        }
        //get scaling percent
        Point scale_pnt;
        bool set_scale;
        int scale_x, scale_y;
        private void button6_Click(object sender, EventArgs e)
        {
            set_scale = true;
        }

        //get x scale
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            scale_x = 200 * trackBar4.Value / 20 + 100;
            label4.Text = scale_x + " %";
        }

        //get y scale
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            scale_y = 200 * trackBar5.Value / 20 + 100;
            label5.Text = scale_y + " %";
        }

        //--------------------4-------------------
        //rotate edge 90
        private void button11_Click(object sender, EventArgs e)
        {
            bool is_edge = 1 == checkedListBox1.CheckedIndices.Count;
            is_edge = is_edge && polygons[checkedListBox1.CheckedIndices[0]].Count==2;
            if (!is_edge)
            {
                MessageBox.Show("Выберите ребро");
            }
            else
            {
                clearPB();
                var poly = polygons[checkedListBox1.CheckedIndices[0]];
                Point center = new Point((poly[0].X + poly[1].X) / 2, (poly[0].Y + poly[1].Y) / 2);
                poly[0] = rotate_point(poly[0].X, poly[0].Y, center, 90);
                poly[1] = rotate_point(poly[1].X, poly[1].Y, center, 90);

                foreach (List<Point> point_list in polygons)
                {
                    if (point_list.Count == 1)
                    {
                        if (!out_of_PB(point_list[0].X, point_list[0].Y))
                            bmp.SetPixel(point_list[0].X, point_list[0].Y, Color.Black);
                    }
                    else
                    {
                        for (int i = 1; i < point_list.Count; i++)
                        {
                            Wu(point_list[i - 1].X, point_list[i - 1].Y, point_list[i].X, point_list[i].Y);
                        }
                        Wu(point_list[point_list.Count - 1].X, point_list[point_list.Count - 1].Y, point_list[0].X, point_list[0].Y);
                    }
                }
                pictureBox1.Invalidate();
            }
        }

        //draw smooth line
        void Wu(int x1, int y1, int x2, int y2)
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
                    if (!out_of_PB((int)xi, (int)yi) && !out_of_PB((int)xi + step, (int)yi))
                    {
                        int c = gradient < 0 ? (int)(255 * Math.Abs(xi - (int)xi)) : 255 - (int)(255 * Math.Abs(xi - (int)xi));
                        bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - c, 255 - c, 255 - c));
                        bmp.SetPixel((int)xi + step, (int)yi, Color.FromArgb(c, c, c));
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
                    if (!out_of_PB((int)xi, (int)yi + step) && !out_of_PB((int)xi, (int)yi))
                    {
                        int c = gradient < 0 ? (int)(255 * Math.Abs(yi - (int)yi)) : 255 - (int)(255 * Math.Abs(yi - (int)yi));
                        bmp.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - c, 255 - c, 255 - c));
                        bmp.SetPixel((int)xi, (int)yi + step, Color.FromArgb(c, c, c));
                    }
                    yi += gradient;
                }
            }
        }
        //--------------------7-------------------
        //left or right point
        private void button7_Click(object sender, EventArgs e)
        {
            bool is_edge_and_point = 2 == checkedListBox1.CheckedIndices.Count;
            is_edge_and_point = is_edge_and_point && polygons[checkedListBox1.CheckedIndices[0]].Count  + polygons[checkedListBox1.CheckedIndices[1]].Count == 3;
            if (is_edge_and_point)
            {
                var line = -1;
                int point = -1;

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.Items[i].ToString().Contains("line") && checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        line = i;

                    if (checkedListBox1.Items[i].ToString().Contains("point") && checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        point = i;

                }
                double dist = Distance(polygons[line], polygons[point]);
                if (dist > 0)
                    MessageBox.Show("Точка справа от ребра");
                else if (dist == 0)
                    MessageBox.Show("Точка на ребре");
                else MessageBox.Show("Точка слева от ребра");
                //MessageBox.Show("Расстояние от отрезка до точки: " + Distance(polygons[line],polygons[point]));

            }
            else
                MessageBox.Show("Выберите точку и ребро");
                
        }



        /* Определяет расстояние до точки. Справа от прямой расстояние положительное, 
        * слева - отрицательное. */
        public float Distance(List<Point> edge, List<Point> point)
        {
            var dx = edge[1].X - edge[0].X;
            var dy = edge[1].Y - edge[0].Y;
            var n = (float)Math.Sqrt(dy * dy + dx * dx);
            return (dx * point[0].Y - dy * point[0].X - dx * edge[0].Y + dy * edge[0].X) / n;
        }

        //--------------------5-------------------
        //пересечения ребер
        // Уравнение прямой, проходящей через две заданные точки (x1,y1) и (x2,y2):
        // (y1 - y2)x + (x2 - x1)y + (x1y2 - x2y1) = 0
        private void lineEquation(List<Point> edge, out float A, out float B, out float C)
        {
            A = edge[0].Y - edge[1].Y;
            B = edge[1].X -edge[0].X;
            C = edge[0].X * edge[1].Y - edge[1].X * edge[0].Y;
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
            return (new Point((int) x, (int)y));

        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool is_edge_edge = 2 == checkedListBox1.CheckedIndices.Count;
            is_edge_edge = is_edge_edge && polygons[checkedListBox1.CheckedIndices[0]].Count == 2;
            is_edge_edge = is_edge_edge && polygons[checkedListBox1.CheckedIndices[1]].Count == 2;
            if (is_edge_edge)
            {
                var edge1 = polygons[checkedListBox1.CheckedIndices[0]];
                var edge2 = polygons[checkedListBox1.CheckedIndices[1]];

                float A1, B1, C1, A2, B2, C2, xRes, yRes;
                lineEquation(edge1, out A1, out B1, out C1);
                lineEquation(edge2, out A2, out B2, out C2);
                var p = findPoint(A1, A2, B1, B2, C1, C2, out xRes, out yRes);

                if ((p.X >= Math.Min(edge1[0].X, edge1[1].X)) && (p.X <= Math.Max(edge1[0].X, edge1[1].X)) &&
                    (p.X >= Math.Min(edge2[0].X, edge2[1].X)) && (p.X <= Math.Max(edge2[0].X, edge2[1].X)) &&
                    (p.Y >= Math.Min(edge1[0].Y, edge1[1].Y)) && (p.Y <= Math.Max(edge1[0].Y, edge1[1].Y)) &&
                    (p.Y >= Math.Min(edge2[0].Y, edge2[1].Y)) && (p.Y <= Math.Max(edge2[0].Y, edge2[1].Y)))
                {
                    drawFancy(p, Color.Red); 
                    pictureBox1.Refresh();
                }
                else
                    MessageBox.Show("Ребра не имеют общей точки");

            }
            else { MessageBox.Show("Выберите 2 ребра"); }
        }

        //check if point is out of the Picture Box
        bool out_of_PB(int x, int y)
        {
            return x <= 0 || y <= 0 || x >= pictureBox1.Width || y >= pictureBox1.Height;
        }

        //-------------------------6---------------point in polygon(невыпуклый)
        private void button9_Click(object sender, EventArgs e)
        {
            bool is_point_poly = 2 == checkedListBox1.CheckedIndices.Count;
            is_point_poly = is_point_poly && ((polygons[checkedListBox1.CheckedIndices[0]].Count>= 3 && polygons[checkedListBox1.CheckedIndices[1]].Count==1)
                ||(polygons[checkedListBox1.CheckedIndices[1]].Count >= 3 && polygons[checkedListBox1.CheckedIndices[0]].Count == 1));
            if (is_point_poly)
            {
                var poly = polygons[checkedListBox1.CheckedIndices[1]];
                var point = polygons[checkedListBox1.CheckedIndices[0]];
                if (poly.Count == 1)
                {
                    poly = point;
                    point =polygons[checkedListBox1.CheckedIndices[1]];
                }
                checkPoint_in_Poly(point[0],poly);
            }
            else 
                MessageBox.Show("Выберите 1 полигон и 1 точку");

        }


        private double degreeBetweenEdges(Point e1_1,Point e1_2, Point e2_1, Point e2_2)
        {
            var dx_e1 = e1_2.X - e1_1.X;
            var dy_e1 = e1_2.Y - e1_1.Y;
            var dx_e2 = e2_2.X - e2_1.X;
            var dy_e2 = e2_2.Y - e2_1.Y;

            var len_e1 = Math.Sqrt(dx_e1 * dx_e1 + dy_e1 * dy_e1);
            var len_e2 = Math.Sqrt(dx_e2 * dx_e2 + dy_e2 * dy_e2);
            var scalarProd = dx_e1 * dx_e2 + dy_e1 * dy_e2;

            return Math.Acos(scalarProd / (len_e1 * len_e2)) * Math.Sign(dx_e1 * dy_e2 - dy_e1 * dx_e2);
        }


        private void checkPoint_in_Poly(Point p, List<Point> poly)
        {
            double sumDegree = 0;

            for (int i = 0; i < poly.Count - 1; ++i)
                sumDegree += degreeBetweenEdges(p, poly[i],p, poly[i + 1]);

            sumDegree += degreeBetweenEdges(p, poly[poly.Count - 1],p, poly[0]);

            var eps = 0.0001;
            if (Math.Abs(sumDegree - 0) < eps)
                MessageBox.Show("Точка лежит снаружи многоугольника");
            else
                MessageBox.Show("Точка лежит внутри многоугольника");
        }

    }
}
