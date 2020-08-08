using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Physics;
using Engine.Physics.Classes;
using Engine.Physics.Classes.Templates;
using Engine.Physics.Structs;

namespace Engine
{
    public class Example : Form
    {
        private Bitmap bmpBuffer;
        private Graphics gfxBuffer;
        private Brush fillRectangleBrush;
        private Brush fillCircleBrush;

        private PhysicsSystem physicsSystem;

        private PhysicsObject player;

        private KeyState keys;

        private struct KeyState
        {
            public bool W, A, S, D;
        }

        public Example()
        {
            Size = new Size(800, 700);

            DoubleBuffered = true;
            bmpBuffer = new Bitmap(Size.Width, Size.Height);
            gfxBuffer = Graphics.FromImage(bmpBuffer);
            fillRectangleBrush = new SolidBrush(Color.Coral);
            fillCircleBrush = new SolidBrush(Color.Silver);
            
            keys = new KeyState();

            physicsSystem = new PhysicsSystem();
            AddObjects();

            var timer = new Timer();
            timer.Interval = 16;
            timer.Tick += GameLoop;
            timer.Start();
        }

        private void AddObjects()
        {
            ObjectTemplates.CreateWall(Vector2.Zero, new Vector2(65, Height));
            ObjectTemplates.CreateWall(new Vector2(Width - 65, 0), new Vector2(Width, Height));
            ObjectTemplates.CreateWall(Vector2.Zero, new Vector2(Width, 65));
            ObjectTemplates.CreateWall(new Vector2(0, Height - 65), new Vector2(Width, Height));
            
            ObjectTemplates.CreateWall(new Vector2(150, 400), new Vector2(300, 500));
            ObjectTemplates.CreateWall(new Vector2(500, 400), new Vector2(650, 500));

            for (var i = 0; i < 400; i += 20) {
                for (var j = 0; j < 100; j += 20) {
                    ObjectTemplates.CreateSmallBall(new Vector2(i + 200, j + 150));
                }
            }

            player = ObjectTemplates.CreateMedBall(new Vector2(300, 100));
            ObjectTemplates.CreateMedBall(new Vector2(500, 100));

            ObjectTemplates.CreateAttractor(new Vector2(400, 450));
        }
		
        private void GameLoop(object sender, EventArgs args)
        {
            var moveSpeed = 32 * 10;
            var velocity = Vector2.Zero;
            if (keys.W) velocity.Y -= moveSpeed;
            if (keys.A) velocity.X -= moveSpeed;
            if (keys.S) velocity.Y += moveSpeed;
            if (keys.D) velocity.X += moveSpeed;

            player.Velocity = velocity;
            
            physicsSystem.Tick(16 / 1000f);
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            Render();
            e.Graphics.DrawImage(bmpBuffer, Point.Empty);
        }
		
        private void Render()
        {
            gfxBuffer.Clear(Color.Black);
            foreach (var o in PhysicsSystem.ListStaticObjects)
            {
                switch (o.Shape)
                {
                    case Circle circle:
                        Draw(circle);
                        break;
                    case AABB aabb:
                        Draw(aabb);
                        break;
                }
            }
        }

        private void Draw(Circle circle)
        {
            var r = circle.Radius;
            gfxBuffer.FillEllipse(
                fillCircleBrush,
                circle.Center.X - r, circle.Center.Y - r,
                r * 2, r * 2);
        }
        
        private void Draw(AABB aabb)
        {
            var size = aabb.Size;
            gfxBuffer.FillRectangle(
                fillRectangleBrush,
                aabb.Min.X, aabb.Min.Y,
                size.X, size.Y);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;
                case Keys.W:
                    keys.W = true;
                    break;
                case Keys.A:
                    keys.A = true;
                    break;
                case Keys.S:
                    keys.S = true;
                    break;
                case Keys.D:
                    keys.D = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    keys.W = false;
                    break;
                case Keys.A:
                    keys.A = false;
                    break;
                case Keys.S:
                    keys.S = false;
                    break;
                case Keys.D:
                    keys.D = false;
                    break;
            }

            base.OnKeyUp(e);
        }
    }
}