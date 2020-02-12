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
        private readonly ScreenText score = new ScreenText();
        private readonly Shape shape = new Shape();

        private Camera camera;
        private Vector2 lastPos;
        private bool firstMove = true;
        private bool focused = true;

        private readonly Text text = new Text();
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


            text.Show("Money");


            triangle.load();
            square.load(_width, _height);
            score.load(_width, _height);

            shape.load(_width, _height);


            camera = new Camera(new Vector3(0.0f, 0.0f, 9.0f), _width / _height)
            {
                Fov = 45.0f,
                AspectRatio = _width / _height
            };

            //shape1.load(_width, _height);
            //shape2.load(_width, _height);

            view = Matrix4.CreateTranslation(0.0f, 5.0f, -3.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);



            //Code goes here
            CursorVisible = false;

            base.OnLoad(e);
        }

        protected override void OnUnload(EventArgs e)
        {

            triangle.unload();
            square.unload();
            score.unload();

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
                square.Update(camera.Position);
            }


            // Camera
            // Keyboard

            float speed = 155.0f;

            if (camera.Position.Y < 12.0f)
            {
                //camera.Position += new Vector3(0.0f,12.0f,0.0f);
            }

            if (input.IsKeyDown(Key.I))
            {
                camera.Position += camera.Front * speed * (float)e.Time; //Forward 
            }

            if (input.IsKeyDown(Key.K))
            {
                camera.Position -= camera.Front * speed * (float)e.Time; //Backwards
            }

            if (input.IsKeyDown(Key.J))
            {
                camera.Position -= Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * speed * (float)e.Time; //Left
            }

            if (input.IsKeyDown(Key.L))
            {
                camera.Position += Vector3.Normalize(Vector3.Cross(camera.Front, camera.Up)) * speed * (float)e.Time; //Right
            }

            if (input.IsKeyDown(Key.Space))
            {
                camera.Position += camera.Up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Key.LShift))
            {
                camera.Position -= camera.Up * speed * (float)e.Time; //Down
            }

        

            // Camera
            // Mouse

            if (firstMove)
            {
                lastPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                firstMove = false;
            }
            else
            {
                float deltaX = Mouse.GetState().X - lastPos.X;
                float deltaY = Mouse.GetState().Y - lastPos.Y;
                lastPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                camera.Yaw += deltaX * camera.Sensitivity;
                if (camera.Pitch > 89.0f)
                {
                    camera.Pitch = 89.0f;
                }
                else if (camera.Pitch < -89.0f)
                {
                    camera.Pitch = -89.0f;
                }
                else
                {
                    camera.Pitch -= deltaY * camera.Sensitivity;
                }
            }

            camera._front.X = (float)Math.Cos(MathHelper.DegreesToRadians(camera.Pitch)) * (float)Math.Cos(MathHelper.DegreesToRadians(camera.Yaw));
            camera._front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(camera.Pitch));
            camera._front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(camera.Pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(camera.Yaw));
            camera._front = Vector3.Normalize(camera._front);

           

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (focused) // check to see if the window is focused  
            {
                Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {

            camera.Fov = Mouse.GetState().WheelPrecise;
            Console.WriteLine(camera.Fov);

            base.OnMouseWheel(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            

            shape.Draw(camera);
            //shape1.draw();
            //shape2.draw();

            //triangle.draw();
            square.Draw(-0.6f,camera);
            score.DrawText(0.8f,0.8f);

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


