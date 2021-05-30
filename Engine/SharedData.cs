using Engine.Example;
using Engine.Example.Components;
using Engine.Physics;
using Engine.Physics.Components;
using Engine.Physics.Components.Shapes;
using Engine.Render;
using Leopotam.EcsLite;

namespace Engine
{
    public class SharedData
    {
        public PhysicsSystemData PhysicsSystemData;
        public DrawingSystemsData DrawingSystemsData;
        public KeyState keys;

        public EcsPool<Attractor> attractors;
        public EcsPool<Player> players;
        public EcsPool<RigidBody> rigidBodies;
        public EcsPool<Velocity> velocities;
        public EcsPool<Pose> poses;
        public EcsPool<Circle> circleShapes;
        public EcsPool<Box> boxShapes;
        public EcsPool<BoundingBox> bboxes;
    }
}
