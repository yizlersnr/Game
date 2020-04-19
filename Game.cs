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
        private readonly Square square2 = new Square();

        private readonly Button[] buttons;
        private readonly Button play1 = new Button(-4.5f, 5.0f,"play");
        private readonly Button play2 = new Button(-4.5f, 3.5f,"options");
        private readonly Button play3 = new Button(-4.5f, 2.0f,"credits");
        private readonly Button play4 = new Button(-4.5f, 0.5f, "quit");

        private readonly Object[] shapes; 
        private readonly Object wave = new Object("wave", "british-flag", new Vector3(3.5f, 0.0f, 0.0f)," s1 ", 50);
        private readonly Object car = new Object("car", "head2", new Vector3(1.5f, 0.0f, 0.0f)," s2 ", 50);

        private Camera camera;
        private Vector2 lastPos;
        private bool firstMove = true;
        private bool focused = true;
        static bool showMenu = true;
        private Vector2 PxPy;
        private Vector2 windowSize;

        private int selected;

        // We create a double to hold how long has passed since the program was opened.
        private double _time;

        //private Matrix4 view; // The view matrix is what you might consider the "camera"
        //private Matrix4 projection;

        private readonly int _width, _height;


        public Game(int width, int height, string title) : base(width, height, new GraphicsMode(32,24,0,16), title)
        {
            _width = width;
            _height = height;

            windowSize = new Vector2(_width, _height);

            buttons = new Button[]{play1, play2, play3, play4};

            shapes = new Object[2];
            

            shapes[0] = wave;
            shapes[1] = car; 


        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.ClearColor(1.2f, 1.3f, 1.3f, 1.0f);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);


            //text.show();


            triangle.load();
            square.load(_width, _height);
            square2.load(_width, _height);


            foreach (Button button in buttons)
            {
                button.Load(windowSize);
            }

            foreach (Object shape in shapes)
            {
                shape.Load();
            }

            //shape.load(_width, _height);
            //shape2.load(_width, _height);


            camera = new Camera(new Vector3(0.0f, 0.0f, 9.0f), _width / _height)
            {
                Fov = 45.0f,
                AspectRatio = _width / _height
            };


            //Code goes here
            //CursorVisible = false;

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //shape.Draw(camera);
            //shape2.Draw(camera);

            foreach (Object shape in shapes)
            {
                shape.Draw(camera);
            }

            //triangle.draw();

            if (showMenu == true)
            {
                foreach (Button button in buttons)
                {
                    button.Draw();
                }
            }

            //play.draw(0.0f);
            square2.Draw(3.6f, 0.765f);

            //square2.update(new Vector3(shapes[selected].x, shapes[selected].y, shapes[selected].z));

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {

            triangle.unload();
            square.unload();
            square2.unload();

            foreach (Button button in buttons)
            {
                button.unload();
            }

            foreach (Object shape in shapes)
            {
                shape.unload();
            }

            //shape.unload();
            //shape2.unload();

            base.OnUnload(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            KeyboardState input = Keyboard.GetState(); 

            if (input.IsKeyDown(Key.Up))
            {
                if (selected < shapes.Length - 1)
                {
                    selected++;
                    Console.WriteLine(selected);
                }
                else
                {
                    selected = shapes.Length - 1;
                }
            }

            if (input.IsKeyDown(Key.Down))
            {
                if (selected >= 1)
                {
                    selected--;
                    Console.WriteLine(selected);
                }
                else
                {
                    selected = 0;
                }
            }

            base.OnKeyDown(e);
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
                shapes[selected].left();
            }

            if (input.IsKeyDown(Key.D))
            {
                shapes[selected].right();
            }

            if (input.IsKeyDown(Key.W))
            {
                shapes[selected].forward();
            }

            if (input.IsKeyDown(Key.S))
            {
                shapes[selected].backward();
            }

            if (input.IsKeyDown(Key.R))
            {
                shapes[selected].Reset();
            }

            if (input.IsKeyDown(Key.N))
            {
                shapes[selected].move(-0.5f);
            }

            if (input.IsKeyDown(Key.M))
            {
                shapes[selected].move(0.5f);
            }

            if (input.IsKeyDown(Key.V))
            {
                showMenu = false;
                CursorVisible = false;
            }

            if (input.IsKeyDown(Key.C))
            {
                showMenu = true;
                CursorVisible = true;
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
                float deltaX = Mouse.GetCursorState().X - lastPos.X;
                float deltaY = Mouse.GetCursorState().Y - lastPos.Y;
                lastPos = new Vector2(Mouse.GetCursorState().X, Mouse.GetCursorState().Y);

                camera.Yaw += deltaX * camera.Sensitivity;
                if (camera.Pitch > 60.0f)
                {
                    camera.Pitch = 60.0f;
                }
                else if (camera.Pitch < -60.0f)
                {
                    camera.Pitch = -60.0f;
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



            square2.move(shapes[selected].name, new Vector3(shapes[selected].x, shapes[selected].y, shapes[selected].z));

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (focused) // check to see if the window is focused  
            {
                //Mouse.SetPosition(X + Width / 2f, Y + Height / 2f);
                PxPy.X = Mouse.GetCursorState().X - X - 7;
                PxPy.Y = Mouse.GetCursorState().Y - Y - 30;
                Console.WriteLine(PxPy.X + "," + PxPy.Y);

                windowSize.X = Width;
                windowSize.Y = Height;

                foreach (Button button in buttons)
                {
                    button.Update(PxPy, windowSize);
                }
            }
            
            base.OnMouseMove(e);
        }

        public static void AwesomeMethod()
        {
            Console.WriteLine("cool");
            showMenu = false;
            //CursorVisible = false;
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            
            foreach (Button button in buttons)
            {
                button.Click(PxPy, windowSize);
            }

            square2.update(new Vector3(shapes[selected].x, shapes[selected].y, shapes[selected].z));

            base.OnMouseDown(e);
        }
        

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            camera.Fov = Mouse.GetState().WheelPrecise;
            Console.WriteLine(camera.Fov);
            base.OnMouseWheel(e);
        }

           

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);

            PxPy.X = Mouse.GetCursorState().X - X - 7;
            PxPy.Y = Mouse.GetCursorState().Y - Y - 30;

            windowSize.X = Width;
            windowSize.Y = Height;

            foreach (Button button in buttons)
            {
                button.Update(PxPy, windowSize);
            }

            base.OnResize(e);
        }

    }
}


