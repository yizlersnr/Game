using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp2;
using OpenTK;
using OpenTK.Graphics.OpenGL4;


namespace ConsoleApp2
{
    class Light
    {
        private float[] vertices1;
        private uint[] indices;

        private int VertexBufferObject;
        private int _vaoLamp;
        private int ElementBufferObject;
        private Shader _lampShader;
        private Texture texture;
        private Texture texture2;
        public float t, x, y, z;

        private Matrix4 view;
        private Matrix4 projection;

        private readonly string tex;
        public string name;

        public void Load()
        {
            GL.Enable(EnableCap.DepthTest);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices1.Length * sizeof(float), vertices1, BufferUsageHint.StaticDraw);

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            _lampShader = new Shader("light.vert", "shape.frag");
            _lampShader.Use();

            //texture = new Texture();
            //texture.Texture1(tex + ".png");
            //texture.Use();

            //texture2 = new Texture();
            //texture2.Texture1("blue.png");
            //texture2.Use();

            //shader.SetInt("texture1", 0);
            //shader.SetInt("texture2", 1);


            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!

            var vertexLocation = _lampShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            //var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            //GL.EnableVertexAttribArray(texCoordLocation);
            //GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

    }
}
