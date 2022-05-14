using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NESv
{
    public class DrawHelper
    {
        public SpriteBatch SpriteBatch { private get; set; }
        private Texture2D texture;
        private Color[,] colorsBuffer;
        private int width;
        private int height;
        private static DrawHelper instance;
        public static DrawHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new DrawHelper();
                return instance;
            }
        }

        public void SetColorBufferSize(int width, int height)
        {
            colorsBuffer = new Color[width, height];
            this.width = width;
            this.height = height;
        }

        public void DrawPixel(int x, int y, Color pixelColor)
        {
            SpriteBatch.Draw(GetPixel(), new Vector2(x, y), pixelColor);
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color pixelColor)
        {
            DrawLine((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, pixelColor);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color pixelColor)
        {
            int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            dx = x2 - x1; dy = y2 - y1;

            Vector2 p1 = new Vector2(x1, y1); Vector2 p2 = new Vector2(x2, y2);
            //	return;
            x1 = (int)p1.X; y1 = (int)p1.Y;
            x2 = (int)p2.X; y2 = (int)p2.Y;
            if (dx == 0)
            {
                if (y2 < y1) swap(ref y1, ref y2);
                for (y = y1; y <= y2; y++)
                    DrawPixel(x1, y, pixelColor);
                return;
            }

            if (dy == 0)
            {
                if (x2 < x1) swap(ref x1, ref x2);
                for (x = x1; x <= x2; x++)
                    DrawPixel(x, y1, pixelColor);
                return;
            }
            dx1 = Math.Abs(dx); dy1 = Math.Abs(dy);
            px = 2 * dy1 - dx1; py = 2 * dx1 - dy1;
            if (dy1 <= dx1)
            {
                if (dx >= 0)
                {
                    x = x1; y = y1; xe = x2;
                }
                else
                {
                    x = x2; y = y2; xe = x1;
                }
                DrawPixel(x, y, pixelColor);

                for (i = 0; x < xe; i++)
                {
                    x = x + 1;
                    if (px < 0)
                        px = px + 2 * dy1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) y = y + 1; else y = y - 1;
                        px = px + 2 * (dy1 - dx1);
                    }
                    DrawPixel(x, y, pixelColor);
                }
            }
            else
            {
                if (dy >= 0)
                {
                    x = x1; y = y1; ye = y2;
                }
                else
                {
                    x = x2; y = y2; ye = y1;
                }
                DrawPixel(x, y, pixelColor);

                for (i = 0; y < ye; i++)
                {
                    y = y + 1;
                    if (py <= 0)
                        py = py + 2 * dx1;
                    else
                    {
                        if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) x = x + 1; else x = x - 1;
                        py = py + 2 * (dx1 - dy1);
                    }
                    DrawPixel(x, y, pixelColor);
                }
            }
        }

        public void DrawCricle(Vector2 p, int radius, Color pixelColor)
        {
            DrawCricle((int)p.X, (int)p.Y, radius, pixelColor);
        }

        public void DrawCricle(int x, int y, int radius, Color pixelColor)
        {
            if (radius < 0 || x < -radius || y < -radius || x - width > radius || y - height > radius)
                return;

            if (radius > 0)
            {
                int x0 = 0;
                int y0 = radius;
                int d = 3 - 2 * radius;

                while (y0 >= x0)
                {
                    DrawPixel(x + x0, y - y0, pixelColor);
                    DrawPixel(x + y0, y + x0, pixelColor);
                    DrawPixel(x - x0, y + y0, pixelColor);
                    DrawPixel(x - y0, y - x0, pixelColor);
                    if (x0 != 0 && x0 != y0)
                    {
                        DrawPixel(x + y0, y - x0, pixelColor);
                        DrawPixel(x + x0, y + y0, pixelColor);
                        DrawPixel(x - y0, y + x0, pixelColor);
                        DrawPixel(x - x0, y - y0, pixelColor);
                    }
                    if (d < 0)
                        d += 4 * x0++ + 6;
                    else
                        d += 4 * (x0++ - y0--) + 10;
                }
            }
            else
                DrawPixel(x, y, pixelColor);
        }

        public void FillCircle(Vector2 p, int radius, Color pixelColor)
        {
            FillCircle((int)p.X, (int)p.Y, radius, pixelColor);
        }

        public void FillCircle(int x, int y, int radius, Color pixelColor)
        {
            if (radius < 0 || x < -radius || y < -radius || x - width > radius || y - height > radius)
                return;

            if (radius > 0)
            {
                int x0 = 0;
                int y0 = radius;
                int d = 3 - 2 * radius;
                void DrawLine(int sx, int ex, int y)
                {
                    for (int x = sx; x <= ex; x++)
                        DrawPixel(x, y, pixelColor);
                }
                while (y0 >= x0)
                {
                    DrawLine(x - y0, x + y0, y - x0);
                    if (x0 > 0) DrawLine(x - y0, x + y0, y + x0);

                    if (d < 0)
                        d += 4 * x0++ + 6;
                    else
                    {
                        if (x0 != y0)
                        {
                            DrawLine(x - x0, x + x0, y - y0);
                            DrawLine(x - x0, x + x0, y + y0);
                        }
                        d += 4 * (x0++ - y0--) + 10;
                    }
                }
            }
            else
                DrawPixel(x, y, pixelColor);
        }

        public void DrawRect(Vector2 p, int w, int h, Color pixelColor)
        {
            DrawRect((int)p.X, (int)p.Y, w, h, pixelColor);
        }

        public void DrawRect(int x, int y, int w, int h, Color pixelColor)
        {
            DrawLine(x, y, x + w, y, pixelColor);
            DrawLine(x + w, y, x + w, y + h, pixelColor);
            DrawLine(x + w, y + h, x, y + h, pixelColor);
            DrawLine(x, y + h, x, y, pixelColor);
        }

        public void FillRect(Vector2 p, int w, int h, Color pixelColor)
        {
            FillRect((int)p.X, (int)p.Y, w, h, pixelColor);
        }

        public void FillRect(int x, int y, int w, int h, Color pixelColor)
        {
            int x2 = x + w;
            int y2 = y + h;

            if (x < 0) x = 0;
            if (x >= width) x = width;
            if (y < 0) y = 0;
            if (y >= height) y = height;

            if (x2 < 0) x2 = 0;
            if (x2 >= width) x2 = width;
            if (y2 < 0) y2 = 0;
            if (y2 >= height) y2 = height;

            for (int i = x; i < x2; i++)
                for (int j = y; j < y2; j++)
                    DrawPixel(i, j, pixelColor);
        }

        public void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Color pixelColor)
        {
            DrawTriangle((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y, (int)p3.X, (int)p3.Y, pixelColor);
        }

        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Color pixelColor)
        {
            DrawLine(x1, y1, x2, y2, pixelColor);
            DrawLine(x2, y2, x3, y3, pixelColor);
            DrawLine(x3, y3, x1, y1, pixelColor);
        }

        public void DrawString(int x, int y, String text, Color pixelColor, int size = 30)
        {
            FontColor[,] colors = FontHelp.FromFontToBitMap(text, pixelColor, size);
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    int resultX = x + i;
                    int resultY = y + j;
                    if (colors[i, j].R != 0 || colors[i, j].G != 0 || colors[i, j].B != 0)
                    {
                        Color resultColor = new Color(colors[i, j].R, colors[i, j].G, colors[i, j].B, colors[i, j].A);
                        DrawPixel(resultX, resultY, resultColor);
                    }
                }
            }
        }

        private void swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public Texture2D GetPixel()
        {
            if (texture == null)
            {
                texture = new Texture2D(SpriteBatch.GraphicsDevice, 1, 1);
                texture.SetData(new[] { Color.White });
            }
            return texture;
        }
    }
}