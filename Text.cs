using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Processing.Drawing;
//using global::DrawWithImageSharp;
//using SixLabors.Shapes.Temp;
//using Shapes;
using SixLabors.Fonts;
using System.IO;
using System.Numerics;
using SixLabors.Shapes;

namespace ConsoleApp2
{
    class Text
    {
        public Text()
        {
            
        }

        public void Show(string name)
        {

            FontCollection fonts = new FontCollection();
            //FontFamily font = fonts.Install(@"SixLaborsSampleAB.ttf");
            //FontFamily fontWoff = fonts.Install(@"SixLaborsSampleAB.woff");
            //FontFamily font2 = fonts.Install("OpenSans-Regular.ttf");
            //FontFamily carter = fonts.Install("CarterOne.ttf");
            FontFamily Wendy_One = fonts.Install("OpenSans-Regular.ttf"); //fonts.Install("WendyOne-Regular.ttf");


            //RenderText(new Font(SystemFonts.Find("Arial"), 20f, FontStyle.Regular), "á é í ó ú ç ã õ", 200, 50);
            //RenderText(new Font(Wendy_One, 50f, FontStyle.BoldItalic), "67", 400, 100);
            //RenderText(new Font(font2, 10f, FontStyle.Regular), "Hello", 200, 50);
            RenderText(new Font(Wendy_One, 30f, FontStyle.Italic), name, 320, 80);
        }

        public static void RenderText(Font font, string text, int width, int height)
        {
            string path = System.IO.Path.GetInvalidFileNameChars().Aggregate("you", (x, c) => x.Replace($"{c}", "-"));
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            using (var img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.Black));

                IPathCollection shapes = TextBuilder.GenerateGlyphs(text, new Vector2(50f, 4f), new RendererOptions(font, 72));
                img.Mutate(x => x.Fill(Rgba32.Red, shapes));

                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath + ".png"))
                {
                    img.SaveAsPng(fs);
                }
            }
        }
    }
}
