using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9
{
    class Light
    {
        public Vertex position;
        public double intensity;

        public Light(Vertex pos, double intens)
        {
            position = pos;
            intensity = intens;
        }

        public double IntensityDiffusion(double koeff, Vertex normal)
        {
            return intensity * koeff * cos(normal, position);
        }

        private double scalar_prod(Vertex vec1, Vertex vec2)
        {
            var mult_vec = vec1 * vec2;
            return mult_vec.X + mult_vec.Y + mult_vec.Z;
        }

        private double cos(Vertex v1, Vertex v2)
        {
            return scalar_prod(v1, v2) / (Mesh.Distance(v1) * Mesh.Distance(v2));
        }

    }
}
