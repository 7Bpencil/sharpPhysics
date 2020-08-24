using System;
using System.Runtime.CompilerServices;
using Engine.Physics.Components;
using Engine.Physics.Components.Colliders;


namespace Engine.Physics.Helpers
{
    public static class CollisionDetection
    {
        public static bool AABBvsAABB(ref AABB a, ref AABB b)
        {
            return !(a.Max.X < b.Min.X || a.Min.X > b.Max.X || a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y);
        }
        
        public static bool BoxBox(Box boxA, Vector2 centerA, Box boxB, Vector2 centerB, ref Manifold m)
        {
            // Vector from A to B
            var n = centerB - centerA;
            
            // Calculate half extents along x axis for each object
            var a_extent = boxA.HalfSize.X;
            var b_extent = boxB.HalfSize.X;

            // Calculate overlap on x axis
            var x_overlap = a_extent + b_extent - Math.Abs(n.X);

            // SAT test on x axis
            if (x_overlap > 0)
            {
                // Calculate half extents along y axis for each object
                a_extent = boxA.HalfSize.Y;
                b_extent = boxB.HalfSize.Y;

                // Calculate overlap on y axis
                var y_overlap = a_extent + b_extent - Math.Abs(n.Y);

                // SAT test on y axis
                if (y_overlap > 0)
                {
                    // Find out which axis is axis of least penetration
                    if (x_overlap < y_overlap)
                    {
                        // Point towards B knowing that n points from A to B
                        m.Normal = n.X < 0 ? new Vector2(-1, 0) : new Vector2(1, 0);
                        m.Penetration = x_overlap;
                    }
                    else
                    {
                        // Point toward B knowing that n points from A to B
                        m.Normal = n.Y < 0 ? new Vector2(0, -1) : new Vector2(0, 1);
                        m.Penetration = y_overlap;
                    }
                    
                    return true;
                }
            }

            return false;
        }
        
        
        public static bool CircleCircle(Circle A, Vector2 centerA, Circle B, Vector2 centerB, ref Manifold m)
        {
            // Vector from A to B
            var n = centerB - centerA;

            var r = A.Radius + B.Radius;
            if (n.LengthSquared > r * r)
            {
                return false;
            }

            // Circles have collided, now compute manifold
            var d = n.Length; // perform actual sqrt
            // If distance between circles is not zero
            if (d != 0)
            {
                // Distance is difference between radius and distance
                m.Penetration = r - d;

                // Utilize our d since we performed sqrt on it already within Length( )
                // Points from A to B, and is a unit vector
                m.Normal = n / d;
                return true;
            }

            // Circles are on same position
            // Choose random (but consistent) values
            m.Penetration = A.Radius;
            m.Normal = new Vector2(1, 0);
            return true;
        }
        
        
        public static bool BoxCircle(Box box, Vector2 centerA, Circle circle, Vector2 centerB, ref Manifold m)
        {
            // Vector from box to circle
            var n = centerB - centerA;

            // Closest point on box to center of circle
            var closest = n;

            // Calculate half extents along each axis
            var x_extent = box.HalfSize.X;
            var y_extent = box.HalfSize.Y;

            // Clamp point to edges of the Box
            closest.X = Clamp(-x_extent, x_extent, closest.X);
            closest.Y = Clamp(-y_extent, y_extent, closest.Y);


            var inside = false;

            // Circle's center is inside the Box, so we need to clamp the circle's center
            // to the closest edge
            if (n == closest)
            {
                inside = true;

                // Find closest axis
                if (Math.Abs(n.X) < Math.Abs(n.Y))
                {
                    // Clamp to closest extent
                    closest.X = closest.X > 0 ? x_extent : -x_extent;
                }

                // y axis is shorter
                else
                {
                    // Clamp to closest extent
                    closest.Y = closest.Y > 0 ? y_extent : -y_extent;
                }
            }

            var normal = n - closest;
            var d = normal.LengthSquared;
            var r = circle.Radius;

            // Early out of the radius is shorter than distance to closest point and
            // Circle not inside the Box
            if (d > r * r && !inside)
            {
                return false;
            }

            // Avoided sqrt until we needed
            d = (float) Math.Sqrt(d);

            // Collision normal needs to be flipped to point outside if circle was
            // inside the Box
            if (inside)
            {
                m.Normal = Vector2.Normalize(-normal);
                m.Penetration = r - d;
            }
            else
            {
                //If pushing up at all, go straight up (gravity hack)
                m.Normal = Vector2.Normalize(normal);
                m.Penetration = r - d;
            }

            return true;
        }
        

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        private static float Clamp(float low, float high, float val)
        {
            return Math.Max(low, Math.Min(val, high));
        }
    }
}