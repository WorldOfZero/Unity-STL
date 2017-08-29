using System;
using System.Collections.Generic;
using System.Text;

namespace STLBuilder
{
    public class MeshBuilder
    {
        private string name;
        private List<Facet> facets;

        public static MeshBuilder Create()
        {
            return new MeshBuilder();
        }

        private MeshBuilder()
        {
            name = "Unknown-Object";
            facets = new List<Facet>();
        }

        public MeshBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public MeshBuilder WithFacet(Facet facet)
        {
            facets.Add(facet);
            return this;
        }

        public MeshBuilder WithFacet(Vertex normal, params Vertex[] vertices)
        {
            facets.Add(new Facet(normal, vertices));
            return this;
        }

        public Mesh Build()
        {
            return new Mesh(name, facets);
        }
    }
}
