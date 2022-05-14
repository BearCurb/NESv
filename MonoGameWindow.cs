using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace NESv
{
    public class MonoGameWindow : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private FrameCounter frameCounter = new FrameCounter();

        Bus bus = new Bus();

        private int width;
        private int height;

        DrawHelper drawHelper;

        public MonoGameWindow(int width, int height)
        {
            this.width = width;
            this.height = height;
            IsMouseVisible = true;
            drawHelper = DrawHelper.Instance;
            graphics = new GraphicsDeviceManager(this);
            drawHelper.SetColorBufferSize(width, height);
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            int[] codes = new int[] { 0xA2, 0x0A, 0x8E, 0x00, 0x00, 0xA2, 0x03, 0x8E, 0x01, 0x00, 0xAC, 0x00, 0x00, 0xA9, 0x00, 0x18, 0x6D, 0x01, 0x00, 0x88, 0xD0, 0xFA, 0x8D, 0x02, 0x00, 0xEA, 0xEA, 0xEA };
            int offset = 0X8000;
            foreach (int code in codes)
            {
                bus.Write(offset++, code);
            }
            System.Console.WriteLine(Convert.ToString(offset, 16));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                bus.cpu.Disassemble(0X8000, 0x801c);
            }

            bus.cpu.pc = (new Random().Next() * 10) & 0XFF;
            bus.cpu.x = (new Random().Next() * 10) & 0XFF;
            bus.cpu.y = (new Random().Next() * 10) & 0XFF;
            bus.cpu.a = (new Random().Next() * 10) & 0XFF;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            drawHelper.SpriteBatch = spriteBatch;
            Random random = new Random();
            drawHelper.DrawLine(10, 10, 100, 100, new Color(random.Next() * 255, random.Next() * 255, random.Next() * 255));

            DrawCpu(516, 2);


            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", Math.Floor(frameCounter.AverageFramesPerSecond));
            drawHelper.DrawString(0, 0, fps, Color.Red);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void DrawCpu(int x, int y)
        {
            int size = 30;
            drawHelper.DrawString(x, y, "STATUS:", Color.White, size);

            drawHelper.DrawString(x + 96, y, "N", (bus.cpu.status & (int)Cpu6502.FLAG.N) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 112, y, "V", (bus.cpu.status & (int)Cpu6502.FLAG.V) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 128, y, "-", (bus.cpu.status & (int)Cpu6502.FLAG.U) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 144, y, "B", (bus.cpu.status & (int)Cpu6502.FLAG.B) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 160, y, "D", (bus.cpu.status & (int)Cpu6502.FLAG.D) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 178, y, "I", (bus.cpu.status & (int)Cpu6502.FLAG.I) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 194, y, "Z", (bus.cpu.status & (int)Cpu6502.FLAG.Z) > 0 ? Color.Green : Color.Red, size);
            drawHelper.DrawString(x + 210, y, "C", (bus.cpu.status & (int)Cpu6502.FLAG.C) > 0 ? Color.Green : Color.Red, size);

            drawHelper.DrawString(x, y + 25, "PC: $" + Convert.ToString(bus.cpu.pc, 16), Color.White, size);
            drawHelper.DrawString(x, y + 50, "A:  $" + Convert.ToString(bus.cpu.a, 16), Color.White, size);
            drawHelper.DrawString(x, y + 75, "X:  $" + Convert.ToString(bus.cpu.x, 16), Color.White, size);
            drawHelper.DrawString(x, y + 100, "Y:  $" + Convert.ToString(bus.cpu.y, 16), Color.White, size);
            drawHelper.DrawString(x, y + 125, "Stack P: $" + Convert.ToString(bus.cpu.stkp, 16), Color.White, size);
        }

        public void DrawCode(int x, int y, int lines)
        {

        }
    }
}