﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using physics.Engine;
using physics.Engine.Classes;
using physics.Engine.Classes.ObjectTemplates;
using physics.Engine.Helpers;
using physics.Engine.Structs;


namespace physics
{
    public partial class FormMainWindow : Form
    {
        private readonly Timer _physicsTimer = new Timer();
        private readonly Timer _refreshTimer = new Timer();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private PhysicsSystem _physicsSystem = new PhysicsSystem();
        private bool MOUSE_PRESSED_LEFT = false;
        private bool MOUSE_PRESSED_RIGHT = false;
        private PointF StartPointF { get; set; }
        private PointF EndPointF { get; set; }

        private bool _grabbing;
        private PointF _mousePos;


        private PointF _mouseThen;
        private long _msFrameTime;
        private long _msLastFrame;
        private long _msPerDrawCycle;
        private long _msPhysics;
        private long _msThisFrame;
        private int _radius = 10;

        Font debugFont = new Font(FontFamily.GenericMonospace, 10);
        SolidBrush debugBrush = new SolidBrush(Color.WhiteSmoke);
        private long _frameTime;
        private long _engineTime;

        public FormMainWindow()
        {
            InitializeComponent();
            //_refreshTimer.Enabled = true;
            //_refreshTimer.Interval = 1000 / 60;
            //_refreshTimer.Tick += RefreshTimer_Tick;

            //_physicsTimer.Enabled = true;
            //_physicsTimer.Interval = 1000 / 90;
            //_physicsTimer.Tick += PhysicsTimer_Tick;
            _stopwatch.Start();
        }

        private void FormMainWindow_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Idle += OnApplicationIdle;
            ObjectTemplates.CreateWall(0, 0, 65, GameCanvas.Height);
            ObjectTemplates.CreateWall(GameCanvas.Width - 65, 0, GameCanvas.Width, GameCanvas.Height);
            ObjectTemplates.CreateWall(0, 0, GameCanvas.Width, 65);
            ObjectTemplates.CreateWall(0, GameCanvas.Height - 65, GameCanvas.Width, GameCanvas.Height);

            for (int i = 0; i < 300; i += 10)
            {
                for (int j = 0; j < 200; j += 10)
                {
                    ObjectTemplates.CreateWater(i + 200, j + 150);
                }
            }

            //CreatePhysicsObject(PhysicsObject.Type.Circle, new PointF(200, 150), 10, false);

        }

        private void InvalidateWindow()
        {
            GameCanvas.Refresh();
        }

        private void GameCanvas_DrawGame(object sender, PaintEventArgs e)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;

            e.Graphics.DrawString("ms per physics cycle: " + _msPhysics, debugFont, debugBrush,
                new PointF(80, 70));
            e.Graphics.DrawString("ms total draw time: " + _msPerDrawCycle, debugFont, debugBrush,
                new PointF(80, 90));
            e.Graphics.DrawString("frame rate: " + 1000 / Math.Max(_msFrameTime, 1), debugFont,
                debugBrush, new PointF(80, 110));
            e.Graphics.DrawString("num objects: " + PhysicsSystem.ListStaticObjects.Count, debugFont,
                debugBrush, new PointF(80, 130));
            if (_grabbing)
            {
                e.Graphics.DrawLine(new Pen(Color.DarkGreen), _mousePos, _physicsSystem.GetActiveObjectCenter());
            }

            if (MOUSE_PRESSED_LEFT)
            {
                var penArrow = new Pen(Color.Green,2);
                penArrow.EndCap = LineCap.ArrowAnchor;
                penArrow.StartCap = LineCap.Round;
                e.Graphics.DrawLine(penArrow, _mousePos, StartPointF);
                e.Graphics.DrawEllipse(new Pen(Color.DarkBlue), (int)(StartPointF.X-_radius), (int)(StartPointF.Y-_radius), _radius*2, _radius*2);
            }

            foreach (var o in PhysicsSystem.ListStaticObjects)
            {
                o.Shader.PreDraw(o, e.Graphics);
            }
            foreach (var o in PhysicsSystem.ListStaticObjects)
            {
                o.Shader.Draw(o, e.Graphics);
            }
            foreach (var o in PhysicsSystem.ListStaticObjects)
            {
                o.Shader.PostDraw(o, e.Graphics);
            }

        }


        #region EngineBindings
        private void DragObject(PointF location)
        {
            _physicsSystem.MoveActiveTowardsPoint(new Vec2 {X = location.X, Y = location.Y});
        }

        private void stopDragObject()
        {
            _physicsSystem.ReleaseActiveObject();
            _grabbing = false;
        }
        #endregion


        #region MouseEvents
        private void GameCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                if (_physicsSystem.ActivateAtPoint(e.Location))
                {
                    _grabbing = true;
                    return;
                }

                StartPointF = e.Location;
                MOUSE_PRESSED_LEFT = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (_physicsSystem.ActivateAtPoint(e.Location))
                {
                    _physicsSystem.RemoveActiveObject();
                }

                MOUSE_PRESSED_RIGHT = true;
            }
        }

        private void GameCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_grabbing)
                {
                    stopDragObject();
                    return;
                }

                if (MOUSE_PRESSED_LEFT)
                {
                    EndPointF = e.Location;

                    ActionTemplates.launch(
                        _physicsSystem,
                        ObjectTemplates.CreateSmallBall(StartPointF.X, StartPointF.Y),
                        StartPointF,
                        EndPointF);

                    MOUSE_PRESSED_LEFT = false;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                MOUSE_PRESSED_RIGHT = false;
            }
        }

        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePos = e.Location;

            if (MOUSE_PRESSED_RIGHT)
            {
                if (_physicsSystem.ActivateAtPoint(e.Location))
                {
                    _physicsSystem.RemoveActiveObject();
                }
            }
        }
        #endregion


        #region KeyboardEvents
        private void FormMainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                _physicsSystem.FreezeStaticObjects();
            }
            if (e.KeyCode == Keys.W)
            {
                _physicsSystem.FreezeStaticObjects();
            }
            if (e.KeyCode == Keys.P)
            {
                ObjectTemplates.CreateAttractor(_mousePos.X, _mousePos.Y);
            }
        }
        #endregion



        //And the declarations for those two native methods members:        
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [System.Security.SuppressUnmanagedCodeSecurity] // We won’t use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())
            {
                if (_stopwatch.ElapsedMilliseconds - _frameTime > 1000 / 45)
                {
                    _frameTime = _stopwatch.ElapsedMilliseconds;
                    RunRenderLoop();
                }

                if (_stopwatch.ElapsedMilliseconds - _engineTime > 1000 / 100)
                {
                    _engineTime = _stopwatch.ElapsedMilliseconds;
                    RunEngineLoop();
                }
            }
        }
        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }

        private void RunRenderLoop()
        {
            InvalidateWindow();
            _msPerDrawCycle = _stopwatch.ElapsedMilliseconds - _frameTime;
            _msLastFrame = _msThisFrame;
            _msThisFrame = _stopwatch.ElapsedMilliseconds;
            _msFrameTime = _msThisFrame - _msLastFrame;
        }


        private void RunEngineLoop()
        {
            if (_grabbing)
            {
                DragObject(_mousePos);
            }
            _physicsSystem.Tick(_stopwatch.ElapsedMilliseconds);
            _msPhysics = _stopwatch.ElapsedMilliseconds - _engineTime;
        }
    }
}