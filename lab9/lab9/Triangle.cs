using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class Triangle
    {
        public int v1, v2, v3;
        public int vt1, vt2, vt3;
        public int vn1, vn2, vn3;

        public Triangle(int v1_, int v2_, int v3_, 
                        int vt1_, int vt2_, int vt3_, 
                        int vn1_, int vn2_, int vn3_)
        {
            v1 = v1_; v2 = v2_; v3 = v3_;
            vt1 = vt1_; vt2 = vt2_; vt3 = vt3_;
            vn1 = vn1_; vn2 = vn2_; vn3 = vn3_;
        }

        public Triangle(string vertex1, string vertex2, string vertex3)
        {
            string []coords1 = vertex1.Split('/');
            string []coords2 = vertex2.Split('/');
            string []coords3 = vertex3.Split('/');

            v1 = int.Parse(coords1[0]) - 1;
            v2 = int.Parse(coords2[0]) - 1;
            v3 = int.Parse(coords3[0]) - 1;

            vt1 = int.Parse(coords1[1]) - 1;
            vt2 = int.Parse(coords2[1]) - 1;
            vt3 = int.Parse(coords3[1]) - 1;

            vn1 = int.Parse(coords1[2]) - 1;
            vn2 = int.Parse(coords2[2]) - 1;
            vn3 = int.Parse(coords3[2]) - 1;
        }

        public Vertex normal(List<Vertex> vertices)
        {
            Vertex vec1 = vertices[v2] - vertices[v1];
            Vertex vec2 = vertices[v1] - vertices[v3];

            return new Vertex(vec1.Y * vec2.Z - vec1.Z * vec2.Y,
                        vec1.Z * vec2.X - vec1.X * vec2.Z,
                        vec1.X * vec2.Y - vec1.Y * vec2.X);
        }
    }
}
