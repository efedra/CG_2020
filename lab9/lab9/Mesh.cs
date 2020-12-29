using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class Mesh
    {
        // points of polyhedron
        public List<Vertex> geometric_vertices = new List<Vertex>();
        // points of polyhedron
        public List<TextureVertex> texture_vertices = new List<TextureVertex>();
        // points of polyhedron
        public List<Vertex> vertex_normals = new List<Vertex>();

        // faces of polyhedron
        public List<Triangle> triangles = new List<Triangle>();
        // central point of polyhedron (weights)
        public Vertex center_point = new Vertex();

        public Mesh(List<Vertex> vert, List<Triangle> triangles_)
        {
            geometric_vertices = vert;
            triangles = triangles_;
            center_point = SetCenter();
        }

        public Mesh() { }

        public void Clear()
        {
            geometric_vertices.Clear();
            texture_vertices.Clear();
            vertex_normals.Clear();
            triangles.Clear();
        }

        public void FromFile(string filename)
        {
            Clear();

            StreamReader sr = File.OpenText(filename);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                    continue;
                line = line.Replace(',', '.');
                string[] ss = line.Split();
                ss = ss.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string type = ss[0];
                switch (type)
                {
                    case "v":
                        geometric_vertices.Add(new Vertex(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture),
                            double.Parse(ss[3], CultureInfo.InvariantCulture), 
                            1));
                        break;

                    case "vt":
                        texture_vertices.Add(new TextureVertex(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture),
                            1));
                        break;

                    case "vn":
                        vertex_normals.Add(new Vertex(double.Parse(ss[1], CultureInfo.InvariantCulture),
                            double.Parse(ss[2], CultureInfo.InvariantCulture),
                            double.Parse(ss[3], CultureInfo.InvariantCulture),
                            1));
                        break;

                    case "f":
                        triangles.Add(new Triangle(ss[1], ss[2], ss[3]));
                        break;

                    default:
                        break;
                }
            }

            sr.Close();
            center_point = SetCenter();
        }

        private Vertex SetCenter(Vertex p1, Vertex p2, Vertex p3) => (p1 + p2 + p3) / 3;

        public Vertex SetCenter()
        {
            Vertex p = new Vertex();
            foreach (Vertex p1 in geometric_vertices)
                p += p1;
            return p / geometric_vertices.Count();
        }

         // Affine transformations
        public void Move(double dx, double dy, double dz)
        {
            Matrix matrix = Matrix.Move(dx, dy, dz);
            foreach (Vertex v in geometric_vertices)
                v.Apply(matrix);
            foreach (Vertex vn in vertex_normals)
                vn.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void Rotate(double angleX, double angleY, double angleZ)
        {
            Matrix matrix = Matrix.Rotate(angleX, angleY, angleZ);
            foreach (Vertex point in geometric_vertices)
                point.Apply(matrix);
            foreach (Vertex vn in vertex_normals)
                vn.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void Scale(double fx, double fy, double fz)
        {
            Matrix matrix = Matrix.Scale(fx, fy, fz);
            foreach (Vertex point in geometric_vertices)
                point.Apply(matrix);
            foreach (Vertex vn in vertex_normals)
                vn.Apply(matrix);
            center_point.Apply(matrix);
        }

        public void ReflectX() => Scale(1, -1, -1);

        public void ReflectY() => Scale(-1, 1, -1);

        public void ReflectZ() => Scale(-1, -1, 1);

        //---------------------------------------- ??? ----------------------------------------
        public static double Distance(Vertex vec) => Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);

        public static double Distance(Vertex p1, Vertex p2) => Distance(p2 - p1);

        public static bool angleIsObtuse(Vertex vec1, Vertex vec2)
        {
            var mult_vec = vec1 * vec2;
            var scalarProd = mult_vec.X + mult_vec.Y + mult_vec.Z;

            return scalarProd / (Distance(vec1) * Distance(vec2)) < 0;
        }


    }
}
