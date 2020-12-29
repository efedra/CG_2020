using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //initializin second pictureBox
            bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g = Graphics.FromImage(bmp2);
            pictureBox2.Image = bmp2;
        }

        //.....................................1.....................................

        Point lastPoint = Point.Empty;//Point.Empty represents null for a Point object
        bool isMouseDown = new Boolean();//this is used to evaluate whether our mousebutton is down or not
        //bool to swap between drawing and starting filling
        bool SetPoint = false;
        //coordinates of filling point
        int pointX = 0, pointY = 0;
        Color point_color = Color.Black;
        Bitmap bmp1;
        Bitmap bmp_img;

        //drawing
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (SetPoint)
            {
                pointX = e.X;
                pointY = e.Y;
            }
            lastPoint = e.Location;//we assign the lastPoint to the current mouse position: e.Location ('e' is from the MouseEventArgs passed into the MouseDown event)
            isMouseDown = !SetPoint;//we set to true because our mouse button is down (clicked)
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null)//if no available bitmap exists on the picturebox to draw on
            {
                //create a new bitmap
                bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = bmp1; //assign the picturebox.Image property to the bitmap created
            }
            if (SetPoint)
            {
                return;
            }
            if (isMouseDown == true)//check to see if the mouse button is down
            {
                if (lastPoint != null)//if our last point is not null, which in this case we have assigned above
                {
                    using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                    {//we need to create a Graphics object to draw on the picture box, its our main tool
                        //when making a Pen object, you can just give it color only or give it color and pen size
                        g.DrawLine(new Pen(Color.Black, 3), lastPoint, e.Location);
                    }
                    pictureBox1.Invalidate();//refreshes the picturebox
                    lastPoint = e.Location;//keep assigning the lastPoint to the current mouse position
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (SetPoint)
            {
                return;
            }
            isMouseDown = false;
            lastPoint = Point.Empty;
        }

        Color old_color1;
        int width1;
        int height1;

        //1
        //Fill with color
        private void button1_Click(object sender, EventArgs e)
        {
            //Stack<Point> st = new Stack<Point>();
            //st.Push(new Point(pointX, pointY));
            old_color1 = bmp1.GetPixel(pointX, pointY);
            (width1, height1) = (pictureBox1.Width, pictureBox1.Height);

            recurs(new Point(pointX, pointY));

            //while (st.Count != 0)
            //{
            //    Point cur_point = st.Pop();
            //    if (bmp1.GetPixel(cur_point.X, cur_point.Y) == old_color)
            //    {
            //        int left_x = cur_point.X;
            //        int right_x = cur_point.X;
            //        //find left border
            //        while (left_x > 0 && (bmp1.GetPixel(left_x, cur_point.Y) == old_color))
            //            left_x--;
            //        left_x++;
            //        //find right border
            //        while (right_x < width && (bmp1.GetPixel(right_x, cur_point.Y) == old_color))
            //            right_x++;
            //        right_x--;

            //        for (int i = left_x; i < right_x + 1; i++)
            //            bmp1.SetPixel(i, cur_point.Y, point_color);

            //        if (cur_point.Y + 1 < height)
            //            for (int i = left_x; i < right_x + 1; i++)
            //                st.Push(new Point(i, cur_point.Y + 1));
            //        if (cur_point.Y - 1 > 0)
            //            for (int i = left_x; i < right_x + 1; i++)
            //                st.Push(new Point(i, cur_point.Y - 1));
            //    }
            //}
            pictureBox1.Image = bmp1;
        }

        private void recurs(Point cur_point)
        {
            //Point cur_point = st.Pop();
            if (bmp1.GetPixel(cur_point.X, cur_point.Y) == old_color1)
            {
                int left_x = cur_point.X;
                int right_x = cur_point.X;
                //find left border
                while (left_x > 0 && (bmp1.GetPixel(left_x, cur_point.Y) == old_color1))
                    left_x--;
                left_x++;
                //find right border
                while (right_x < width1 && (bmp1.GetPixel(right_x, cur_point.Y) == old_color1))
                    right_x++;
                right_x--;

                for (int i = left_x; i < right_x + 1; i++)
                    bmp1.SetPixel(i, cur_point.Y, point_color);

                if (cur_point.Y + 1 < height1)
                    for (int i = left_x; i < right_x + 1; i++)
                        recurs(new Point(i, cur_point.Y + 1));
                if (cur_point.Y - 1 > 0)
                    for (int i = left_x; i < right_x + 1; i++)
                        recurs(new Point(i, cur_point.Y - 1));
            }
        }

        //1
        //Choose color
        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.Color = point_color;//initial color

            if (MyDialog.ShowDialog() == DialogResult.OK)
                point_color = MyDialog.Color;
        }

        //1
        //Fill with Image
        private void button2_Click(object sender, EventArgs e)
        {
            Stack<Point> st = new Stack<Point>();
            st.Push(new Point(pointX, pointY));
            Color old_color = bmp1.GetPixel(pointX, pointY);
            (int width, int height) = (pictureBox1.Width, pictureBox1.Height);
            (int img_width, int img_height) = (bmp_img.Width, bmp_img.Height);

            while (st.Count != 0)
            {
                Point cur_point = st.Pop();
                if (bmp1.GetPixel(cur_point.X, cur_point.Y) == old_color)
                {
                    int left_x = cur_point.X;
                    int right_x = cur_point.X;
                    //find left border
                    while (left_x > 0 && (bmp1.GetPixel(left_x, cur_point.Y) == old_color))
                        left_x--;
                    left_x++;
                    //find right border
                    while (right_x < width && (bmp1.GetPixel(right_x, cur_point.Y) == old_color))
                        right_x++;
                    right_x--;

                    for (int i = left_x; i < right_x + 1; i++)
                    {
                        int diff_x = i - pointX + img_width * 10;
                        int diff_y = cur_point.Y - pointY + img_width * 10;
                        bmp1.SetPixel(i, cur_point.Y, bmp_img.GetPixel(diff_x % img_width, diff_y % img_height));
                    }


                    if (cur_point.Y + 1 < height)
                        for (int i = left_x; i < right_x + 1; i++)
                            st.Push(new Point(i, cur_point.Y + 1));
                    if (cur_point.Y - 1 > 0)
                        for (int i = left_x; i < right_x + 1; i++)
                            st.Push(new Point(i, cur_point.Y - 1));
                }
            }
            pictureBox1.Image = bmp1;
        }

        //1
        //Choose Image
        private void button4_Click(object sender, EventArgs e)
        {
            string imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG files|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    bmp_img = new Bitmap(imageLocation);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //1
        //clear
        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
            }
        }

        //draw figure
        private void button8_Click(object sender, EventArgs e)
        {
            SetPoint = false;
        }
        //select point
        private void button9_Click(object sender, EventArgs e)
        {
            SetPoint = true;
        }


        //.....................................2.....................................

        Graphics g;
        int sizePen = 1;
        Point mouse;

        Bitmap bmp2;

        //рисуем на второй картинке
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = e.Location;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                g.DrawLine(new Pen(Color.Black, sizePen), mouse, e.Location);
                mouse = e.Location;
                pictureBox2.Refresh();
            }
        }

        bool find = false;
        //при нажатии Go начинаем алгоритм
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (find)
            {
                GetBorderPoints(bmp2, Color.Blue);
                find = false;
            }
            pictureBox2.Refresh();
        }

        //2
        //(Go!)
        private void button6_Click(object sender, EventArgs e)
        {
            find = true;
        }

        //2
        //Clear
        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image = null;
            }
            find = false;
            bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            g = Graphics.FromImage(bmp2);
            pictureBox2.Image = bmp2;
        }

        //Функция поиска начальной точки
        private Point FindStartPoint(Bitmap sourceImage)
        {
            int x = mouse.X;
            int y = mouse.Y;
            int borderX = -1, borderY = -1;
            Color back_color = bmp2.GetPixel(mouse.X, mouse.Y);
            Color cur_color = back_color;

                
            while (cur_color == back_color && y > 0)
            {
                while (cur_color == back_color && x + 1 < bmp2.Width)
                    cur_color = bmp2.GetPixel(++x, y); //сначала плюс плюс потом получить
                if (borderX <= x)
                {
                    borderY = y;
                    borderX = x;
                }
                y--;
                x = mouse.X;
                cur_color = bmp2.GetPixel(x, y);
            }
            return new Point(borderX, borderY);
        }

        //Функция формирования списка граничных точек
        private List<Point> GetBorderPoints(Bitmap bmp_img, Color c)
        {
            List<Point> border = new List<Point>();
            Point cur = FindStartPoint(bmp_img);
            border.Add(cur);
            Point start = cur;
            Point next = cur;
            int pred_Dir = 6;
            int cur_dir = 6;
            Color borderColor = bmp_img.GetPixel(cur.X, cur.Y);

            //Будем идти против часовой стрелке и ходить изнутри области
            int dir = 8;
            do
            {
                dir += dir - 1 < 0 ? 7 : -2;
                int t = dir;
                do
                {
                    next = cur;
                    switch (dir)
                    {
                        case 0: next.X++; pred_Dir = 0; break;
                        case 1: next.X++; next.Y--; pred_Dir = 1; break;
                        case 2: next.Y--; pred_Dir = 2; break;
                        case 3: next.X--; next.Y--; pred_Dir = 3; break;
                        case 4: next.X--; pred_Dir = 4; break;
                        case 5: next.X--; next.Y++; pred_Dir = 5; break;
                        case 6: next.Y++; pred_Dir = 6; break;
                        case 7: next.X++; next.Y++; pred_Dir = 7; break;
                    }
                    //Если не нашли - останавливаемся
                    if (next == start)
                        break;
                    if (bmp_img.GetPixel(next.X, next.Y) == borderColor)
                    {
                        //Кладем в список
                        border.Add(next);
                        if (cur_dir == 2 && pred_Dir == 6 || cur_dir == 6 && pred_Dir == 2)
                            border.Add(next);
                        else if (cur_dir == 1 && pred_Dir == 7 || cur_dir == 5 && pred_Dir == 3)
                            border.Add(next);
                        else if (cur_dir == 4 && pred_Dir == 0 || cur_dir == 4 && pred_Dir == 4)
                            border.Add(next);
                        cur = next;
                        cur_dir = pred_Dir;
                        break;
                    }
                    dir = (dir + 1) % 8;
                } while (dir != t);
            } while (next != start);

            if (c != Color.Empty)
            {
                foreach (var x in border)
                    bmp_img.SetPixel(x.X, x.Y, c);
                return null;
            }
            else
            {
                border.Sort(ComparePoints);
                return border;
            }
        }

        //сравниваем точки
        private static int ComparePoints(Point a, Point b)
        {
            if (a.Y == b.Y)
                return a.X.CompareTo(b.X);
            else if (a.Y < b.Y)
                return -1;
            else return 1;
        }

        //.....................................3.....................................

        //line coordinates
        int xBegin, yBegin, xEnd, yEnd;

        Bitmap bmp3;
        //bool to swap between algorithms
        bool brsnhm = true;

        //VU
        private void button10_Click(object sender, EventArgs e)
        {
            if (brsnhm)
            {
                button10.Text = "Wu";
                brsnhm = false;
            }
            else
            {
                button10.Text = "Bresenham";
                brsnhm = true;
            }
        }

        bool second = false;
        //setting the coordinates
        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            bmp3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            pictureBox3.Image = bmp3;

            if (second)
            {
                xEnd = e.X;
                yEnd = e.Y;
                bmp3.SetPixel(xEnd, yEnd, Color.Black);
                second = false;
                
                //start if the second point is set
                if (brsnhm)
                {
                    Bresenham(xBegin, yBegin, xEnd, yEnd);
                }
                else
                {
                    Wu(xBegin, yBegin, xEnd, yEnd);
                }
            }
            else
            {
                xBegin = e.X;
                yBegin = e.Y;
                bmp3.SetPixel(xBegin, yBegin, Color.Black);
                second = true;
            }
        }

        //3
        void Bresenham(int x1, int y1, int x2, int y2)
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
            int xi = x1;
            int yi = y1;
            int step = 1;
            int di = 2 * dy - dx;
            //1 & 4 octants
            if (dx == 0 || Math.Abs(dy / (double)dx) > 1)
            {
                //4 octant
                if (dy / (double)dx < 0)
                {
                    xi = x2;
                    step = -1;
                    dy = -dy;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }
                for (yi = y1; yi <= y2; yi++)
                {
                    bmp3.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        xi += step;
                        di += 2 * (dx - dy);
                    }
                    else
                    {
                        di += 2 * dx;
                    }
                }
            }
            else
            {
                if (dy / (double)dx < 0)
                {
                    step = -1;
                    dy = -dy;
                }
                for (xi = x1; xi <= x2; xi++)
                {
                    bmp3.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        yi += step;
                        di += 2 * (dy - dx);
                    }
                    else
                    {
                        di += 2 * dy;
                    }
                }
            }
        }

        //3
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
            if (Math.Abs(gradient) > 1)
            {
                gradient = 1 / gradient;
                //4 octant
                if (gradient < 0)
                {
                    xi = x2;
                    step = -1;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }

                for (yi = y1; yi <= y2; yi += 1)
                {
                    int c = gradient < 0 ? (int)(255 * (xi - (int)xi)) : 255 - (int)(255 * (xi - (int)xi));
                    bmp3.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - c, 255 - c, 255 - c));
                    bmp3.SetPixel((int)xi + step, (int)yi, Color.FromArgb(c, c, c));
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
                for (xi = x1; xi <= x2; xi+=1)
                {
                    int c = gradient < 0 ? (int)(255 * (yi - (int)yi)) : 255 - (int)(255 * (yi - (int)yi));
                    bmp3.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - c, 255 - c, 255 - c));
                    bmp3.SetPixel((int)xi, (int)yi + step, Color.FromArgb(c, c, c));
                    yi += gradient;
                }
            }
        }

        //3
        //draw stars
        private void button11_Click(object sender, EventArgs e)
        {
            int wdth = pictureBox3.Width;
            int hght = pictureBox3.Height;
            bmp3 = new Bitmap(wdth, hght);
            pictureBox3.Image = bmp3;

            Bresenham((int)(wdth * 0.15), (int)(hght * 0.45), (int)(wdth * 0.25), (int)(hght * 0.05));
            Bresenham((int)(wdth * 0.25), (int)(hght * 0.05), (int)(wdth * 0.35), (int)(hght * 0.45));
            Bresenham((int)(wdth * 0.35), (int)(hght * 0.45), (int)(wdth * 0.05), (int)(hght * 0.2));
            Bresenham((int)(wdth * 0.05), (int)(hght * 0.2), (int)(wdth * 0.45), (int)(hght * 0.2));
            Bresenham((int)(wdth * 0.45), (int)(hght * 0.2), (int)(wdth * 0.15), (int)(hght * 0.45));

            Wu((int)(wdth * 0.65), (int)(hght * 0.45), (int)(wdth * 0.75), (int)(hght * 0.05));
            Wu((int)(wdth * 0.75), (int)(hght * 0.05), (int)(wdth * 0.85), (int)(hght * 0.45));
            Wu((int)(wdth * 0.85), (int)(hght * 0.45), (int)(wdth * 0.55), (int)(hght * 0.2));
            Wu((int)(wdth * 0.55), (int)(hght * 0.2), (int)(wdth * 0.95), (int)(hght * 0.2));
            Wu((int)(wdth * 0.95), (int)(hght * 0.2), (int)(wdth * 0.65), (int)(hght * 0.45));
        }
    }
}
