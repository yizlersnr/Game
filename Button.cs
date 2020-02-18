using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ConsoleApp2
{
    class Button
    {

        private float[] vertices1;
        private uint[] indices;

        private readonly float[] vertices =
        {
            // Position         Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left

            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f,  // top left
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
        };

        private readonly uint[] indices1 =
        {

            3, 4, 5,
            0, 1, 2

        };


        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private Shader shader;
        //private Texture temp = new Texture();
        private Texture texture;
        private Texture texture2;
        public float t, x, y, z, Bx, By;
        public float btnTop, btnBottom, btnLeft, btnRight;
        public string title;

        private Matrix4 view;
        private Matrix4 projection;
        private readonly Text info = new Text();
        public string texto;
        public Vector2 currentPos = Vector2.Zero;

        public Button(float x, float y, string name)
        {

            //ObjLoader obj1 = new ObjLoader("object3.txt");
            //vertices1 = obj1.Verts();
            //indices = obj1.index();

            vertices1 = vertices;
            indices = indices1;

            Bx = x;
            By = y;
            title = name;

        }

        public void Load(Vector2 windowSize)
        {
            float width = windowSize.X; 
            float height =  windowSize.Y;

            GL.Enable(EnableCap.DepthTest);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices1.Length * sizeof(float), vertices1, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader("shape.vert", "shape.frag");
            shader.Use();


            //Text info = new Text();
            info.show("Menu");

            texture = new Texture();
            texture.Texture1("Output/you.png");
            texture.Use();

            texture2 = new Texture();
            texture2.Texture1("blue.png");
            texture2.Use();

            shader.SetInt("texture1", 0);
            shader.SetInt("texture2", 1);


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);


            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            view = Matrix4.CreateTranslation(0.0f, 0.0f, -7.9f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), width / (float)height, 0.1f, 100.0f);

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -66.9f);
            projection = Matrix4.CreateOrthographic(width, height,0.1f,100.0f);

        }

        public void unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            shader.Dispose();
        }

        public void Draw()
        {
    
            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);
            shader.Use();


            var model = Matrix4.Identity * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180f));
            model *= Matrix4.CreateTranslation(Bx * 1.0f, By * 1.0f, z * 1.0f);
            model *= Matrix4.CreateScale(100.0f,50.0f,1.0f);


            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, (indices.Length / 5));

        }




        public void update(Vector2 pos, Vector2 windowSize)
        {
            Console.WriteLine("Bx,By " + Bx + ","+ By);

            btnTop = (windowSize.Y / 2 - 25); 
            btnBottom = (windowSize.Y / 2 + 25);
            btnLeft = (windowSize.X / 2 - 50);
            btnRight = (windowSize.X / 2 + 50);

            Console.WriteLine("Windosize " + windowSize);

            if (pos.X > (Bx * windowSize.X/12 + btnLeft) && pos.X < (Bx * windowSize.X / 12 + btnRight) && pos.Y > (btnTop - By * windowSize.Y / 12) && pos.Y < (btnBottom - By * windowSize.Y / 12))
            {
                //texto = "Quit"; // pos.ToString();
                //info.show(texto);
                texture.Texture1("Output/" + title + "2.png");
                texture.Use();
                shader.SetInt("texture1", 0);

                currentPos = pos;
            }
            else
            {
                //texto = "not"; // pos.ToString();
                //info.show(texto);
                texture.Texture1("Output/" + title + ".png");
                texture.Use();
                shader.SetInt("texture1", 0);
            }
        }

        public void Reset()
        {
            x = 0;
            y = 0;
        }

        public void left()
        {
            x -= 0.1f;
        }

        public void right()
        {
            x += 0.1f;
        }

        public void forward()
        {
            z -= 0.1f;
        }

        public void backward()
        {
            z += 0.1f;
        }

        public void move(float x)
        {
            t += 1;

            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);
            shader.Use();

            var transform = Matrix4.Identity;

            //transform *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(t * 1.0f));
            //transform *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(20f));
            //transform *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(t * 1.0f));
            transform *= Matrix4.CreateScale(0.65f);
            transform *= Matrix4.CreateTranslation(x, 0.0f, 0.0f);

            shader.SetMatrix4("transform", transform);

            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        }


    }
}
