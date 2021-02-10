using System;
using System.Windows.Forms;

namespace Engine
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new Example.Example());
        }
    }
}
