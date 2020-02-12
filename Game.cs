using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES30;
using OpenTK.Input;

namespace ConsoleApp2
{
    public class Game : GameWindow
    {
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
             0.5f, -0.5f, 0.0f, //Bottom-right vertex
             0.0f,  0.5f, 0.0f  //Top vertex
        };

      
        //private readonly Shader shader;
        private readonly Triangle triangle = new Triangle();
        private readonly Square square = new Square();
        //private readonly Square square2 = new Square();
        private readonly Shape shape = new Shape();

        //private readonly Text text = new Text();
        //private readonly Shape shape1 = new Shape();
        //private readonly Shape shape2 = new Shape(-0.9f, 0.8f, 0.2f, 0.1f);




        // We create a double to hold how long has passed since the program was opened.
        private double _time;

        private Matrix4 view; // The view matrix is what you might consider the "camera"
        private Matrix4 projection;

        private int _width, _height;


        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            _width = width;
            _height = height;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);


            //text.show();


            triangle.load();
            square.load(_width, _height);
            //square2.load(_width, _height);

            shape.load(_width, _height);
            //shape1.load(_width, _height);
            //shape2.load(_width, _height);

            //view = Matrix4.CreateTranslation(0.0f, 5.0f, -3.0f);
            //projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);

            

         

            //Code goes here

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {

            triangle.unload();
            square.unload();
            //square2.unload();

            shape.unload();
            //shape1.unload();
            //shape2.unload();

            //shader.Dispose();
            base.OnUnload(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (input.IsKeyDown(Key.A))
            {
                shape.left();
            }

            if (input.IsKeyDown(Key.D))
            {
                shape.right();
            }

            if (input.IsKeyDown(Key.W))
            {
                shape.forward();
            }

            if (input.IsKeyDown(Key.S))
            {
                shape.backward();
            }

            if (input.IsKeyDown(Key.R))
            {
                shape.Reset();
            }

            if (input.IsKeyDown(Key.N))
            {
                shape.move(-0.5f);
            }

            if (input.IsKeyDown(Key.M))
            {
                shape.move(0.5f);
            }

            if (input.IsKeyDown(Key.B))
            {
                square.update();
            }


            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            

            shape.draw();
            //shape1.draw();
            //shape2.draw();

           // triangle.draw();
            square.draw();
            //square.draw();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }


        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

    }
}


