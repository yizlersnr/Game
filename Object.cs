﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Moq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace ConsoleApp2
{
    class Object
    {
        private float[] vertices;
        private float[] vertices2;
        private float[] frames;
        
        private uint[] indices;
        private uint[] indicesCount;
        private uint[] indices2;

        private int VertexBufferObject;
        private int VertexArrayObject;
        private int ElementBufferObject;
        private Shader shader;
        private Texture texture;
        private Texture texture2;
        public float t, x, y, z;
        public int s, icount, fcount = -1440;

        private Matrix4 view;
        private Matrix4 projection;

        private Vector3 objectColor = new Vector3(1.0f, 0.0f, 0.5f); //The color of the object.
        private Vector3 lightColor = new Vector3(1, 1, 1); //The color of the light.
        private Vector3 lightPos = new Vector3(0,0,2);
        private Vector3 viewPos = new Vector3(0, 0, 1);

        private readonly string tex;
        public string name;

        public Object(string obj, string _tex, Vector3 pos, string shapeName)
        {
            ObjLoader obj1 = new ObjLoader(obj + "_000001.obj");
            vertices = obj1.Verts();
            indicesCount = obj1.index();
            tex = _tex;
            x = pos.X;
            y = pos.Y;
            z = pos.Z;
            name = shapeName;


            frames = new float[vertices.Length * 50];
            Array.Copy(vertices, frames, vertices.Length);
            for (s = 2; s < 50; s++)
            {
                ObjLoader objf = new ObjLoader(obj + "_" + s.ToString("D6") + ".obj");
                Console.WriteLine("Read and opened frame "+ s.ToString());
                vertices2 = objf.Verts();
                Array.Copy(vertices2, 0, frames, vertices.Length * (s), vertices2.Length);
            }

            s = 0;

            icount = indicesCount.Length * 50;
            indices = new uint[icount];
            for (uint x = 0; x < icount; x++) {
                indices[x] = x;
            }
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

            shader.SetVector3("objectColor", objectColor);
            shader.SetVector3("lightColor", lightColor);
            shader.SetVector3("lightPos", lightPos);
            shader.SetVector3("viewPos", viewPos);


            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));

            var normlLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normlLocation);
            GL.VertexAttribPointer(normlLocation, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 5 * sizeof(float));

            var colourLocation = shader.GetAttribLocation("aColour");
            GL.EnableVertexAttribArray(colourLocation);
            GL.VertexAttribPointer(colourLocation, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 8 * sizeof(float));
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
            //model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(t * 0.05f));
            //model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(t * 0.05f));
            model *= Matrix4.CreateTranslation(x * 1.0f, y * 0.5f, z * 1.0f);

            view = cam.GetViewMatrix();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(cam.Fov), cam.AspectRatio, 0.1f, 5000.0f);


            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);


            GL.BindVertexArray(VertexArrayObject);
            
                
                GL.DrawElements(PrimitiveType.Triangles, indices.Length/50, DrawElementsType.UnsignedInt, s);
       
          
            s += indicesCount.Length * 4;

            Thread.Sleep(1);

            if(s > (indicesCount.Length * 4 * 50)){ s = 0; }
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
