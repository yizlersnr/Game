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
            //FontFamily Wendy_One = fonts.Install("WendyOne-Regular.ttf");
            //FontFamily Mona = fonts.Install("Mona-Shark.otf");
            //FontFamily Assassin = fonts.Install("Assassin.ttf");
            FontFamily SinkinSans_300Light = fonts.Install("SinkinSans-300Light.ttf");


            //RenderText(font, "abc", 72);
            //RenderText(font, "ABd", 72);
            //RenderText(fontWoff, "abe", 72);
            //RenderText(fontWoff, "ABf", 72);
            //RenderText(font2, "ov", 72);
            //RenderText(font2, "a\ta", 72);
            //RenderText(font2, "aa\ta", 72);
            //RenderText(font2, "aaa\ta", 72);
            //RenderText(font2, "aaaa\ta", 72);
            //RenderText(font2, "aaaaa\ta", 72);
            //RenderText(font2, "aaaaaa\ta", 72);
            //RenderText(font2, "Hello\nWorld", 72);
            //RenderText(carter, "Hello\0World", 72);
            //RenderText(Wendy_One, "Hello\0World", 72);

            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\tx");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\tx");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\t\tx");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\t\t\tx");

            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 0 }, "Zero\tTab");

            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 0 }, "Zero\tTab");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "One\tTab");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 6 }, "\tTab Then Words");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Tab Then Words");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Words Then Tab\t");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "                 Spaces Then Words");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Words Then Spaces                 ");
            //RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "\naaaabbbbccccddddeeee\n\t\t\t3 tabs\n\t\t\t\t\t5 tabs");

            //RenderText(new Font(SystemFonts.Find("Arial"), 20f, FontStyle.Regular), "á é í ó ú ç ã õ", 200, 50);
            //RenderText(new Font(Wendy_One, 50f, FontStyle.BoldItalic), "67", 400, 100);
            //RenderText(new Font(font2, 10f, FontStyle.Regular), "Hello", 200, 50);
            RenderText(new Font(SinkinSans_300Light, 72f, FontStyle.Italic), name, 320, 320);

            ShowText(name);

            //RenderText(new RendererOptions(SystemFonts.CreateFont("consolas", 72)) { TabWidth = 4 }, "xxxxxxxxxxxxxxxx\n\txxxx\txxxx\n\t\txxxxxxxx\n\t\t\txxxx");

            //BoundingBoxes.Generate("a b c y q G H T", SystemFonts.CreateFont("arial", 40f));

            //TextAlignment.Generate(SystemFonts.CreateFont("arial", 50f));
            //TextAlignmentWrapped.Generate(SystemFonts.CreateFont("arial", 50f));

            //var sb = new StringBuilder();
            //for (char c = 'a'; c <= 'z'; c++)
            //{
            //    sb.Append(c);
            //}
            //for (char c = 'A'; c <= 'Z'; c++)
            //{
            //    sb.Append(c);
            //}
            //for (char c = '0'; c <= '9'; c++)
            //{
            //    sb.Append(c);
            //}
            //string text = sb.ToString();

            //foreach (FontFamily f in fonts.Families)
            //{
            //    RenderText(f, text, 72);
            //}
        }

        public static void RenderText(Font font, string text, int width, int height)
        {
            string path = System.IO.Path.GetInvalidFileNameChars().Aggregate("you", (x, c) => x.Replace($"{c}", "-"));
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            using (var img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.White));

                //for (int v = 0; v < width; v+=5)
                //{
                //    for (int r = 0; r < height; r+=5)
                //    {
                //        img[v, r] = Rgba32.Blue;
                //    }
                //}

                IPathCollection shapes = TextBuilder.GenerateGlyphs(text, new Vector2(5f, 5f), new RendererOptions(font, 60));
                img.Mutate(x => x.Fill(Rgba32.Black, shapes));

                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath + ".png"))
                {
                    img.SaveAsPng(fs);
                }
            }
        }

        public static void RenderText(RendererOptions font, string text)
        {
        //    var builder = new GlyphBuilder();
        //    var renderer = new TextRenderer(builder);
        //    FontRectangle size = TextMeasurer.Measure(text, font);
        //    renderer.RenderText(text, font);

        //    builder.Paths
          //      .SaveImage((int)size.Width + 20, (int)size.Height + 20, font.Font.Name, text + ".png");
        }
        public static void RenderText(FontFamily font, string text, float pointSize = 12)
        {
           // RenderText(new RendererOptions(new Font(font, pointSize), 96) { ApplyKerning = true, WrappingWidth = 340 }, text);
        }

        public void ShowText(string text)
        {
            Console.WriteLine(text);
        }

    }
}
