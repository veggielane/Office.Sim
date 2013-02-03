using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veg.Maths;

namespace Office.Sim.Core.Graphics
{
    public interface ICamera
    {
        // Vect3 Postion { get; }
        double Near { get; }
        double Far { get; }

        Mat4 Model { get; set; }
        Mat4 View { get; }
        Mat4 Projection { get; set; }
        Mat4 MVP { get; }

        Vect3 Eye { get; set; }
        Vect3 Target { get; set; }
        Vect3 Up { get; set; }

        void Update(double delta);
        void Resize(int width, int height);
    }
}
