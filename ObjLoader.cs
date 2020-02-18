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
    class ObjLoader
    {
      
        readonly string text;
        readonly string line;
        readonly string currentLine;

        int count;

        private float[] vertices = new float[0];
        readonly float[] texture = new float[0];
        readonly float[] normals = new float[0];

        readonly float[] faces = new float[0];
        public float[] model = new float[0];
        private uint[] indices = new uint[0];
        


        public ObjLoader(string path)
        {
            text = File.ReadAllText(path);
            string[] lines = File.ReadAllLines(path);
            if (text == null)
            {
                Console.WriteLine("Imposible to open the file");
            }
            else
            {
                Console.Write("read and open");
            }

            
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                currentLine = (line[0] + "" + line[1]).ToString();
                string[] vertice_split = line.Split(new Char[] { ' ' });
                if (currentLine == "v ")
                {
                    Array.Resize(ref vertices, vertices.Length + 3);
                    vertices[vertices.Length - 3] = (float)Convert.ToDouble(vertice_split[1]);
                    vertices[vertices.Length - 2] = (float)Convert.ToDouble(vertice_split[2]);
                    vertices[vertices.Length - 1] = (float)Convert.ToDouble(vertice_split[3]);
                }
            
                string[] texture_split = line.Split(new Char[] { ' ' });
                if (currentLine == "vt")
                {
                    Array.Resize(ref texture, texture.Length + 2);
                    texture[texture.Length - 2] = (float)Convert.ToDouble(texture_split[1]);
                    texture[texture.Length - 1] = (float)Convert.ToDouble(texture_split[2 ]);
                }

                string[] normal_split = line.Split(new Char[] { ' ' });
                if (currentLine == "vn")
                {
                    Array.Resize(ref normals, normals.Length + 3);
                    normals[normals.Length - 3] = (float)Convert.ToDouble(normal_split[1]);
                    normals[normals.Length - 2] = (float)Convert.ToDouble(normal_split[2]);
                    normals[normals.Length - 1] = (float)Convert.ToDouble(normal_split[3]);
                }


                string[] face_split = line.Split(new Char[] { ' ', '/' });
                if (currentLine == "f ")
                {
                    Array.Resize(ref faces, faces.Length + 9);
                    faces[faces.Length - 9] = (int)Convert.ToInt32(face_split[1]);
                    faces[faces.Length - 8] = (int)Convert.ToInt32(face_split[2]);
                    faces[faces.Length - 7] = (int)Convert.ToInt32(face_split[3]);

                            Array.Resize(ref model, model.Length + 5);
                            model[model.Length - 5] = vertices[((int)faces[faces.Length - 9] * 3) - 3];
                            model[model.Length - 4] = vertices[((int)faces[faces.Length - 9] * 3) - 2];
                            model[model.Length - 3] = vertices[((int)faces[faces.Length - 9] * 3) - 1];

                            model[model.Length - 2] = texture[((int)faces[faces.Length - 8] * 2) - 2];
                            model[model.Length - 1] = texture[((int)faces[faces.Length - 8] * 2) - 1];

                    faces[faces.Length - 6] = (int)Convert.ToInt32(face_split[4]);
                    faces[faces.Length - 5] = (int)Convert.ToInt32(face_split[5]);
                    faces[faces.Length - 4] = (int)Convert.ToInt32(face_split[6]);

                            Array.Resize(ref model, model.Length + 5);
                            model[model.Length - 5] = vertices[((int)faces[faces.Length - 6] * 3) - 3];
                            model[model.Length - 4] = vertices[((int)faces[faces.Length - 6] * 3) - 2];
                            model[model.Length - 3] = vertices[((int)faces[faces.Length - 6] * 3) - 1];

                            model[model.Length - 2] = texture[((int)faces[faces.Length - 5] * 2) - 2];
                            model[model.Length - 1] = texture[((int)faces[faces.Length - 5] * 2) - 1];

                    faces[faces.Length - 3] = (int)Convert.ToInt32(face_split[7]);
                    faces[faces.Length - 2] = (int)Convert.ToInt32(face_split[8]);
                    faces[faces.Length - 1] = (int)Convert.ToInt32(face_split[9]);

                            Array.Resize(ref model, model.Length + 5);
                            model[model.Length - 5] = vertices[((int)faces[faces.Length - 3] * 3) - 3];
                            model[model.Length - 4] = vertices[((int)faces[faces.Length - 3] * 3) - 2];
                            model[model.Length - 3] = vertices[((int)faces[faces.Length - 3] * 3) - 1];

                            model[model.Length - 2] = texture[((int)faces[faces.Length - 2] * 2) - 2];
                            model[model.Length - 1] = texture[((int)faces[faces.Length - 2] * 2) - 1];
                    count++;
                }

            }

            for(uint t = 0;t < count * 3; t++)
            {
                Array.Resize(ref indices, indices.Length + 1);
                indices[indices.Length - 1] = t;
            }

        }

        public float[] Verts()
        {
            return model;
        }

        public uint[] index()
        {
            return indices;
        }

    }
}
