using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DEngine {
    class Triangle {
        public Triangle(Point3D a, Point3D b, Point3D c, Color color) {
            
        }
    }

    struct Point3D {
        public Point3D(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public float x { get; }
        public float y { get; }
        public float z { get; }
    }
}
