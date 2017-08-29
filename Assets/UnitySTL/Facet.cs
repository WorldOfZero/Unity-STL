using System;
using System.Collections.Generic;
using System.Text;

namespace STLBuilder
{
    public class Facet
    {
        public Vertex Normal { get; set; }
        public IEnumerable<Vertex> Vertices { get; private set; }

        public Facet(Vertex normal, params Vertex[] vertices)
        {
            this.Normal = normal;
            this.Vertices = new List<Vertex>(vertices);
        }
    }
}
