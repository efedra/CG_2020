using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace lab9
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        List<Mesh> meshes;
        int cur_mesh_ind;

        private Vertex camera;

        private string tex_file_name = "C:\\Users\\nica\\Documents\\studies\\CG\\lab9\\lab9\\bin\\Debug\\tex2.jpg";
        public static double y_angle = 0;
        public static double x_angle = 0;
        public static double dist = 400;

        public Form1()
        {
            InitializeComponent();
            camera = new Vertex(0, 0, dist, 1);
            meshes = new List<Mesh>();
            redraw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //import mesh
        private void import_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFileDialog1.Title = "Open Object File";
            openFileDialog1.Filter = "OBJ files|*.obj";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                Mesh cur_mesh = new Mesh();
                cur_mesh.FromFile(selectedFileName);
                meshes.Add(cur_mesh);
                cur_mesh_ind = meshes.Count - 1;
            }
            //cur_polyhedron.Move(100, 0, 0); //----------------------
            redraw();
        }

        //import texture
        private void button19_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            openFileDialog1.Title = "Open Image File";
            openFileDialog1.Filter = "JPEG files|*.jpg";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                tex_file_name = openFileDialog1.FileName;
            redraw();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            double angle_step = 0.15;
            if (e.KeyCode == Keys.W) x_angle += angle_step;
            if (e.KeyCode == Keys.A) y_angle += angle_step;
            if (e.KeyCode == Keys.S) x_angle -= angle_step;
            if (e.KeyCode == Keys.D) y_angle -= angle_step;

            double delta = 10;
            if (e.KeyCode == Keys.Q) dist -= delta;
            if (e.KeyCode == Keys.E) dist += delta;

            if (e.KeyCode == Keys.Space)
                if (cur_mesh_ind == meshes.Count - 1)
                    cur_mesh_ind = 0;
                else
                    cur_mesh_ind += 1;

            redraw();
        }

        bool lighting = false;
        bool texturing = false;

        public void redraw()
        {
            if (checkBox1.Checked)
            {
                FloatingHorizontON();
            }
            else
            {
                camera = new Vertex(0, 0, dist, 1);
                clear_bmp();
                if (lighting)
                    foreach (Mesh p in meshes)
                        show_lightened(p);
                else if (texturing)
                    foreach (Mesh p in meshes)
                        show_textured(p);
                else
                    foreach (Mesh p in meshes)
                        show_none(p);

                if (meshes.Count > 1)
                    show_none(meshes[cur_mesh_ind]);

                pictureBox1.Image = bmp;
            }

        }

        private void FloatingHorizontON()
        {
            var xStart = double.Parse(textBox4.Text);
            var xEnd = double.Parse(textBox5.Text);
            var xStep = double.Parse(textBox6.Text);
            var yStart = double.Parse(textBox7.Text);
            var yEnd = double.Parse(textBox8.Text);
            Bitmap bmp = FloatingHorizont.DrawHorizont(pictureBox1.Width, pictureBox1.Height, comboBox1.SelectedIndex, xStart, xEnd, xStep, yStart, yEnd);
            pictureBox1.Image = bmp;
            pictureBox1.Invalidate();
        }

        private void show_lightened(Mesh mesh)
        {
            Light l = new Light(new Vertex(0, 0, 300), 1);
            //l.position.perspective(dist, x_angle, -y_angle);
            List<Vertex> pnts = new List<Vertex>();
            for (int i = 0; i < mesh.geometric_vertices.Count; i++)
            {
                pnts.Add(new Vertex(mesh.geometric_vertices[i]));
                pnts[i].perspective(dist, x_angle, -y_angle);
            }
            //List<Vertex> normal_pnts = new List<Vertex>();
            //for (int i = 0; i < mesh.vertex_normals.Count; i++)
            //{
            //    normal_pnts.Add(new Vertex(mesh.vertex_normals[i]));
            //    normal_pnts[i].perspective(dist, x_angle, -y_angle);
            //}

            Color c = Color.LightSkyBlue;

            List<Color> clrs = new List<Color>();
            for (int k = 0; k < mesh.vertex_normals.Count; k++)
            {
                double intens = l.IntensityDiffusion(0.9, mesh.vertex_normals[k]);
                if (intens < 0)
                    intens = 0;
                clrs.Add(Color.FromArgb((int)(c.R * intens), (int)(c.G * intens), (int)(c.B * intens)));
            }

            List<Triangle> front = front_faces(mesh.triangles, pnts, camera);
            for (int i = 0; i < front.Count; i++)
            {
                Point p1 = new Point((int)pnts[front[i].v1].X + bmp.Width / 2,
                    (int)pnts[front[i].v1].Y + bmp.Height / 2);
                Point p2 = new Point((int)pnts[front[i].v2].X + bmp.Width / 2,
                    (int)pnts[front[i].v2].Y + bmp.Height / 2);
                Point p3 = new Point((int)pnts[front[i].v3].X + bmp.Width / 2,
                    (int)pnts[front[i].v3].Y + bmp.Height / 2);

                interpolate_light(p1, p2, p3, clrs[front[i].vn1], clrs[front[i].vn2], clrs[front[i].vn3]);
            }
        }

        private void show_textured(Mesh mesh)
        {
            List<Vertex> pnts = new List<Vertex>();

            for (int i = 0; i < mesh.geometric_vertices.Count; i++)
            {
                pnts.Add(new Vertex(mesh.geometric_vertices[i]));
                pnts[i].perspective(dist, x_angle, -y_angle);
            }

            Bitmap tex_image = new Bitmap(tex_file_name);

            List<Triangle> front = front_faces(mesh.triangles, pnts, camera);
            for (int i = 0; i < front.Count; i++)
            {
                Point p1 = new Point((int)pnts[front[i].v1].X + bmp.Width / 2,
                    (int)pnts[front[i].v1].Y + bmp.Height / 2);
                Point p2 = new Point((int)pnts[front[i].v2].X + bmp.Width / 2,
                    (int)pnts[front[i].v2].Y + bmp.Height / 2);
                Point p3 = new Point((int)pnts[front[i].v3].X + bmp.Width / 2,
                    (int)pnts[front[i].v3].Y + bmp.Height / 2);

                TextureVertex vt1 = new TextureVertex(mesh.texture_vertices[front[i].vt1]);
                TextureVertex vt2 = new TextureVertex(mesh.texture_vertices[front[i].vt2]);
                TextureVertex vt3 = new TextureVertex(mesh.texture_vertices[front[i].vt3]);

                //vt1.W = 1 * pnts[front[i].v1].W; vt2.W = 1 * pnts[front[i].v2].W; vt3.W = 1 * pnts[front[i].v3].W;
                //vt1.U *= vt1.W; vt2.U *= vt2.W; vt3.U *= vt3.W;
                //vt1.V *= vt1.W; vt2.V *= vt2.W; vt3.V *= vt3.W;

                vt1.W = 1 / pnts[front[i].v1].W; vt2.W = 1 / pnts[front[i].v2].W; vt3.W = 1 / pnts[front[i].v3].W;
                vt1.U *= vt1.W; vt2.U *= vt2.W; vt3.U *= vt3.W;
                vt1.V *= vt1.W; vt2.V *= vt2.W; vt3.V *= vt3.W;

                interpolate_texture(p1, p2, p3, vt1, vt2, vt3, tex_image);
            }
        }

        private void show_none(Mesh mesh)
        {
            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < mesh.geometric_vertices.Count; i++)
            {
                vertices.Add(new Vertex(mesh.geometric_vertices[i]));
                vertices[i].perspective(dist, x_angle, -y_angle);
            }

            List<Triangle> front_triangles = front_faces(mesh.triangles, vertices, camera);
            for (int i = 0; i < front_triangles.Count; i++)
            {
                Point p1 = new Point((int)vertices[front_triangles[i].v1].X + bmp.Width / 2,
                    (int)vertices[front_triangles[i].v1].Y + bmp.Height / 2);
                Point p2 = new Point((int)vertices[front_triangles[i].v2].X + bmp.Width / 2,
                    (int)vertices[front_triangles[i].v2].Y + bmp.Height / 2);
                Point p3 = new Point((int)vertices[front_triangles[i].v3].X + bmp.Width / 2,
                    (int)vertices[front_triangles[i].v3].Y + bmp.Height / 2);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawLine(new Pen(Color.LightGray), p1, p2);
                    g.DrawLine(new Pen(Color.LightGray), p2, p3);
                    g.DrawLine(new Pen(Color.LightGray), p3, p1);
                }
            }
        }

        private void clear_bmp()
        {
            if (bmp != null)
                bmp.Dispose();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            show_axis();
        }

        private void show_axis()
        {
            List<Vertex> axis = new List<Vertex> { new Vertex(0, 0, 0), new Vertex(400, 0, 0), new Vertex(0, 400, 0), new Vertex(0, 0, -400) };
            List<Color> colors = new List<Color> { Color.Black, Color.IndianRed, Color.LimeGreen, Color.SteelBlue };
            for (int i = 0; i < 4; i++)
            {
                axis[0].perspective(dist, x_angle, y_angle);
                axis[i].perspective(dist, x_angle, y_angle);

                //if (axis[0].Z > dist)
                //    axis[0].Z = -axis[0].Z;
                //if (axis[i].Z > dist)
                //    axis[i].Z = -axis[i].Z;

                Point p1_2D = new Point(bmp.Width / 2 + (int)axis[0].X,
                    bmp.Height / 2 - (int)axis[0].Y);
                Point p2_2D = new Point(bmp.Width / 2 + (int)axis[i].X,
                    bmp.Height / 2 - (int)axis[i].Y);

                using (Graphics g = Graphics.FromImage(bmp))
                    g.DrawLine(new Pen(colors[i]), p1_2D, p2_2D);
            }
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            meshes.Clear();
            clear_bmp();
            redraw();
        }

        private List<Triangle> front_faces(List<Triangle> all_faces, List<Vertex> all_vertices, Vertex camera_pos)
        {
            Vertex direction_of_sight = camera_pos;
            List<Triangle> front_faces = new List<Triangle>();

            foreach (Triangle t in all_faces)
                if (Mesh.angleIsObtuse(t.normal(all_vertices), direction_of_sight))
                    front_faces.Add(t);

            return front_faces;
        }

        private void radioButton_none_CheckedChanged(object sender, EventArgs e)
        {
            //gettting the number of the radio button from its name
            RadioButton radioBtn = this.Controls.OfType<RadioButton>().Where(z => z.Checked).FirstOrDefault();
            string str = radioBtn.Name;
            string res = string.Empty;
            int val = 10;
            for (int j = 0; j < str.Length; j++)
                if (Char.IsDigit(str[j]))
                    res += str[j];
            if (res.Length > 0)
                val = int.Parse(res);

            //switching buttons
            switch (val)
            {
                case 2:
                    lighting = true;
                    texturing = false;
                    break;
                case 3:
                    lighting = false;
                    texturing = true;
                    break;
                default:
                    lighting = false;
                    texturing = false;
                    break;
            }
        }

        private void radioButton_light_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_none_CheckedChanged(sender, e);
        }

        private void radioButton_texture_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_none_CheckedChanged(sender, e);
        }

        private void interpolate_light(Point p1, Point p2, Point p3, Color c1, Color c2, Color c3)
        {
            if (p2.Y < p1.Y)
            {
                (p1, p2) = (p2, p1);
                (c1, c2) = (c2, c1);
            }
            if (p3.Y < p1.Y)
            {
                (p1, p3) = (p3, p1);
                (c1, c3) = (c3, c1);
            }
            if (p3.Y < p2.Y)
            {
                (p2, p3) = (p3, p2);
                (c2, c3) = (c3, c2);
            }

            int dy1 = p2.Y - p1.Y;
            int dx1 = p2.X - p1.X;
            int dr1 = c2.R - c1.R;
            int dg1 = c2.G - c1.G;
            int db1 = c2.B - c1.B;

            int dy2 = p3.Y - p1.Y;
            int dx2 = p3.X - p1.X;
            int dr2 = c3.R - c1.R;
            int dg2 = c3.G - c1.G;
            int db2 = c3.B - c1.B;

            double dax_step = 0, dbx_step = 0;
            double dr1_step = 0, dr2_step = 0, // --------- int
                dg1_step = 0, dg2_step = 0,
                db1_step = 0, db2_step = 0;
            //next point with current step
            double col_r, col_g, col_b;

            if (dy1 != 0)
            {
                dax_step = dx1 / (double)Math.Abs(dy1);
                dr1_step = dr1 / (double)Math.Abs(dy1);
                dg1_step = dg1 / (double)Math.Abs(dy1);
                db1_step = db1 / (double)Math.Abs(dy1);
            }
            if (dy2 != 0)
            {
                dbx_step = dx2 / (double)Math.Abs(dy2);
                dr2_step = dr2 / (double)Math.Abs(dy2);
                dg2_step = dg2 / (double)Math.Abs(dy2);
                db2_step = db2 / (double)Math.Abs(dy2);
            }

            if (dy1 != 0)
            {
                for (int i = p1.Y; i <= p2.Y; i++)
                {
                    int ax = (int)(p1.X + (double)(i - p1.Y) * dax_step);
                    int bx = (int)(p1.X + (double)(i - p1.Y) * dbx_step);
                    //start red, g,b
                    double col_sr = c1.R + (i - p1.Y) * dr1_step;
                    double col_sg = c1.G + (i - p1.Y) * dg1_step;
                    double col_sb = c1.B + (i - p1.Y) * db1_step;
                    //end red,g,b
                    double col_er = c1.R + (i - p1.Y) * dr2_step;
                    double col_eg = c1.G + (i - p1.Y) * dg2_step;
                    double col_eb = c1.B + (i - p1.Y) * db2_step;

                    if (ax > bx)
                    {
                        (ax, bx) = (bx, ax);
                        (col_sr, col_er) = (col_er, col_sr);
                        (col_sg, col_eg) = (col_eg, col_sg);
                        (col_sb, col_eb) = (col_eb, col_sb);
                    }

                    double tstep = 1.0 / (bx - ax);
                    double t = 0;

                    for (int j = ax; j < bx; j++)
                    {
                        col_r = ((1 - t) * col_sr + t * col_er);
                        col_g = ((1 - t) * col_sg + t * col_eg);
                        col_b = ((1 - t) * col_sb + t * col_eb);
                        if (!out_of_PB(j, i, ref bmp))
                            bmp.SetPixel(j, i, Color.FromArgb((int)col_r, (int)col_g, (int)col_b));
                        t += tstep;
                    }
                }
            }

            dy1 = p3.Y - p2.Y;
            dx1 = p3.X - p2.X;
            dr1 = c3.R - c2.R;
            dg1 = c3.G - c2.G;
            db1 = c3.B - c2.B;
            dr1_step = dg1_step = db1_step = 0;
            if (dy1 != 0)
            {
                dax_step = dx1 / (double)Math.Abs(dy1);
                dr1_step = dr1 / (double)Math.Abs(dy1);
                dg1_step = dg1 / (double)Math.Abs(dy1);
                db1_step = db1 / (double)Math.Abs(dy1);
            }
            if (dy2 != 0)
            {
                dbx_step = dx2 / (double)Math.Abs(dy2);
            }
            for (int i = p2.Y; i <= p3.Y; i++)
            {
                int ax = (int)(p2.X + (i - p2.Y) * dax_step);
                int bx = (int)(p1.X + (i - p1.Y) * dbx_step);
                //start red, g,b
                double col_sr = c2.R + (i - p2.Y) * dr1_step;
                double col_sg = c2.G + (i - p2.Y) * dg1_step;
                double col_sb = c2.B + (i - p2.Y) * db1_step;
                //end red,g,b
                double col_er = c1.R + (i - p1.Y) * dr2_step;
                double col_eg = c1.G + (i - p1.Y) * dg2_step;
                double col_eb = c1.B + (i - p1.Y) * db2_step;

                if (ax > bx)
                {
                    (ax, bx) = (bx, ax);
                    (col_sr, col_er) = (col_er, col_sr);
                    (col_sg, col_eg) = (col_eg, col_sg);
                    (col_sb, col_eb) = (col_eb, col_sb);

                }

                double tstep = 1.0 / (bx - ax);
                double t = 0;

                for (int j = ax; j < bx; j++)
                {
                    col_r = ((1 - t) * col_sr + t * col_er);
                    col_g = ((1 - t) * col_sg + t * col_eg);
                    col_b = ((1 - t) * col_sb + t * col_eb);
                    if (!out_of_PB(j, i, ref bmp))
                        bmp.SetPixel(j, i, Color.FromArgb((int)col_r, (int)col_g, (int)col_b));
                    t += tstep;
                }
            }
        }

        private void interpolate_texture(Point p1, Point p2, Point p3, TextureVertex vt1, TextureVertex vt2, TextureVertex vt3, Bitmap tex_image)
        {
            // v1 is the top point (has the smallest y)
            if (p2.Y < p1.Y)
            {
                (p1, p2) = (p2, p1);
                (vt1, vt2) = (vt2, vt1);
            }
            if (p3.Y < p1.Y)
            {
                (p1, p3) = (p3, p1);
                (vt1, vt3) = (vt3, vt1);
            }
            if (p3.Y < p2.Y)
            {
                (p2, p3) = (p3, p2);
                (vt2, vt3) = (vt3, vt2);
            }

            //v1 -> v2 (will change later)
            int dy1 = p2.Y - p1.Y;
            int dx1 = p2.X - p1.X;
            double du1 = vt2.U - vt1.U;
            double dv1 = vt2.V - vt1.V;
            double dw1 = vt2.W - vt1.W;

            //v1 -> v3
            int dy2 = p3.Y - p1.Y;
            int dx2 = p3.X - p1.X;
            double du2 = vt3.U - vt1.U;
            double dv2 = vt3.V - vt1.V;
            double dw2 = vt3.W - vt1.W;

            // ax : v1 -> v2
            // bx : v1 -> v3
            double dax_step = 0, dbx_step = 0,
                du1_step = 0, du2_step = 0,
                dv1_step = 0, dv2_step = 0,
                dw1_step = 0, dw2_step = 0;
            double u_step, v_step, w_step;

            if (dy1 != 0)
            {
                dax_step = dx1 / (double)Math.Abs(dy1);
                du1_step = du1 / Math.Abs(dy1);
                dv1_step = dv1 / Math.Abs(dy1);
                dw1_step = dw1 / Math.Abs(dy1);
            }
            if (dy2 != 0)
            {
                dbx_step = dx2 / (double)Math.Abs(dy2);
                du2_step = du2 / Math.Abs(dy2);
                dv2_step = dv2 / Math.Abs(dy2);
                dw2_step = dw2 / Math.Abs(dy2);
            }


            if (dy1 != 0)
            {
                // i is Y position
                for (int i = p1.Y; i <= p2.Y; i++)
                {
                    // steps to the left & to the right
                    int ax = (int)(p1.X + (i - p1.Y) * dax_step);
                    int bx = (int)(p1.X + (i - p1.Y) * dbx_step);

                    // u, v start & end
                    double u_start = vt1.U + (i - p1.Y) * du1_step;
                    double v_start = vt1.V + (i - p1.Y) * dv1_step;
                    double w_start = vt1.W + (i - p1.Y) * dw1_step;

                    double u_end = vt1.U + (i - p1.Y) * du2_step;
                    double v_end = vt1.V + (i - p1.Y) * dv2_step;
                    double w_end = vt1.W + (i - p1.Y) * dw2_step;

                    if (ax > bx)
                    {
                        (ax, bx) = (bx, ax);
                        (u_start, u_end) = (u_end, u_start);
                        (v_start, v_end) = (v_end, v_start);
                        (w_start, w_end) = (w_end, w_start);
                    }

                    double tstep = 1.0 / (bx - ax);
                    double t = 0;

                    for (int j = ax; j < bx; j++)
                    {
                        u_step = ((1 - t) * u_start + t * u_end);
                        v_step = ((1 - t) * v_start + t * v_end);
                        w_step = ((1 - t) * w_start + t * w_end);

                        int pixel_x = (int)((tex_image.Width - 1) * u_step / w_step);
                        int pixel_y = (int)((tex_image.Height - 1) * v_step / w_step);

                        if (!out_of_PB(j, i, ref bmp))
                            bmp.SetPixel(j, i, tex_image.GetPixel(pixel_x, pixel_y));
                        t += tstep;
                    }
                }
            }

            dy1 = p3.Y - p2.Y;
            dx1 = p3.X - p2.X;
            du1 = vt3.U - vt2.U;
            dv1 = vt3.V - vt2.V;
            du1_step = dv1_step = 0;
            if (dy1 != 0)
            {
                dax_step = dx1 / (double)Math.Abs(dy1);
                du1_step = du1 / Math.Abs(dy1);
                dv1_step = dv1 / Math.Abs(dy1);
                dw1_step = dw1 / Math.Abs(dy1);
            }
            if (dy2 != 0)
            {
                dbx_step = dx2 / (double)Math.Abs(dy2);
            }
            for (int i = p2.Y; i <= p3.Y; i++)
            {
                int ax = (int)(p2.X + (i - p2.Y) * dax_step);
                int bx = (int)(p1.X + (i - p1.Y) * dbx_step);
                //start red, g,b
                double u_start = vt2.U + (i - p2.Y) * du1_step;
                double v_start = vt2.V + (i - p2.Y) * dv1_step;
                double w_start = vt2.W + (i - p2.Y) * dw1_step;
                //end red,g,b
                double u_end = vt1.U + (i - p1.Y) * du2_step;
                double v_end = vt1.V + (i - p1.Y) * dv2_step;
                double w_end = vt1.W + (i - p1.Y) * dw2_step;

                if (ax > bx)
                {
                    (ax, bx) = (bx, ax);
                    (u_start, u_end) = (u_end, u_start);
                    (v_start, v_end) = (v_end, v_start);
                    (w_start, w_end) = (w_end, w_start);
                }

                double tstep = 1.0 / (bx - ax);
                double t = 0;

                for (int j = ax; j < bx; j++)
                {
                    u_step = ((1 - t) * u_start + t * u_end);
                    v_step = ((1 - t) * v_start + t * v_end);
                    w_step = ((1 - t) * w_start + t * w_end);

                    int pixel_x = (int)((tex_image.Width - 1) * u_step / w_step); 
                    int pixel_y = (int)((tex_image.Height - 1) * v_step / w_step); 

                    bmp.SetPixel(j, i, tex_image.GetPixel(pixel_x, pixel_y));
                    t += tstep;
                }
            }
        }


        //check if point is out of the Picture Box
        public static bool out_of_PB(int x, int y, ref Bitmap bmp)
        {
            return x <= 0 || y <= 0 || x >= bmp.Width || y >= bmp.Height;
        }

        // --------- MOVE ---------
        private void button1_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(-10, 0, 0);
            redraw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(0, 10, 0);
            redraw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(0, 0, -10);
            redraw();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(10, 0, 0);
            redraw();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(0, -10, 0);
            redraw();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Move(0, 0, 10);
            redraw();
        }

        // --------- ROTATE ---------
        private void button7_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(-18, 0, 0);
            redraw();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(0, -18, 0);
            redraw();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(0, 0, -18);
            redraw();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(18, 0, 0);
            redraw();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(0, 18, 0);
            redraw();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Rotate(0, 0, 18);
            redraw();
        }

        // --------- SCALE ---------
        private void button13_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(0.9, 1, 1);
            redraw();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(1, 0.9, 1);
            redraw();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(1, 1, 0.9);
            redraw();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(1.1, 1, 1);
            redraw();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(1, 1.1, 1);
            redraw();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            meshes[cur_mesh_ind].Scale(1, 1, 1.1);
            redraw();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            FloatingHorizontON();
        }
    }
}
