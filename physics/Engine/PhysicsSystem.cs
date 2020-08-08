using System.Collections.Generic;
using physics.Engine.Classes;
using physics.Engine.Helpers;
using physics.Engine.Structs;

namespace physics.Engine
{
    public class PhysicsSystem
    {
        public float GravityScale = 10F;
        public Vector2 Gravity;
        public float Friction;
        
        private const float dt = 1 / 60f;
        private double accumulator = 0;

        public static PhysicsObject ActiveObject;
        public static readonly List<CollisionPair> ListCollisionPairs = new List<CollisionPair>();
        public static readonly List<PhysicsObject> ListGravityObjects = new List<PhysicsObject>();
        public static readonly List<PhysicsObject> ListStaticObjects = new List<PhysicsObject>();
        public static readonly Queue<PhysicsObject> RemovalQueue = new Queue<PhysicsObject>();
        
        public PhysicsSystem()
        {
            Gravity = new Vector2(0, -9.8f * GravityScale);
            Friction = 1F;
        }

        internal IEnumerable<PhysicsObject> GetMoveableObjects()
        {
            foreach (var obj in ListStaticObjects)
            {
                if (!obj.Locked && obj.Mass < 1000000)
                {
                    yield return obj;
                }
            }
        }
        
        internal void SetVelocity(PhysicsObject physicsObject, Vector2 velocity)
        {
            physicsObject.Velocity = velocity;
        }
        
        public static PhysicsObject CreateStaticCircle(Vector2 loc, int radius, float restitution, bool locked)
        {
            var obj = new PhysicsObject(new Circle(loc, radius), loc, restitution, locked);
            ListStaticObjects.Add(obj);
            return obj;
        }

        public static PhysicsObject CreateStaticBox(Vector2 start, Vector2 end, bool locked, float mass)
        {
            var oAabb = new AABB(start, end);
            PhysMath.CorrectBoundingBox(oAabb);
            var obj = new PhysicsObject(oAabb, (oAabb.Min + oAabb.Max) / 2, 0.95f, locked, mass);
            ListStaticObjects.Add(obj);
            return obj;
        }

        public bool ActivateAtPoint(Vector2 p)
        {
            ActiveObject = CheckObjectAtPoint(p);

            if (ActiveObject == null)
            {
                ActiveObject = null;
                return false;
            }

            return true;
        }

        public void AddVelocityToActive(Vector2 velocityDelta)
        {
            if (ActiveObject == null || ActiveObject.Mass >= 1000000)
            {
                return;
            }

            ActiveObject.Velocity += velocityDelta;
        }

        public void SetVelocityOfActive(Vector2 velocityDelta)
        {
            if (ActiveObject == null || ActiveObject.Mass >= 1000000)
            {
                return;
            }

            ActiveObject.Velocity = velocityDelta;
        }

        public void FreezeStaticObjects()
        {
            foreach (var physicsObject in ListStaticObjects)
            {
                physicsObject.Velocity = Vector2.Zero;
            }
        }

        public Vector2 GetActiveObjectCenter()
        {
            if (ActiveObject == null)
            {
                return Vector2.Zero;
            }

            return ActiveObject.Center;
        }

        public void MoveActiveTowardsPoint(Vector2 point)
        {
            if (ActiveObject == null)
            {
                return;
            }

            var delta = ActiveObject.Center - point;
            AddVelocityToActive(-delta / 10000);
        }

        public void HoldActiveAtPoint(Vector2 point)
        {
            if (ActiveObject == null)
            {
                return;
            }

            var delta = ActiveObject.Center - point;
            SetVelocityOfActive(-delta * 10);
        }

        public void ReleaseActiveObject()
        {
            ActiveObject = null;
        }

        public void RemoveActiveObject()
        {
            if (ListGravityObjects.Contains(ActiveObject))
            {
                ListGravityObjects.Remove(ActiveObject);
            }

            ListStaticObjects.Remove(ActiveObject);
            ActiveObject = null;
        }

        public void RemoveAllMoveableObjects()
        {
            foreach (PhysicsObject obj in GetMoveableObjects())
            {
                RemovalQueue.Enqueue(obj);
            }
        }

        public void Tick(double elapsedTime)
        {
            accumulator += elapsedTime;

            //Avoid accumulator spiral of death by clamping
            if (accumulator > 0.1f)
                accumulator = 0.1f;

            while (accumulator > dt)
            {
                BroadPhase_GeneratePairs();
                UpdatePhysics(dt);
                ProcessRemovalQueue();
                accumulator -= dt;
            }
        }
        
        private void BroadPhase_GeneratePairs()
        {
            ListCollisionPairs.Clear();

            PhysicsObject A;
            PhysicsObject B;

            for (var i = 0; i < ListStaticObjects.Count; i++)
            {
                for (var j = i + 1; j < ListStaticObjects.Count; j++)
                {
                    A = ListStaticObjects[i];
                    B = ListStaticObjects[j];

                    if (Collision.AABBvsAABB(A.GetBoundingBox(), B.GetBoundingBox()))
                    {
                        ListCollisionPairs.Add(new CollisionPair(A, B));
                    }
                }
            }
        }

        private void UpdatePhysics(float dt)
        {
            foreach (var pair in ListCollisionPairs)
            {
                var objA = pair.A;
                var objB = pair.B;

                var manifold = new Manifold();
                var collision = false;

                if (objA.Shape is Circle && objB.Shape is AABB)
                {
                    manifold.A = objB;
                    manifold.B = objA;
                }
                else
                {
                    manifold.A = objA;
                    manifold.B = objB;
                }

                //Box vs anything
                if (manifold.A.Shape is AABB)
                {
                    if (manifold.B.Shape is AABB)
                    {
                        //continue;
                        if (Collision.AABBvsAABB(manifold))
                        {
                            collision = true;
                        }
                    }

                    if (manifold.B.Shape is Circle)
                    {
                        if (Collision.AABBvsCircle(manifold))
                        {
                            collision = true;
                        }
                    }
                }

                //Circle Circle
                else
                {
                    if (manifold.B.Shape is Circle)
                    {
                        if (Collision.CirclevsCircle(manifold))
                        {
                            collision = true;
                        }
                    }
                }

                //Resolve Collision
                if (collision)
                {
                    Collision.ResolveCollision(manifold);
                    Collision.PositionalCorrection(manifold);
                }
            }

            for (var i = 0; i < ListStaticObjects.Count; i++)
            {
                ApplyConstants(ListStaticObjects[i], dt);
                ListStaticObjects[i].Move(dt);
            }
        }
        
        private void ProcessRemovalQueue()
        {
            if (RemovalQueue.Count > 0)
            {
                var obj = RemovalQueue.Dequeue();
                ListStaticObjects.Remove(obj);
                ListGravityObjects.Remove(obj);
            }
        }

        private void AddGravity(PhysicsObject obj, float dt)
        {
            obj.Velocity += GetGravityVector(obj) * dt;
        }

        private void ApplyConstants(PhysicsObject obj, float dt)
        {
            if (obj.Locked)
            {
                return;
            }

            //AddGravity(obj, dt);
            obj.Velocity -= new Vector2(Friction * dt);

            if (obj.Center.Y > 2000 || obj.Center.Y < -2000 || obj.Center.X > 2000 || obj.Center.X < -2000)
            {
                RemovalQueue.Enqueue(obj);
            }
        }

        private Vector2 CalculatePointGravity(PhysicsObject obj)
        {
            var forces = Vector2.Zero;

            if (obj.Locked)
            {
                return forces;
            }

            foreach (var gpt in ListGravityObjects)
            {
                var diff = gpt.Center - obj.Center;
                PhysMath.RoundToZero(ref diff, 5F);

                //apply inverse square law
                var falloffMultiplier = gpt.Mass / diff.LengthSquared;

                diff.X = (int) diff.X == 0 ? 0 : diff.X * falloffMultiplier;
                diff.Y = (int) diff.Y == 0 ? 0 : diff.Y * falloffMultiplier;

                if (diff.Length > .005F)
                {
                    forces += diff;
                }
            }

            return forces;
        }

        private PhysicsObject CheckObjectAtPoint(Vector2 p)
        {
            foreach (var physicsObject in ListStaticObjects)
            {
                if (physicsObject.Contains(p))
                {
                    return physicsObject;
                }
            }

            return null;
        }

        private Vector2 GetGravityVector(PhysicsObject obj)
        {
            return CalculatePointGravity(obj) + Gravity;
        }
    }
}