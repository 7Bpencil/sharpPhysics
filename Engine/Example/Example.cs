using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Example.Components;
using Engine.Example.Systems;
using Engine.Physics;
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
        private PhysicsSettings physicsSettings;
        private DrawingState drawingState;
        private KeyState keys;

        public Example()
        {
            DoubleBuffered = true;
            Size = new Size(900, 720);
            drawingState = new DrawingState(Size.Width, Size.Height);
            physicsSettings = new PhysicsSettings();
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
                .Inject(physicsSettings)

                .Init();


            physicsSystems = new EcsSystems (world);
            physicsSystems
                .Add(new ApplyGravity())
                .Add(new ApplyAttractorsForces())

                .Add(new UpdateBoundingBoxes())
                .Add(new BroadPhase())
                .Add(new NarrowPhase())
                .Add(new SolveCollisions())
                .Add(new CorrectPositions())

                .Add(new IntegratePoses())

                .Add(new CleanPhysicsState())
                .Add(new RemoveLeakedObjects())

                //.Add(new PrintDebug())

                .Inject(physicsSettings)
                .Inject(new PhysicsSystemState())

                .Init();


            renderSystems = new EcsSystems(world);
            renderSystems
                //.Add(new DrawColliders())
                .Add(new DrawVelocities())
                //.Add(new DrawBoundingBoxes())

                .Inject(drawingState)

                .Init();
        }

        private void AddObjects()
        {
            PhysicsObjectsFactory.CreateWall(new Vector2(0, 1), new Vector2(1, 11), world);
            PhysicsObjectsFactory.CreateWall(new Vector2(1, 0), new Vector2(14, 1), world);
            PhysicsObjectsFactory.CreateWall(new Vector2(14, 1), new Vector2(15, 11), world);
            PhysicsObjectsFactory.CreateWall(new Vector2(1, 11), new Vector2(14, 12), world);

            PhysicsObjectsFactory.CreateWall(new Vector2(3, 8.5f), new Vector2(6, 9.5f), world);


            var offset = new Vector2(2);
            for (var i = 0; i < 20; ++i) {
                for (var j = 0; j < 5; ++j) {
                    PhysicsObjectsFactory.CreateSmallBall(offset + new Vector2(i, j) * 0.3f, world);
                }
            }

            PhysicsObjectsFactory.CreateAttractor(new Vector2(9, 8.5f), true, world);
            PhysicsObjectsFactory.CreateAttractor(new Vector2(9, 6), true, world)
                .Replace(new Player());
        }

        private void GameLoop(object sender, EventArgs args)
        {
            gameplaySystems.Run();
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
