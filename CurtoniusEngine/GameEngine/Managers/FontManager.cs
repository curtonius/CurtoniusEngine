using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEngine
{
    //BAD DESIGN, WILL WORK ON LATER
    static class FontManager
    {
        public static Dictionary<string, Dictionary<string, Bitmap>> fonts = new Dictionary<string, Dictionary<string, Bitmap>>();
        public static int fontColorsBeforeClear = 10;
        static Bitmap ConvertStringToImage(string txt, string fontname)
        {
            //creating bitmap image
            Bitmap bmp = new Bitmap(1, 1);

            //FromImage method creates a new Graphics from the specified Image.
            Graphics graphics = Graphics.FromImage(bmp);
            // Create the Font object for the image text drawing.
            Font font = new Font(fontname, 64);
            // Instantiating object of Bitmap image again with the correct size for the text and font.
            SizeF stringSize = graphics.MeasureString(txt, font);
            bmp = new Bitmap(bmp, (int)stringSize.Width, (int)stringSize.Height);
            graphics = Graphics.FromImage(bmp);


            //Draw Specified text with specified format 
            graphics.DrawString(txt, font, Brushes.White, 0, 0);
            font.Dispose();
            graphics.Flush();
            graphics.Dispose();

            return bmp;
        }

        public static void CheckForText(string text, string fontFamily)
        {
            if (!fonts.ContainsKey(fontFamily))
            {
                fonts.Add(fontFamily, new Dictionary<string, Bitmap>());
            }

            if (!fonts[fontFamily].ContainsKey(text))
            {
                Bitmap bmp = ConvertStringToImage(text, fontFamily);
                fonts[fontFamily][text] = bmp;
            }
        }

        public static void SetColor(string text, string fontFamily, Color color)
        {
            if(fonts.ContainsKey(fontFamily) && fonts[fontFamily].ContainsKey(text))
            {
                Bitmap srcBitmap = fonts[fontFamily][text];
                for (int i = 0; i < srcBitmap.Width; i++)
                {
                    for (int j = 0; j < srcBitmap.Height; j++)
                    {
                        //get the pixel from the scrBitmap image
                        Color pixel = srcBitmap.GetPixel(i, j);
                        // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                        if (pixel.A > 150)
                            srcBitmap.SetPixel(i, j, color);
                        else
                            srcBitmap.SetPixel(i, j, pixel);
                    }
                }
            }
        }
    }
}
