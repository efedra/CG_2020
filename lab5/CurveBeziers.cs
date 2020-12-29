using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace lab6
{
    class CurveBeziers
    {
        public LinkedList<PointF> Points { get; private set; }
        public bool closed;

        public CurveBeziers(){
            Points = new LinkedList<PointF>();
            closed = false;
        }
        
        public void AddPoint(PointF p, ref Bitmap bmp)
        {
            if (Points.Count == 0)
            {
                Points.AddLast(p);
                OldFunctions.drawFancy(new Point((int)p.X, (int)p.Y), Color.Black, ref bmp);
            }
            else if (Points.Count % 3 != 0)
            {
                PointF p1 = Points.Last.Value;
                PointF p2;
                PointF p3;
                float dx = p.X - p1.X;
                float dy = p.Y - p1.Y;

                if (Points.Count != 1)
                {
                    PointF p0 = Points.Last.Previous.Value;
                    p2 = new PointF(p1.X + p1.X - p0.X, p1.Y + p1.Y - p0.Y);
                    p3 = new PointF(p2.X + dx / 2, p2.Y + dy / 2);
                }
                else
                {
                    p2 = new PointF(p1.X + dx / 3 + dy / 3, p1.Y + dy / 3 - dx / 3);
                    p3 = new PointF(p.X - dx / 3 + dy / 3, p.Y - dy / 3 - dx / 3);
                }
                Points.AddLast(p2);
                Points.AddLast(p3);
                Points.AddLast(p);
            }
            //when we first close the spline
            if (closed && Points.Count % 3 == 1)
            {
                PointF p0 = Points.Last.Previous.Value;
                PointF p1 = Points.Last.Value;
                PointF p2 = new PointF(p1.X + p1.X - p0.X, p1.Y + p1.Y - p0.Y);
                PointF p4 = Points.First.Value;
                PointF p5 = Points.First.Next.Value;
                PointF p3 = new PointF(p4.X + p4.X - p5.X, p4.Y + p4.Y - p5.Y);

                Points.AddLast(p2);
                Points.AddLast(p3);
            }
            //when the spline is closed and we add another point
            else if (closed && Points.Count % 3 == 0)
            {
                LinkedListNode<PointF> closest_point1_nd = Points.Last.Previous.Previous;
                LinkedListNode<PointF> closest_point2_nd = Points.First;
                double min_dist = OldFunctions.distance( closest_point1_nd.Value, p) +
                     OldFunctions.distance(closest_point2_nd.Value, p);
                LinkedListNode<PointF> tmp_pnt_nd = Points.First;
                while (tmp_pnt_nd!= Points.Last.Previous.Previous)
                {
                    double cur_dist = OldFunctions.distance(tmp_pnt_nd.Value, p) + OldFunctions.distance(tmp_pnt_nd.Next.Next.Next.Value, p);
                    if (cur_dist < min_dist)
                    {
                        min_dist = cur_dist;
                        closest_point1_nd = tmp_pnt_nd;
                        closest_point2_nd = tmp_pnt_nd.Next.Next.Next;
                    }
                    tmp_pnt_nd = tmp_pnt_nd.Next.Next.Next;
                }
                PointF p1 = closest_point1_nd.Value;
                PointF p2 = closest_point1_nd.Next.Value;
                float dx = p.X - p1.X;
                float dy = p.Y - p1.Y;
                PointF p3 = new PointF(p2.X + dx / 2, p2.Y + dy / 2);
                PointF p5 = new PointF(p.X + p.X - p3.X, p.Y + p.Y - p3.Y);
                Points.AddAfter(closest_point1_nd.Next, p3);
                Points.AddAfter(closest_point1_nd.Next.Next, p);
                Points.AddAfter(closest_point1_nd.Next.Next.Next, p5);
            }
            Draw(ref bmp);
        }

        public void DeletePoint(PointF p, ref Bitmap bmp)
        {
            int i = 0;
            LinkedListNode<PointF> temp = Points.First;
            while (temp != Points.Last)
            {
                if (temp.Value == p)
                    break;
                i++;
                temp = temp.Next;
            }
            if (i % 3 != 0)
                return;

            LinkedListNode<PointF> next = Points.Find(p).Next;
            LinkedListNode<PointF> prev;
            if (Points.Find(p) != Points.First)
                prev = Points.Find(p).Previous;
            else
                prev = Points.Last;
            Points.Remove(prev.Value);
            Points.Remove(p);
            Points.Remove(next.Value);
            if (i == 0)
            {
                Points.AddLast(Points.First.Value);
                Points.RemoveFirst();
            }
            Draw(ref bmp);
        }

        public void MovePoint(PointF old_point, PointF new_point)
        {
            int i = 0;
            LinkedListNode<PointF> temp = Points.First;
            while (temp != Points.Last)
            {
                if (temp.Value == old_point)
                    break;
                i++;
                temp = temp.Next;
            }
            float dx = new_point.X - old_point.X;
            float dy = new_point.Y - old_point.Y;

            //actual point
            if (i % 3 == 0)
            {
                if (i == 0)
                {
                    PointF temp_prev = Points.Last.Value;
                    PointF temp_next = Points.First.Next.Value;

                    temp_prev.X += dx; temp_prev.Y += dy;
                    temp_next.X += dx; temp_next.Y += dy;

                    Points.AddAfter(temp, new_point);
                    Points.Remove(temp);
                    Points.RemoveLast();
                    Points.AddLast(temp_prev);
                    Points.Remove(Points.First.Next);
                    Points.AddAfter(Points.First, temp_next);
                } 
                else
                {
                    PointF temp_prev = temp.Previous.Value;
                    PointF temp_next = temp.Next.Value;

                    temp_prev.X += dx; temp_prev.Y += dy;
                    temp_next.X += dx; temp_next.Y += dy;

                    Points.AddAfter(temp, temp_next);
                    Points.Remove(temp.Next.Next);
                    Points.AddBefore(temp, temp_prev);
                    Points.Remove(temp.Previous.Previous);
                    Points.AddAfter(temp, new_point);
                    Points.Remove(temp);
                }
            }
            //first extra point (prev is actual point)
            else if (i % 3 == 1)
            {
                if (i == 1)
                {
                    //we dont need to also move anything
                    if (!closed)
                    {
                        Points.AddAfter(temp, new_point);
                        Points.Remove(temp);
                    }
                    else
                    {
                        PointF tmp = Points.Last.Value;
                        tmp.X -= dx; tmp.Y -= dy;
                        Points.RemoveLast();
                        Points.AddLast(tmp);
                        Points.AddAfter(temp, new_point);
                        Points.Remove(temp);
                    }
                }
                else
                {
                    PointF tmp = temp.Previous.Previous.Value;
                    tmp.X -= dx; tmp.Y -= dy;

                    Points.AddBefore(temp.Previous, tmp);
                    Points.Remove(temp.Previous.Previous.Previous);
                    Points.AddAfter(temp, new_point);
                    Points.Remove(temp);
                }
            }
            else
            {
                //we dont need to also move anything
                if (i == Points.Count - 1)
                {
                    //we dont need to also move anything
                    if (!closed)
                    {
                        Points.AddAfter(temp, new_point);
                        Points.Remove(temp);
                    }
                    else
                    {
                        PointF tmp = Points.First.Next.Value;
                        tmp.X -= dx; tmp.Y -= dy;
                        Points.Remove(Points.First.Next);
                        Points.AddAfter(Points.First, tmp);
                        Points.AddAfter(temp, new_point);
                        Points.Remove(temp);
                    }
                }
                else
                {
                    PointF tmp = temp.Next.Next.Value;
                    tmp.X -= dx; tmp.Y -= dy;

                    Points.AddAfter(temp.Next, tmp);
                    Points.Remove(temp.Next.Next.Next);
                    Points.AddAfter(temp, new_point);
                    Points.Remove(temp);
                }
            }
        }

        //draws 
        public void Draw(ref Bitmap bmp)
        {
            int width = bmp.Width; int height = bmp.Height;
            bmp.Dispose();
            bmp = new Bitmap(width, height); 

            LinkedListNode<PointF> temp = Points.First;
            while (temp != Points.Last)
            {
                DrawBezierSegment(temp.Value, temp.Next.Value, temp.Next.Next.Value, temp.Next.Next.Next.Value, ref bmp);
                temp = temp.Next.Next.Next;
                if (temp == Points.Last.Previous.Previous)
                    break;
            }
            if (closed)
                DrawBezierSegment(temp.Value, temp.Next.Value, temp.Next.Next.Value, Points.First.Value, ref bmp);

        }

        public void DrawBezierSegment(PointF p1, PointF p2, PointF p3, PointF p4, ref Bitmap bmp)
        {
            OldFunctions.Wu((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, Color.Aqua, ref bmp);
            OldFunctions.Wu((int)p3.X, (int)p3.Y, (int)p4.X, (int)p4.Y, Color.Violet, ref bmp);
            for (float t = 0; t < 1; t += 0.002f)
            {
                PointF q0 = Bezier_thing(p1, p2, t);
                PointF q1 = Bezier_thing(p2, p3, t);
                PointF q2 = Bezier_thing(p3, p4, t);
                PointF r0 = Bezier_thing(q0, q1, t);
                PointF r1 = Bezier_thing(q1, q2, t);
                PointF b = Bezier_thing(r0, r1, t);

                if (!OldFunctions.out_of_PB((int)b.X, (int)b.Y, ref bmp))
                    bmp.SetPixel((int)b.X, (int)b.Y, Color.Black);
                OldFunctions.drawFancy(new Point((int)p2.X, (int)p2.Y), Color.Aqua, ref bmp);
                OldFunctions.drawFancy(new Point((int)p3.X, (int)p3.Y), Color.Violet, ref bmp);
                OldFunctions.drawFancy(new Point((int)p4.X, (int)p4.Y), Color.Black, ref bmp);
            }
        }

        public PointF Bezier_thing(PointF p1, PointF p2, float t)
        {
            float x = p1.X * (1 - t) + p2.X * t;
            float y = p1.Y * (1  -t) + p2.Y * t;
            return new PointF(x, y);
        }

        //есть еще штука с матрицами можно взять из OldFunctions перемножение
        private PointF B_res(float t,PointF p0, PointF p1, PointF p2, PointF p3)
        {
            var sum_l = new List<PointF>();
            float[] deg_t = {1,t, t*t, t*t*t };
            float[] deg_1t = { 1, 1-t, (1-t)*(1-t)*(1-t) };

            sum_l.Add(mult_p(p0, deg_1t[3]));
            sum_l.Add(mult_p(mult_p(p1, deg_1t[2]),3*deg_t[1]));
            sum_l.Add(mult_p(mult_p(p2, deg_1t[1]), 3 * deg_t[2]));
            sum_l.Add(mult_p(p3, deg_t[3]));

            return sum_points(sum_l);
        }

        private PointF mult_p(PointF p, float k)
        {
            return new PointF(p.X * k, p.Y * k);
        }

        private PointF sum_points(List<PointF> sum_l)
        {
            PointF sum = new PointF(0, 0);
            foreach (var p in sum_l)
                sum = new PointF(sum.X + p.X, sum.Y + p.Y);
            return sum;
        }

        public void Clear()
        {
            Points.Clear();
        }
    }
}
