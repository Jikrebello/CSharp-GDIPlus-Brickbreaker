using System;
using System.Windows.Forms;

namespace Pong
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DateTime currentTime;
            DateTime pastTime;
            TimeSpan deltaTime;
            _ = DateTime.Now;
            pastTime = DateTime.Now;

            Form1 form = new Form1();
            form.Show();

            while (form.Created == true)
            {
                currentTime = DateTime.Now;
                deltaTime = currentTime - pastTime;
                if (deltaTime.TotalMilliseconds > 10)
                {
                    Application.DoEvents();
                    form.Loop();
                    form.Refresh();
                    pastTime = DateTime.Now;
                }
            }
        }
    }
}
