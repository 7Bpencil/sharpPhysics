using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Example.Components;
using Engine.Example.Systems;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Physics.Systems;
using Engine.Render;
using Engine.Render.Systems;
using Leopotam.EcsLite;


namespace Engine.Example
{
    public class KeyState
    {
        public bool W, A, S, D;
    }

    public sealed class Example : Form
    {
        private EcsWorld world;
        private EcsSystems gameplaySystems;
        private EcsSystems physicsSystems;
        private EcsSystems renderSystems;

        private DrawingSystemsData drawingSystemsData;
        private PhysicsSystemData physicsSystemData;
        private KeyState keys;

        private ObjectsFactory objectsFactory;

        public Example()
        {
            DoubleBuffered = true;
            Size = new Size(900, 720);
            keys = new KeyState();
            drawingSystemsData = new DrawingSystemsData(Size.Width, Size.Height);
            physicsSystemData = new PhysicsSystemData(
                1 / 60f,
                new Vector2(0, -9.80665f),
                new BoundingBox {Min = new Vector2(-200), Max = new Vector2(200)});

            world = new EcsWorld();
            var sharedData = new SharedData
            {
                keys = keys,
                DrawingSystemsData = drawingSystemsData,
                PhysicsSystemData = physicsSystemData,

                attractors = world.GetPool<Attractor>(),
                players = world.GetPool<Player>(),
                rigidBodies = world.GetPool<RigidBody>(),
                velocities = world.GetPool<Velocity>(),
                poses = world.GetPool<Pose>(),
                circleShapes = world.GetPool<Circle>(),
                boxShapes = world.GetPool<Box>(),
                bboxes = world.GetPool<BoundingBox>()
            };
            objectsFactory = new ObjectsFactory(sharedData, world);

            CreateSystems(sharedData);
            AddObjects();

            var timer = new Timer();
            timer.Interval = 16;
            timer.Tick += GameLoop;
            timer.Start();
        }

        private void CreateSystems(SharedData sharedData)
        {
            gameplaySystems = new EcsSystems(world, sharedData);
            gameplaySystems
                // user defined logic - physics engine know nothing about gravity, players, or attractors
                .Add(new InputSystem())
                .Add(new ApplyGravity())
                .Add(new ApplyAttractorsForces())

                .Init();


            physicsSystems = new EcsSystems(world, sharedData);
            physicsSystems
                .Add(new UpdateBoundingBoxes())
                .Add(new BroadPhase())
                .Add(new NarrowPhase())
                .Add(new SolveCollisions())
                .Add(new CorrectPositions())
                .Add(new IntegratePoses())
                .Add(new CleanPhysicsState())
                .Add(new RemoveLeakedObjects())

                .Init();


            renderSystems = new EcsSystems(world, sharedData);
            renderSystems
                //.Add(new DrawColliders())
                .Add(new DrawVelocities())
                //.Add(new DrawBoundingBoxes())

                .Init();
        }

        private void AddObjects()
        {
            objectsFactory.CreateWall(new Vector2(0, 1), new Vector2(1, 11));
            objectsFactory.CreateWall(new Vector2(1, 0), new Vector2(14, 1));
            objectsFactory.CreateWall(new Vector2(14, 1), new Vector2(15, 11));
            objectsFactory.CreateWall(new Vector2(1, 11), new Vector2(14, 12));

            objectsFactory.CreateWall(new Vector2(3, 2.5f), new Vector2(6, 3.5f));

            var offset = new Vector2(2, 7);
            for (var i = 0; i < 20; ++i) {
                for (var j = 0; j < 5; ++j) {
                    objectsFactory.CreateSmallBall(offset + new Vector2(i, j) * 0.3f);
                }
            }

            for (var i = 11; i < 14; ++i) {
                for (var j = 0; j < 2; ++j) {
                    objectsFactory.CreateMediumBall(offset + new Vector2(i, j) * 0.6f);
                }
            }

            objectsFactory.CreateAttractor(new Vector2(9, 3.5f), true);
            objectsFactory.MarkAsPlayer(
                objectsFactory.CreateAttractor(new Vector2(9, 6), false));
        }

        private void GameLoop(object sender, EventArgs args)
        {
            gameplaySystems.Run();
            physicsSystems.Run();

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawingSystemsData.gfxBuffer.Clear(Color.Black);
            renderSystems.Run();

            e.Graphics.DrawImage(drawingSystemsData.bmpBuffer, Point.Empty);
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
