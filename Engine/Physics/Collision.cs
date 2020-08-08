using System;
using Engine.Physics.Classes;
using Engine.Physics.Structs;

namespace Engine.Physics
{
    internal static class Collision
    {
        public static bool AABBvsAABB(AABB a, AABB b)
        {
            return !(a.Max.X < b.Min.X || a.Min.X > b.Max.X || a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y);
        }

        public static bool AABBvsAABB(Manifold m)
        {
            // Setup a couple pointers to each object
            var A = m.A;
            var B = m.B;

            // Vector from A to B
            var n = B.Center - A.Center;

            var abox = (AABB) A.Shape;
            var bbox = (AABB) B.Shape;

            // Calculate half extents along x axis for each object
            var a_extent = (abox.Max.X - abox.Min.X) / 2;
            var b_extent = (bbox.Max.X - bbox.Min.X) / 2;

            // Calculate overlap on x axis
            var x_overlap = a_extent + b_extent - Math.Abs(n.X);

            // SAT test on x axis
            if (x_overlap > 0)
            {
                // Calculate half extents along y axis for each object
                a_extent = (abox.Max.Y - abox.Min.Y) / 2;
                b_extent = (bbox.Max.Y - bbox.Min.Y) / 2;

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

        public static bool CirclevsCircle(Manifold m)
        {
            // Setup a couple pointers to each object
            var A = (Circle) m.A.Shape;
            var B = (Circle) m.B.Shape;

            // Vector from A to B
            var n = m.B.Center - m.A.Center;

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

        public static bool AABBvsCircle(Manifold m)
        {
            // Setup a couple pointers to each object
            //Box Shape
            var box = (AABB) m.A.Shape;

            //CircleShape
            var circle = (Circle) m.B.Shape;

            // Vector from box to circle
            var n = m.B.Center - m.A.Center;

            // Closest point on box to center of circle
            var closest = n;

            // Calculate half extents along each axis
            var x_extent = (box.Max.X - box.Min.X) / 2;
            var y_extent = (box.Max.Y - box.Min.Y) / 2;

            // Clamp point to edges of the AABB
            closest.X = Clamp(-x_extent, x_extent, closest.X);
            closest.Y = Clamp(-y_extent, y_extent, closest.Y);


            var inside = false;

            // Circle is inside the AABB, so we need to clamp the circle's center
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
            // Circle not inside the AABB
            if (d > r * r && !inside)
            {
                return false;
            }

            // Avoided sqrt until we needed
            d = (float) Math.Sqrt(d);

            // Collision normal needs to be flipped to point outside if circle was
            // inside the AABB
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

        public static void ResolveCollision(Manifold m)
        {
            var rv = m.B.Velocity - m.A.Velocity;

            if (float.IsNaN(m.Normal.X + m.Normal.Y))
            {
                return;
            }

            var velAlongNormal = Vector2.Dot(rv, m.Normal);

            if (velAlongNormal > 0)
            {
                return;
            }

            var e = Math.Min(m.A.Restitution, m.B.Restitution);

            var j = -(1 + e) * velAlongNormal / (m.A.IMass + m.B.IMass);

            var impulse = m.Normal * j;

            m.A.Velocity = !m.A.Locked ? m.A.Velocity - impulse * m.A.IMass : m.A.Velocity;
            m.B.Velocity = !m.B.Locked ? m.B.Velocity + impulse * m.B.IMass : m.B.Velocity;
        }

        public static void PositionalCorrection(Manifold m)
        {
            const float percent = 0.6F; // usually 20% to 80%
            var correction = m.Normal * (percent * (m.Penetration / (m.A.IMass + m.B.IMass)));
            if (!m.A.Locked)
            {
                m.A.Move(-correction * m.A.IMass);
            }

            if (!m.B.Locked)
            {
                m.B.Move(correction * m.B.IMass);
            }
        }

        private static float Clamp(float low, float high, float val)
        {
            return Math.Max(low, Math.Min(val, high));
        }
    }
}