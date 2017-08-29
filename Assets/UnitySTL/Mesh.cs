using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace STLBuilder
{
    public class Mesh
    {
        public string Name { get; private set; }
        public IEnumerable<Facet> Facets { get; private set; }

        public Mesh(string name, IEnumerable<Facet> facets)
        {
            this.Name = name;
            this.Facets = new List<Facet>(facets);
        }

        public static Mesh Build(string name, params MeshContainer[] baseMeshes)
        {
            LinkedList<Facet> facets = new LinkedList<Facet>();

            foreach (var container in baseMeshes)
            {
                var mesh = container.Mesh;
                for (int i = 0; i < mesh.triangles.Length; i += 3)
                {
                    Vector3 normal = (mesh.normals[mesh.triangles[i]] +
                                     mesh.normals[mesh.triangles[i + 1]] +
                                     mesh.normals[mesh.triangles[i + 2]]) / 3.0f;
                    facets.AddLast(new Facet(
                        new Vertex(normal),
                        new Vertex(mesh.vertices[mesh.triangles[i]] + container.Translation),
                        new Vertex(mesh.vertices[mesh.triangles[i + 1]] + container.Translation),
                        new Vertex(mesh.vertices[mesh.triangles[i + 2]] + container.Translation)));
                }
            }

            return new Mesh(name, facets);
        }
    }

    public class MeshContainer
    {
        public UnityEngine.Mesh Mesh { get; set; }
        public Vector3 Translation { get; set; }
    }
}
