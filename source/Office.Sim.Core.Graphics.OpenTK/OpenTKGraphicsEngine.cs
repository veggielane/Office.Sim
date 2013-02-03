using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Messaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Veg.Maths;
using Veg.OpenTK;
using Veg.OpenTK.Buffers;
using Veg.OpenTK.Shaders;
using Veg.OpenTK.Vertices;

namespace Office.Sim.Core.Graphics.OpenTK
{
    public class OpenTKGraphicsEngine:IGraphicsEngine
    {
        private readonly IMessageBus _bus;
        private readonly ILevel _level;
        private Window _win;
        private Task _task;
        public OpenTKGraphicsEngine(IMessageBus bus, ILevel level)
        {
            _bus = bus;
            _level = level;
        }

        public void Start()
        {
            _task = new Task(() =>
                {
                    _win = new Window(_bus,_level);
                    _win.Run();
                });
            _task.Start();
        }

        public void LoadLevel(ILevel level)
        {
            //_win.Load("haha");
        }

        public void Stop()
        {
  
        }
    }

    public class Window:GameWindow
    {
        private readonly IMessageBus _bus;
        private readonly ILevel _level;



        private IShaderProgram _shader;
        private VBO _vbo;
        private VAO _vao;
        private CameraUBO _ubo;
        private readonly MainCamera _camera;

        public Window(IMessageBus bus, ILevel level)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "OpenCAD")
        {
            _bus = bus;
            _level = level;
            VSync = VSyncMode.On;

            _camera = new MainCamera();

            Mouse.WheelChanged += (sender, args) =>
            {
                _camera.Eye += new Vect3(0, 0, args.DeltaPrecise * -2.0);
            };
        }




        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PointSize(5f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            _ubo = new CameraUBO();
            _shader = new BasicShaderProgram(_ubo);
            var data = new List<Vertex>
            {

            };


            var size = 100f;
            for (var i = 0; i < _level.Map.Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < _level.Map.Tiles.GetLength(1); j++)
                {
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size, j * size, _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (size / 2f), j * size + (- size / 2f), _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (size / 2f), j * size + (size / 2f), _level.Map.Tiles[i, j].Height) });

                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size, j * size, 0) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (-size / 2f), j * size + (-size / 2f), _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (size / 2f), j * size + (-size / 2f), _level.Map.Tiles[i, j].Height) });

                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size, j * size, 0) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (-size / 2f), j * size + (size / 2f), _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (-size / 2f), j * size + (-size / 2f), _level.Map.Tiles[i, j].Height) });

                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size, j * size, _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (size / 2f), j * size + (size / 2f), _level.Map.Tiles[i, j].Height) });
                    data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i * size + (-size / 2f), j * size + (size / 2f), _level.Map.Tiles[i, j].Height) });



                   // data.Add(new Vertex() { Colour = Color4.LightYellow.ToVector4(), Position = new Vector3(i, j, _level.Map.Tiles[i, j].Height) });
                }
            }



            _vbo = new VBO(data) { BeginMode = BeginMode.Triangles };
            _vao = new VAO(_shader, _vbo);

            



            var err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at OnLoad: " + err);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Keyboard[Key.Left])
            {
                _camera.Eye += new Vect3(-0.05, 0, 0);
                _camera.Target += new Vect3(-0.05, 0, 0);
            }

            if (Keyboard[Key.Right])
            {
                _camera.Eye += new Vect3(0.05, 0, 0);
                _camera.Target += new Vect3(0.05, 0, 0);
            }

            if (Keyboard[Key.Up])
            {
                _camera.Eye += new Vect3(0, 0.05, 0);
                _camera.Target += new Vect3(0, 0.05, 0);
            }

            if (Keyboard[Key.Down])
            {
                _camera.Eye += new Vect3(0, -0.05, 0);
                _camera.Target += new Vect3(0, -0.05, 0);
            }
            _ubo.Update(_camera);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));



            using (new Bind(_vao))
            {
                GL.DrawArrays(_vao.VBO.BeginMode, 0, _vao.VBO.Count);
            }


            SwapBuffers();
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at Swapbuffers: " + err.ToString());
            Title = String.Format(" FPS:{0} Mouse<{1},{2}>", 1.0 / e.Time, Mouse.X, Height - Mouse.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _camera.Resize(Width, Height);
        }
    }



 

 

     public class BasicShaderProgram : BaseShaderProgram
     {
         public BasicShaderProgram(CameraUBO ubo)
         {
             CompileShader(ShaderType.VertexShader, @"#version 400
precision highp float; 

layout(std140) uniform Camera {
    mat4 MVP;
    mat4 Model;
    mat4 View;
    mat4 Projection;
    mat4 NormalMatrix;
};
layout (location = 0) in vec3 vert_position; 
layout (location = 1) in vec3 vert_normal; 
layout (location = 2) in vec4 vert_colour; 
out vec4 col;

void main(void) 
{ 
    gl_Position = (MVP) * vec4(vert_position, 1); 
    col = vert_colour;
}");
             CompileShader(ShaderType.FragmentShader, @"#version 400
in vec4 col;
layout( location = 0 ) out vec4 FragColor;
void main() {
    FragColor = col;
}");
             Link();
             ubo.BindToShaderProgram(this);
         }
     }

     public class CameraUBO : BaseUBO<CameraUBO.CameraData>
     {

         public struct CameraData
         {
             public Matrix4 MVP;
             public Matrix4 Model;
             public Matrix4 View;
             public Matrix4 Projection;
             public Matrix4 NormalMatrix;
         }

         public CameraUBO()
             : base("Camera", 0)
         {

         }

         public void Update(ICamera camera)
         {
             var normal = (camera.Model * camera.View).ToMatrix4();
             normal.Invert();
             normal.Transpose();
             Data = new CameraData
             {
                 MVP = camera.MVP.ToMatrix4(),
                 Model = camera.Model.ToMatrix4(),
                 View = camera.View.ToMatrix4(),
                 Projection = camera.Projection.ToMatrix4(),
                 NormalMatrix = normal
             };
             Update();
         }
     }


     public class MainCamera : ICamera
     {
         private int _width;
         private int _height;
         public MainCamera()
         {

             Near = 0f;
             Far = 512.0f;
             Model = Mat4.Identity;
             Eye = Vect3.Zero;
             Target = Vect3.Zero;
             Up = Vect3.UnitY;
             Eye = new Vect3(0.0f, 0.0f, 5.0f);
         }

         public double Near { get; private set; }
         public double Far { get; private set; }
         public Mat4 Model { get; set; }


         public Mat4 View
         {
             get { return Mat4.LookAt(Eye, Target, Up); }
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
             //Projection = Mat4.CreatePerspectiveFieldOfView(Math.PI / 4, _width / (float)_height, Near, Far);
             Projection = Matrix4.CreateOrthographic(width, height, (float)Near, (float)Far).ToMat4();
         }
     }

}
