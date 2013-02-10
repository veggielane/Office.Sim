using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Office.Sim.Core.Mapping;
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
  
        private VAO _vao;
        private VAO _vao2;

        private CameraUBO _ubo;
        private readonly ICamera _camera;

        public Window(IMessageBus bus, ILevel level)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "OpenCAD")
        {
            _bus = bus;
            _level = level;
            VSync = VSyncMode.On;

            _camera = new MainCamera();

            Mouse.WheelChanged += (sender, args) =>
                {
                    _camera.View = _camera.View* Mat4.Translate(0, 0, args.DeltaPrecise * -10.0);
                //_camera.Eye += new Vect3(0, 0, args.DeltaPrecise * -10.0);
               // Console.WriteLine(_camera.Eye);
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

            _ubo = new CameraUBO();
            _shader = new BasicShaderProgram(_ubo);
            var data = new List<Vertex>{};

            var green = new Color4(0.156f, 0.627f, 0.353f, 1.0f).ToVector4();

            for (var i = 0; i < _level.Map.Tiles.GetLength(0); i++)
            {
                for (var j = 0; j < _level.Map.Tiles.GetLength(1); j++)
                {
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C1.Position.ToVector3() });
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C3.Position.ToVector3() });
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C2.Position.ToVector3() });
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C1.Position.ToVector3() });
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C4.Position.ToVector3() });
                    data.Add(new Vertex { Colour = green, Position = _level.Map.Tiles[i, j].C3.Position.ToVector3() });
                }
            }


            foreach (var tile in Enumerable.Range(0, _level.Map.Tiles.GetLength(1)).Select(col => _level.Map.Tiles[0, col]))
            {

                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C1.Position.X,tile.C1.Position.Y,-10).ToVector3()});
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C1.Position.ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C2.Position.ToVector3() });
                                    
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C2.Position.ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C2.Position.X, tile.C2.Position.Y, -10).ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C1.Position.X, tile.C1.Position.Y, -10).ToVector3() });
            }

            foreach (var tile in Enumerable.Range(0, _level.Map.Tiles.GetLength(0)).Select(row => _level.Map.Tiles[row,0]))
            {

                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C4.Position.ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C1.Position.ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C4.Position.X, tile.C4.Position.Y, -10).ToVector3() });

                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = tile.C1.Position.ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C1.Position.X, tile.C1.Position.Y, -10).ToVector3() });
                data.Add(new Vertex { Colour = Color4.Brown.ToVector4(), Position = new Vect3(tile.C4.Position.X, tile.C4.Position.Y, -10).ToVector3() });

            }


            _vao = new VAO(_shader,  new VBO(data) { BeginMode = BeginMode.Triangles });



            

            var data2 = data.Select(v => new Vertex() {Colour = Color4.Black.ToVector4(), Position = v.Position + new Vector3(0,0,0.05f)});

            _vao2 = new VAO(_shader, new VBO(data2) { BeginMode = BeginMode.Lines });


            var err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at OnLoad: " + err);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            var speed = 0.05;
            if (Keyboard[Key.Left])
            {
                _camera.Eye += new Vect3(-speed, 0, speed);
                _camera.Target += new Vect3(-speed, 0, speed);
            }

            if (Keyboard[Key.Right])
            {
                _camera.View *= Mat4.Translate(1,0,0);
            }

            if (Keyboard[Key.Up])
            {
                _camera.Eye += new Vect3(speed, 0, -speed);
                _camera.Target += new Vect3(speed, 0, -speed);
            }

            if (Keyboard[Key.Down])
            {
                _camera.Eye += new Vect3(-speed, 0, speed);
                _camera.Target += new Vect3(-speed, 0, speed);
            }

            if (Keyboard[Key.A])
            {
                _camera.View *= Mat4.RotateY(Angle.FromDegrees(0.5));
            }

            if (Keyboard[Key.D])
            {
                _camera.View *= Mat4.RotateY(Angle.FromDegrees(-0.5));
            }

            if (Keyboard[Key.W])
            {
                _camera.View *= Mat4.RotateX(Angle.FromDegrees(0.5));
            }


            if (Keyboard[Key.S])
            {
                _camera.View *= Mat4.RotateX(Angle.FromDegrees(-0.5));
            }


            if (Keyboard[Key.Q])
            {
                _camera.View *= Mat4.RotateZ(Angle.FromDegrees(0.5));
            }
            if (Keyboard[Key.E])
            {
                _camera.View *= Mat4.RotateZ(Angle.FromDegrees(-0.5));
            }

            _ubo.Update(_camera);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            _vao.Render();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

            //_vao2.Render();

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



     public class OrthographicCamera : ICamera
     {
         private int _width;
         private int _height;
         public OrthographicCamera()
         {

             Near = -200f;
             Far = 5000f;
             Model = Mat4.Translate(-20, -20, 0);
             Target = Vect3.Zero;
             Up = -Vect3.UnitZ;
             Eye = new Vect3(Math.Sqrt(1 / 3.0), Math.Sqrt(1 / 3.0), Math.Sqrt(1 / 3.0));
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
             Projection = Matrix4.CreateOrthographic(width / 10f, height / 10f, (float)Near, (float)Far).ToMat4();
         }


         Mat4 ICamera.View
         {
             get
             {
                 throw new NotImplementedException();
             }
             set
             {
                 throw new NotImplementedException();
             }
         }
     }
}
