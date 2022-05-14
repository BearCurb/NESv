using System;

namespace NESv
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            StartMonoGameWindow();
        }

        static void StartMonoGameWindow()
        {
            using (MonoGameWindow window = new MonoGameWindow(800, 600))
            {
                window.Run();
            }
        }
    }
}
