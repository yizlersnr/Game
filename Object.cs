using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ConsoleApp2
{
    class Object
    {
        private float[] vertices1;
        private float[] vertices2;
        private float[] frames;
        int[] list1 = new int[4] { 1, 2, 3, 4 };
        int[] list2 = new int[4] { 5, 6, 7, 8 };
        int[] list3 = new int[4] { 1, 3, 2, 1 };
        int[] list4 = new int[4] { 5, 4, 3, 2 };


        private uint[] indices;
        private uint[] indices1;
        private uint[] indices2;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private Shader shader;
        private Texture texture;
        private Texture texture2;
        public float t, x, y, z;
        public int s = -1440;

        private Matrix4 view;
        private Matrix4 projection;

        private readonly string tex;
        public string name;

        public Object(string obj, string _tex, Vector3 pos, string shapeName)
        {
            ObjLoader obj1 = new ObjLoader("animate_50.obj");
            vertices1 = obj1.Verts();
            indices1 = obj1.index();
            tex = _tex;
            x = pos.X;
            y = pos.Y;
            z = pos.Z;
            name = shapeName;

            // if (shapeName == "ani")
            //
            ObjLoader obj2 = new ObjLoader("animate_100.obj");
            vertices2 = obj2.Verts();
            indices2 = obj1.index();
            //}

            //frames = new float[][] { vertices1, vertices2};
            frames = new float[vertices1.Length + vertices2.Length];
            Array.Copy(vertices1, frames, vertices1.Length);
            Array.Copy(vertices2, 0, frames, vertices1.Length, vertices2.Length);


            indices2 = new uint[36] { 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71 };
            indices = new uint[indices1.Length + indices2.Length];
            Array.Copy(indices1, indices, indices1.Length);
            Array.Copy(indices2, 0, indices, indices1.Length, indices2.Length);
        }

        public void Load()
        {
            GL.Enable(EnableCap.DepthTest);

            //VertexBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //GL.BufferData(BufferTarget.ArrayBuffer, vertices1.Length * sizeof(float), vertices1, BufferUsageHint.StaticDraw);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, frames.Length * sizeof(float), frames, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            shader = new Shader("shape.vert", "shape.frag");
            shader.Use();

            texture = new Texture();
            texture.Texture1(tex + ".png");
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
        }

        public void unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            shader.Dispose();
        }

        public void Draw(Camera cam)
        {
            t += 21;


            texture.Use(TextureUnit.Texture0);
            texture2.Use(TextureUnit.Texture1);
            shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(t * 0.05f));
            model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(t * 0.05f));
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(t * 0.05f));
            model *= Matrix4.CreateTranslation(x * 1.0f, y * 0.5f, z * 1.0f);

            view = cam.GetViewMatrix();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(cam.Fov), cam.AspectRatio, 0.1f, 5000.0f);


            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);


            GL.BindVertexArray(VertexArrayObject);
            if (s==0) { 
                GL.DrawElements(PrimitiveType.Triangles, indices2.Length, DrawElementsType.UnsignedInt, 0);
                s = 1;
            }else{
                GL.DrawElements(PrimitiveType.Triangles, indices2.Length, DrawElementsType.UnsignedInt, 144);
                s = 0;
            }
        }

        public void Reset()
        {
           x = 0;
            y = 0;
        }

        public void left()
        {
            x-=0.1f;
        }

        public void right()
        {
            x+=0.1f;
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
