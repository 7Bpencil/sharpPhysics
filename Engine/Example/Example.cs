using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Example.Components;
using Engine.Example.Systems;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Helpers;
using Engine.Physics.Systems;
using Engine.Render;
using Engine.Render.Systems;
using Leopotam.Ecs;


namespace Engine.Example
{
    public class KeyState
    {
        public bool W, A, S, D;
    }

    public class Example : Form
    {
        private EcsWorld world;
        private EcsSystems gameplaySystems;
        private EcsSystems physicsSystems;
        private EcsSystems renderSystems;
        private PhysicsSettings settings;
        private DrawingState drawingState;
        private KeyState keys;

        public Example()
        {
            DoubleBuffered = true;
            Size = new Size(800, 700);
            drawingState = new DrawingState(Size.Width, Size.Height);
            settings = new PhysicsSettings();
            keys = new KeyState();

            CreateSystems();
            AddObjects();

            var timer = new Timer();
            timer.Interval = 16;
            timer.Tick += GameLoop;
            timer.Start();
        }

        private void CreateSystems()
        {
            world = new EcsWorld ();

            gameplaySystems = new EcsSystems(world);
            gameplaySystems
                .Add(new InputSystem())
                .Inject(keys)
                .Init();

            physicsSystems = new EcsSystems (world);
            physicsSystems
                .Add(new CalculateBoundingBoxes())
                .Add(new BroadPhase())
                .Add(new NarrowPhase())
                .Add(new SolveCollisions())
                .Add(new CorrectPositions())

                .Add(new ApplyGlobalGravity())
                .Add(new ApplyAttractorsForces())
                .Add(new ApplyFriction())

                .Add(new ApplyVelocity())

                .Add(new CleanPhysicsState())
                .Add(new DeleteUnusedObjects())

                //.Add(new PrintDebug())

                .Inject(settings)
                .Inject(new PhysicsSystemState())
                .Init();

            renderSystems = new EcsSystems(world);
            renderSystems
                //.Add(new DrawColliders())
                .Add(new DrawVelocities())
                //.Add(new DrawBoundingBoxes())

                .Inject(settings)
                .Inject(drawingState)
                .Init();
        }

        private void AddObjects()
        {
            var factory = new PhysicsObjectsFactory(settings);

            factory.CreateWall(Vector2.Zero, new Vector2(65, Height), world);
            factory.CreateWall(new Vector2(Width - 65, 0), new Vector2(Width, Height), world);
            factory.CreateWall(Vector2.Zero, new Vector2(Width, 65), world);
            factory.CreateWall(new Vector2(0, Height - 65), new Vector2(Width, Height), world);

            factory.CreateWall(new Vector2(150, 400), new Vector2(300, 500), world);

            for (var i = 0; i < 400; i += 20) {
                for (var j = 0; j < 100; j += 20) {
                    factory.CreateSmallBall(new Vector2(i + 200, j + 150), world);
                }
            }

            factory.CreateAttractor(new Vector2(500, 450), true, world);

            factory.CreateAttractor(new Vector2(600, 250), true, world)
                .Replace(new Player());
        }

        private void GameLoop(object sender, EventArgs args)
        {
            gameplaySystems.Run();

            for (var i = 0; i < settings.IterationsAmount; i++)
                physicsSystems.Run();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawingState.gfxBuffer.Clear(Color.Black);
            renderSystems.Run();

            e.Graphics.DrawImage(drawingState.bmpBuffer, Point.Empty);
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
