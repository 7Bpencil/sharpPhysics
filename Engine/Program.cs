﻿using System;
using System.Windows.Forms;
using Engine.Game;

namespace Engine
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.Run(new MainForm());
        }
    }
}
