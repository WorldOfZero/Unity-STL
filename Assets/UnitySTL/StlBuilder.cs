using System;
using System.Collections.Generic;
using System.Text;

namespace STLBuilder
{
    public static class StlBuilder
    {
        public static string Build(Mesh mesh)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("solid {0}", mesh.Name));

            foreach (var facet in mesh.Facets)
            {
                builder.AppendLine(String.Format("facet normal {0} {1} {2}", facet.Normal.X, facet.Normal.Y, facet.Normal.Z));
                builder.AppendLine("  outer loop");
                foreach (var vertex in facet.Vertices)
                {
                    builder.AppendLine(String.Format("    vertex {0} {1} {2}", vertex.X, vertex.Y, vertex.Z));
                }
                builder.AppendLine("  endloop");
                builder.AppendLine("endfacet");
            }

            builder.AppendLine(String.Format("endsolid {0}", mesh.Name));
            return builder.ToString();
        }
    }
}
