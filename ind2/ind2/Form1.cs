using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Color = System.Drawing.Color;

namespace ind2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static List<SceneShape> scene = new List<SceneShape>();
        private void InitBall(Point3D center, double r, Color color, Material material)
        {
            scene.Add(new SceneShape(ShapeType.Ball, new Sphere(center, r), color, material));
        }
        private void InitBalls()
        {
            InitBall(new Point3D(-20, 0, 20), 4, Color.Aquamarine, Material.Transparent);
            InitBall(new Point3D(-10, 10, 50), 10, Color.Green, Material.Mirror);
            InitBall(new Point3D(15, -10, 50), 2, Color.Red, Material.Matte);
            InitBall(new Point3D(0, 0, 50), 2, Color.Blue, Material.Matte);
            InitBall(new Point3D(-24, 5, 18), 0.8, Color.Aquamarine, Material.Transparent);
            InitBall(new Point3D(-24, -7, 23), 1, Color.Aquamarine, Material.Transparent);
        }
        private void InitWall(Point3D p, Point3D p2, Point3D normal, Color color, Material material)
        {
            scene.Add(new SceneShape(ShapeType.Wall, new Face(p, p2, normal), color, material));
        }
        private void InitFace(Point3D p, Point3D p2, Point3D normal, Color color, Material material)
        {
            scene.Add(new SceneShape(ShapeType.Face, new Face(p, p2, normal), color, material));
        }
        private void InitRoom()
        {
            (double x, double y, double z) = (25, 25, 70);

            // Задняя стена
            InitWall(new Point3D(-x, -y, z), new Point3D(x, y, z), new Point3D(0, 0, -1), Color.Coral, Material.Matte);
            // Левая
            InitWall(new Point3D(-x, -y, -z), new Point3D(x, -y, z), new Point3D(0, 1, 0), Color.BlueViolet, Material.Matte);
            // Правая
            InitWall(new Point3D(-x, y, -z), new Point3D(x, y, z), new Point3D(0, -1, 0), Color.DarkGreen, Material.Mirror);
            // Пол
            InitWall(new Point3D(-x, -y, -z), new Point3D(-x, y, z), new Point3D(1, 0, 0), Color.White, Material.Matte);
            // Потолок
            InitWall(new Point3D(x, -y, -z), new Point3D(x, y, z), new Point3D(-1, 0, 0), Color.Violet, Material.Mirror);
            // Передняя стена (видна только в отражении)
            InitWall(new Point3D(-x, -y, -z), new Point3D(x, y, -z), new Point3D(0, 0, 1), Color.Cyan, Material.Matte);
        }
        private void InitCube(Point3D center, double r, Color color, Material material)
        {
            double x1 = center.x - r, x2 = center.x + r;
            double y1 = center.y - r, y2 = center.y + r;
            double z1 = center.z - r, z2 = center.z + r;
            InitFace(new Point3D(x1, y1, z2), new Point3D(x2, y2, z2), new Point3D(0, 0, 1), color, material);
            InitFace(new Point3D(x1, y1, z1), new Point3D(x2, y2, z1), new Point3D(0, 0, -1), color, material);

            InitFace(new Point3D(x1, y1, z1), new Point3D(x1, y2, z2), new Point3D(-1, 0, 0), color, material);
            InitFace(new Point3D(x2, y1, z1), new Point3D(x2, y2, z2), new Point3D(1, 0, 0), color, material);

            InitFace(new Point3D(x1, y1, z1), new Point3D(x2, y1, z2), new Point3D(0, -1, 0), color, material);
            InitFace(new Point3D(x1, y2, z1), new Point3D(x2, y2, z2), new Point3D(0, 1, 0), color, material);
        }


        private void InitCube1(Point3D center, double r, Color color, Material material)
        {
            double x1 = center.x - r, x2 = center.x + r - 3;
            double y1 = center.y - r, y2 = center.y + r + 5;
            double z1 = center.z - r, z2 = center.z + r;
            InitFace(new Point3D(x1, y1, z2), new Point3D(x2, y2, z2), new Point3D(0, 0, 1), color, material);
            InitFace(new Point3D(x1, y1, z1), new Point3D(x2, y2, z1), new Point3D(0, 0, -1), color, material);

            InitFace(new Point3D(x1, y1, z1), new Point3D(x1, y2, z2), new Point3D(-1, 0, 0), color, material);
            InitFace(new Point3D(x2, y1, z1), new Point3D(x2, y2, z2), new Point3D(1, 0, 0), color, material);

            InitFace(new Point3D(x1, y1, z1), new Point3D(x2, y1, z2), new Point3D(0, -1, 0), color, material);
            InitFace(new Point3D(x1, y2, z1), new Point3D(x2, y2, z2), new Point3D(0, 1, 0), color, material);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitBalls();
            InitRoom();
            InitCube(new Point3D(-21, -15, 50), 4, Color.Brown, Material.Matte);
            InitCube(new Point3D(-14, -15, 50), 3, Color.GreenYellow, Material.Mirror);
            InitCube1(new Point3D(-22, -15, 40), 3, Color.PaleGreen, Material.Matte);


            List<LightSource> lights = new List<LightSource>
            {
               new LightSource(new Point3D(20, -10, 15), 0.4),
               new LightSource(new Point3D(10, 10, 15), 0.8)
            };

            pictureBox1.Image = RayTracing.GetImage(pictureBox1.Width, pictureBox1.Height, scene, lights, this);
        }
    }
}
