using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace STLBuilder
{
    public class Vertex
    {
        private float[] verticies = new float[3];

        public Vertex(float x = 0, float y = 0, float z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vertex(Vector3 vector)
        {
            this.X = vector.x;
            this.Y = vector.y;
            this.Z = vector.z;
        }

        public float X
        {
            get { return verticies[0]; }
            set
            {
                verticies[0] = value;
            }
        }

        public float Y
        {
            get { return verticies[1]; }
            set
            {
                verticies[1] = value;
            }
        }

        public float Z
        {
            get { return verticies[2]; }
            set
            {
                verticies[2] = value;
            }
        }
    }
}
