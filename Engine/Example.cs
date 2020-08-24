using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Helpers;
using Engine.Physics.Systems;
using Leopotam.Ecs;


namespace Engine
{
    public class Example : Form
    {
        private EcsWorld world;
        private EcsSystems logicSystems;
        private EcsSystems renderSystems;
        private PhysicsSettings settings;
        private DrawingState drawingState;

        private KeyState keys;

        private struct KeyState
        {
            public bool W, A, S, D;
        }

        public Example()
        {
            DoubleBuffered = true;
            Size = new Size(800, 700);
            drawingState = new DrawingState(Size.Width, Size.Height);
            settings = new PhysicsSettings();
            
            CreateSystems();
            AddObjects();
            
            keys = new KeyState();

            var timer = new Timer();
            timer.Interval = 16;
            timer.Tick += GameLoop;
            timer.Start();
        }
        
        private void CreateSystems()
        {
            world = new EcsWorld ();
            
            logicSystems = new EcsSystems (world);
            logicSystems
                .Add(new CalcualteCollidersAABBs())
                .Add(new Broadphase())
                .Add(new Narrowphase())
                .Add(new SolveCollisions())
                .Add(new CorrectPositions())
                
                .Add(new ApplyGlobalGravity())
                .Add(new ApplyAttractorsForces())
                //.Add(new ApplyFriction())
                
                .Add(new ApplyVelocity())
                
                .Add(new CleanPhysicsState())
                .Add(new DeleteUnusedObjects())
                
                //.Add(new PrintDebug())
                
                .Inject(settings)
                .Inject(new PhysicsSystemState())
                .Init();
            
            
            renderSystems = new EcsSystems(world);
            renderSystems
                .Add(new DrawColliders())
                .Add(new DrawAABBs())
                
                .Inject(settings)
                .Inject(drawingState)
                .Init();
        }

        private void AddObjects()
        {
            var pToM = settings.PixelsToMeters; 
            
            PhysicsObjectsFactory.CreateWall(Vector2.Zero, new Vector2(65, Height), world, pToM);
            PhysicsObjectsFactory.CreateWall(new Vector2(Width - 65, 0), new Vector2(Width, Height), world, pToM);
            PhysicsObjectsFactory.CreateWall(Vector2.Zero, new Vector2(Width, 65), world, pToM);
            PhysicsObjectsFactory.CreateWall(new Vector2(0, Height - 65), new Vector2(Width, Height), world, pToM);
            
            //PhysicsObjectsFactory.CreateWall(new Vector2(150, 400), new Vector2(300, 500), world, pToM);
            //PhysicsObjectsFactory.CreateWall(new Vector2(500, 400), new Vector2(650, 500), world, pToM);

            for (var i = 0; i < 400; i += 20) {
                for (var j = 0; j < 100; j += 20) {
                    PhysicsObjectsFactory.CreateSmallBall(new Vector2(i + 200, j + 150), world, pToM);
                }
            }

            // player = ObjectTemplates.CreateMedBall(new Vector2(300, 100));
            // ObjectTemplates.CreateMedBall(new Vector2(500, 100));
            //
            PhysicsObjectsFactory.CreateAttractor(new Vector2(400, 450), world, pToM);
        }
		
        private void GameLoop(object sender, EventArgs args)
        {
            // var moveSpeed = 32 * 10;
            // var velocity = Vector2.Zero;
            // if (keys.W) velocity.Y -= moveSpeed;
            // if (keys.A) velocity.X -= moveSpeed;
            // if (keys.S) velocity.Y += moveSpeed;
            // if (keys.D) velocity.X += moveSpeed;
            //
            // player.Velocity = velocity;
            
            for (var i = 0; i < 4; i++)
                logicSystems.Run();
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            drawingState.gfxBuffer.Clear(Color.Black);
            renderSystems.Run();
            
            e.Graphics.DrawImage(drawingState.bmpBuffer, Point.Empty);
        }

        // protected override void OnKeyDown(KeyEventArgs e)
        // {
        //     switch (e.KeyCode)
        //     {
        //         case Keys.Escape:
        //             Application.Exit();
        //             break;
        //         case Keys.W:
        //             keys.W = true;
        //             break;
        //         case Keys.A:
        //             keys.A = true;
        //             break;
        //         case Keys.S:
        //             keys.S = true;
        //             break;
        //         case Keys.D:
        //             keys.D = true;
        //             break;
        //     }
        //
        //     base.OnKeyDown(e);
        // }
        //
        // protected override void OnKeyUp(KeyEventArgs e)
        // {
        //     switch (e.KeyCode)
        //     {
        //         case Keys.W:
        //             keys.W = false;
        //             break;
        //         case Keys.A:
        //             keys.A = false;
        //             break;
        //         case Keys.S:
        //             keys.S = false;
        //             break;
        //         case Keys.D:
        //             keys.D = false;
        //             break;
        //     }
        //
        //     base.OnKeyUp(e);
        // }
    }
}