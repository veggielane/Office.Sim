using System;
using Veg.Maths;

namespace Office.Sim.Core.Graphics.OpenTK
{
    public class MainCamera : ICamera
    {
        private int _width;
        private int _height;

        private Mat4 _view;
        public MainCamera()
        {

            Near = 1f;
            Far = 500f;
            Model = Mat4.Translate(0,0,0);
            Target = Vect3.Zero;
            Up = Vect3.UnitY;

            Eye = new Vect3(0,0,20);


            _view =Mat4.LookAt(Eye, Target, Up);

           // _view = Mat4.RotateX(Angle.FromRadians(Math.Atan(Math.Sin(Angle.FromDegrees(-45))))) * (Mat4.LookAt(Eye, Target, Up) * Mat4.RotateZ(Angle.FromDegrees(45)));
            //_view = Mat4.Identity;
        }

        public double Near { get; private set; }
        public double Far { get; private set; }
        public Mat4 Model { get; set; }


        public Mat4 View
        {
            get { return _view; }
            set { _view = value; }
        }

        public Mat4 Projection { get; set; }

        public Mat4 MVP
        {
            get { return Projection * View * Model; }
        }

        public Vect3 Eye { get; set; }

        public Vect3 Target { get; set; }

        public Vect3 Up { get; set; }

        public void Update(double delta)
        {

        }

        public void Resize(int width, int height)
        {
            _width = width;
            _height = height;
            Projection = Mat4.CreatePerspectiveFieldOfView(Math.PI / 2, _width / (float)_height, Near, Far);

        }
    }
}