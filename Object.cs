using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ConsoleApp2
{
    class Object
    {
        private float[] vertices1;
        private uint[] indices;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private Shader shader;
        private Texture texture;
        private Texture texture2;
        public float t,x,y,z;

        private Matrix4 view;
        private Matrix4 projection;

        private readonly string tex;
        public string name;

        public Object(string obj, string _tex, Vector3 pos, string shapeName)
        {
            ObjLoader obj1 = new ObjLoader(obj + ".txt");
            vertices1 = obj1.Verts();
            indices = obj1.index();
            tex = _tex;
            x = pos.X;
            y = pos.Y;
            z = pos.Z;
            name = shapeName;
        }

        public void Load()
        {
            GL.Enable(EnableCap.DepthTest);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices1.Length * sizeof(float), vertices1, BufferUsageHint.StaticDraw);

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
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
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
