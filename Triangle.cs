using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp2
{
    class Triangle
    {
        float[] tvertices = {
            -0.75f, -0.5f, 0.0f, //Bottom-left vertex
            -0.85f, -0.5f, 0.0f, //Bottom-right vertex
            -0.80f,  0.5f, 0.0f  //Top vertex
        };

        private int VertexBufferObject;
        private int VertexArrayObject;
        private Shader shader;

        public Triangle()
        {
        }

        public void load()
        {
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, tvertices.Length * sizeof(float), tvertices, BufferUsageHint.StaticDraw);

            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

        }

        public void unload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            shader.Dispose();
        }

        public void draw()
        {
            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }


    }
}
