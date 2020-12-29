using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace lab6
{
    class Lsystem
    {
        String atom = "";
        int angle = 0;
        int first_direction = 0;
        Dictionary<Char, String> rules = new Dictionary<Char, String>();
        int width = 0, height = 0;
        int n = 0;

        LinkedList<PointF> points = new LinkedList<PointF>();


        int rotate_angle = 0;//current 
        String res = "";
        //bounds points
        private PointF left_point;
        private PointF right_point;
        private PointF up_point;
        private PointF down_point;


        public Lsystem(string fname, int n, int w, int h)
        {
            if (rules.Count == 0)
                ReadFile(fname);
            this.n = n;
            iterate(n);
            width = w - 1;
            height = h - 1;
            left_point = new PointF(float.MaxValue, 0);
            right_point = new PointF(float.MinValue, 0);
            up_point = new PointF(0, float.MaxValue);
            down_point = new PointF(0, float.MinValue);
        }

        private void ReadFile(string fname)
        {
            using (StreamReader sr = new StreamReader(fname, System.Text.Encoding.Default))
            {
                string line = sr.ReadLine();
                string[] arr = line.Split();
                (atom, angle, first_direction) = (arr[0], int.Parse(arr[1]), int.Parse(arr[2]));
                while ((line = sr.ReadLine()) != null)
                    rules.Add(line[0], line.Substring(2));
            }
        }

        private void iterate(int n)
        {
            res = atom;
            var keys = rules.Keys.ToArray();
            for (int i = 0; i < n; i++)
            {
                int k = 0;
                k = res.IndexOfAny(keys, k);
                while (k != -1)
                {
                    char c = res[k];
                    res = res.Remove(k, 1);
                    var val = rules[c];
                    res = res.Insert(k, val);
                    k += val.Length;
                    k = res.IndexOfAny(keys, k);
                }
            }
        }

        private void forward1(ref LinkedListNode<PointF> current, ref List<Edge> edges)
        {
            LinkedListNode<PointF> last = current;
            double angle_ = Math.PI * rotate_angle / 180.0;
            PointF p = new PointF((float)Math.Cos(angle_) + last.Value.X, (float)Math.Sin(angle_) + last.Value.Y);
            CorrectBoundsPoints(p);
            points.AddLast(p);
            current = points.Last;
            edges.Add(new Edge(last, current));
        }

        private void forwardTree(ref LinkedListNode<PointF> current, int len, ref List<EdgeTree> edges, float thick, Color cl)
        {
            LinkedListNode<PointF> last = current;
            double angle_ = Math.PI * rotate_angle / 180.0;
            PointF p = new PointF((float)(Math.Cos(angle_) * len) + last.Value.X, (float)(Math.Sin(angle_) * len) + last.Value.Y);
            CorrectBoundsPoints(p);
            points.AddLast(p);
            current = points.Last;
            edges.Add(new EdgeTree(last, current, cl, thick));
        }

        class Edge
        {
            public LinkedListNode<PointF> P1 { get; set; }
            public LinkedListNode<PointF> P2 { get; set; }
            public Edge(LinkedListNode<PointF> ln1, LinkedListNode<PointF> ln2)
            {
                P1 = ln1;
                P2 = ln2;
            }
        }

        class EdgeTree
        {
            public LinkedListNode<PointF> P1 { get; set; }
            public LinkedListNode<PointF> P2 { get; set; }
            public Color Col { get; set; }
            public float Thickness { get; set; }
            public EdgeTree(LinkedListNode<PointF> ln1, LinkedListNode<PointF> ln2, Color c, float thick)
            {
                P1 = ln1;
                P2 = ln2;
                Col = c;
                Thickness = thick;
            }
        }

        struct StatePoint
        {
            public LinkedListNode<PointF> Point { get; set; }
            public int Len { get; set; }
            public int Angle { get; set; }
            public Color Col { get; set; }
            public float Thickness { get; set; }
            public StatePoint(LinkedListNode<PointF> point, int len, int angle, Color color, float thickness)
            {
                Point = point;
                Len = len;
                Angle = angle;
                Col = color;
                Thickness = thickness;
            }
        }
        public void Draw(ref Bitmap bmp, bool random)
        {
            var of = new OldFunctions(width, height);
            PointF p = new PointF(width / 2, height / 2);
            CorrectBoundsPoints(p);
            rotate_angle = first_direction;
            Stack<KeyValuePair<LinkedListNode<PointF>, int>> stack_states = new Stack<KeyValuePair<LinkedListNode<PointF>, int>>();
            points.AddLast(p);
            LinkedListNode<PointF> current = points.First;
            stack_states.Push(new KeyValuePair<LinkedListNode<PointF>, int>(current, rotate_angle));
            List<Edge> edges = new List<Edge>();

            Random r = new Random();
            double rand = random ? r.NextDouble() : 1;
            foreach (var c in res)
            {
                switch (c)
                {
                    case 'F': forward1(ref current, ref edges); break;
                    case '-': rotate_angle += (int)(angle * rand); break;
                    case '+': rotate_angle -= (int)(angle * rand); break;
                    case '[': stack_states.Push(new KeyValuePair<LinkedListNode<PointF>, int>(current, rotate_angle)); break;
                    case ']': var p_angle = stack_states.Pop(); (current, rotate_angle) = (p_angle.Key, p_angle.Value); break;
                    default: break;
                }
                rand = random ? r.NextDouble() : 1;
            }
            //scale points
            float dx = right_point.X - left_point.X;
            float dy = down_point.Y - up_point.Y;
            PointF center = new PointF(left_point.X + dx / 2, up_point.Y + dy / 2);

            float min_resize = Math.Min(width / dx, height / dy);
            current = points.First;
            while (current != null)
            {
                current.Value = of.scale_shift_point(current.Value.X, current.Value.Y, width / 2 - center.X, height / 2 - center.Y, min_resize, min_resize, center);
                current = current.Next;
            }
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (var e in edges)
                    g.DrawLine(new Pen(Color.Black, 1), e.P1.Value, e.P2.Value);
            }
        }

        public void DrawTree(ref Bitmap bmp)
        {
            var of = new OldFunctions(width, height);
            PointF p = new PointF(width / 2, height / 2);
            int len = n + 3;
            CorrectBoundsPoints(p);
            points.AddLast(p);
            LinkedListNode<PointF> current = points.First;
            rotate_angle = first_direction;
            Stack<StatePoint> state_points = new Stack<StatePoint>();
            float thickness = 16;
            Color cl = Color.FromArgb(23, 16, 9);
            List<EdgeTree> edges = new List<EdgeTree>();

            Random r = new Random();
            foreach (var c in res)
                switch (c)
                {
                    case 'F': forwardTree(ref current, len, ref edges, thickness, cl); break;
                    case 'X': forwardTree(ref current, len, ref edges, thickness, cl); break;
                    case '-': rotate_angle += (int)(angle * r.NextDouble()); break;
                    case '+': rotate_angle -= (int)(angle * r.NextDouble()); break;
                    case '[': state_points.Push(new StatePoint(current, len, rotate_angle, cl, thickness)); break;
                    case ']':
                        var state_ = state_points.Pop();
                        rotate_angle = state_.Angle;
                        current = state_.Point;
                        len = state_.Len;
                        cl = state_.Col;
                        thickness = state_.Thickness; break;
                    case '@':
                        len--;
                        thickness -= 2;
                        cl = Color.FromArgb(cl.R - 1 > 0 ? cl.R - 1 : 0, cl.G + 5, cl.B - 1 > 0 ? cl.B - 1 : 0);
                        break;
                    default: break;

                }
            //scale points
            float dx = right_point.X - left_point.X;
            float dy = down_point.Y - up_point.Y;
            PointF center = new PointF(left_point.X + dx / 2, up_point.Y + dy / 2);

            float min_resize = Math.Min(width / dx, height / dy);
            current = points.First;
            while (current != null)
            {
                current.Value = of.scale_shift_point(current.Value.X, current.Value.Y, width / 2 - center.X, height / 2 - center.Y, min_resize, min_resize, center);
                current = current.Next;
            }
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (var e in edges)
                    g.DrawLine(new Pen(e.Col, e.Thickness), e.P1.Value, e.P2.Value);
            }
        }
        private void CorrectBoundsPoints(PointF p)
        {
            if (p.X < left_point.X)
                left_point = p;
            if (p.X > right_point.X)
                right_point = p;
            if (p.Y < up_point.Y)
                up_point = p;
            if (p.Y > down_point.Y)
                down_point = p;
        }
    }
}
