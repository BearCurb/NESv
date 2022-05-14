using System;
using System.Drawing;

namespace NESv
{
    public class FontHelp
    {
        public static FontColor[,] FromFontToBitMap(string text, Microsoft.Xna.Framework.Color color, int size)
        {
            Graphics g = Graphics.FromImage(new Bitmap(1, 1));
            Font font = new Font("Silver", size);
            // Font font = new Font("Arial", size);
            SizeF sizeF = g.MeasureString(text, font);
            Brush brush = Brushes.Lime;
            PointF pf = new PointF(0, 0);
            Bitmap img = new Bitmap(Convert.ToInt32(sizeF.Width), Convert.ToInt32(sizeF.Height));
            g = Graphics.FromImage(img);
            g.DrawString(text, font, brush, pf);

            FontColor[,] imgCorlors = new FontColor[img.Width, img.Height];
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    Color currentColor = img.GetPixel(i, j);
                    if (currentColor.R != 0 || currentColor.G != 0 || currentColor.B != 0)
                    {
                        imgCorlors[i, j] = new FontColor(color.R, color.G, color.B, color.A);
                    }
                }
            }

            return imgCorlors;
        }
    }

    public readonly struct FontColor
    {
        public readonly int R;
        public readonly int G;
        public readonly int B;
        public readonly int A;
        public FontColor(int r, int g, int b, int a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}