using System;
using System.Collections.Generic;
using System.Numerics;

namespace GameEngine
{
    public enum OutOfBoundsHandling { Destroy, Wrap, Freeze};
    static class PhysicsManager
    {
        //All Colliders
        public static List<Collider> colliders = new List<Collider>();
        //All Rigidbodies
        public static List<RigidBody> rigidbodies = new List<RigidBody>();
        //How should Rigidbodies behave if they go out of world bounds
        public static OutOfBoundsHandling outOfBoundsHandling = OutOfBoundsHandling.Freeze;
        //How many physics steps to take
        public static int physicsIterations = 2;

        //Add new Physics to the List
        public static void AddPhysics(RigidBody p)
        {
            if (!rigidbodies.Contains(p))
            {
                rigidbodies.Add(p);
            }
        }

        //Remove Physics from the List
        public static void RemovePhysics(RigidBody p)
        {
            if (rigidbodies.Contains(p))
            {
                rigidbodies.Remove(p);
            }
        }

        //Add new Collider to the List
        public static void AddCollider(Collider col)
        {
            if(!colliders.Contains(col))
            {
                colliders.Add(col);
            }
        }

        //Remove Collider from the List
        public static void RemoveCollider(Collider col)
        {
            if (colliders.Contains(col))
            {
                colliders.Remove(col);
            }
        }

        #region Raycasting
        //All raycasting
        //Get rays
        static Vector2[] GetRay(Vector2 p1, Vector2 p2)
        {
            return new Vector2[2]
                    {
                        p1,
                        p2
                    };
        }
        static Vector2[] GetRay(Vector2 origin, Vector2 direction, float length)
        {
            direction = Vector2.Normalize(direction);
            return new Vector2[2]
                    {
                        origin,
                        origin + direction*length
                    };
        }
        //Check raycast
        static bool CheckRayCast(Collider col, Vector2[] colliderVerts)
        {
            CollisionShape shape = col.Shape;
            if (shape == CollisionShape.BOX)
            {
                if (Collisions.IntersectPolygons(col.GameObject.transformedVertices, colliderVerts))
                {
                    return true;
                }
            }
            else
            {
                if (Collisions.IntersectCirclePolygon(col.GameObject.Position, col.GameObject.Size.X / 2, colliderVerts))
                {
                    return true;
                }
            }
            return false;
        }
        static bool CheckRayCast(Collider col, Vector2[] colliderVerts, out Collider hit)
        {
            CollisionShape shape = col.Shape;
            if (shape == CollisionShape.BOX)
            {
                if (Collisions.IntersectPolygons(col.GameObject.transformedVertices, colliderVerts))
                {
                    hit = col;
                    return true;
                }
            }
            else
            {
                if (Collisions.IntersectCirclePolygon(col.GameObject.Position, col.GameObject.Size.X / 2, colliderVerts))
                {
                    hit = col;
                    return true;
                }
            }
            hit = null;
            return false;
        }

        //Raycast. Origin = start, Endpoint = origin + direction, length = how far,
        //out Collider hit gives the collider being hit, out Collider[] hits gives all colliders intersecting ray,
        //string ignoreTag and string[] ignoretags is what objects to ignore
        public static bool Raycast(Vector2 origin, Vector2 direction, float length)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                if (CheckRayCast(col, colliderVerts))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, out Collider hit)
        {
            hit = null;
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                if (CheckRayCast(col, colliderVerts, out hit))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, string ignoreTag)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                if (col.GameObject.tag == ignoreTag)
                {
                    continue;
                }
                if (CheckRayCast(col, colliderVerts))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, out Collider hit, string ignoreTag)
        {
            hit = null;
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                if (col.GameObject.tag == ignoreTag)
                {
                    continue;
                }
                if (CheckRayCast(col, colliderVerts, out hit))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, string[] ignoreTags)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                foreach (string ignoreTag in ignoreTags)
                {
                    if (col.GameObject.tag == ignoreTag)
                    {
                        continue;
                    }
                }

                if (CheckRayCast(col, colliderVerts))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, out Collider hit, string[] ignoreTags)
        {
            hit = null;
            Vector2[] colliderVerts = GetRay(origin, direction, length);

            foreach (Collider col in colliders)
            {
                foreach (string ignoreTag in ignoreTags)
                {
                    if (col.GameObject.tag == ignoreTag)
                    {
                        continue;
                    }
                }
                if (CheckRayCast(col, colliderVerts, out hit))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool RaycastAll(Vector2 origin, Vector2 direction, float length, out Collider[] hits)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);
            List<Collider> cols = new List<Collider>();
            foreach (Collider col in colliders)
            {
                if (CheckRayCast(col, colliderVerts, out Collider hit))
                {
                    cols.Add(hit);
                }
            }
            hits = cols.ToArray();

            if(hits.Length > 0)
            {
                return true;
            }
            return false;
        } 
        public static bool RaycastAll(Vector2 origin, Vector2 direction, float length, out Collider[] hits, string ignoreTag)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);
            List<Collider> cols = new List<Collider>();
            foreach (Collider col in colliders)
            {
                if (col.GameObject.tag == ignoreTag)
                {
                    continue;
                }
                if (CheckRayCast(col, colliderVerts, out Collider hit))
                {
                    cols.Add(hit);
                }
            }
            hits = cols.ToArray();
            if(hits.Length > 0)
            {
                return true;
            }
            return false;
        }
        public static bool Raycast(Vector2 origin, Vector2 direction, float length, out Collider[] hits, string[] ignoreTags)
        {
            Vector2[] colliderVerts = GetRay(origin, direction, length);
            List<Collider> cols = new List<Collider>();
            foreach (Collider col in colliders)
            {
                foreach (string ignoreTag in ignoreTags)
                {
                    if (col.GameObject.tag == ignoreTag)
                    {
                        continue;
                    }
                }
                if (CheckRayCast(col, colliderVerts, out Collider hit))
                {
                    cols.Add(hit);
                }
            }
            hits = cols.ToArray();
            if(hits.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Collision Casting
        //Simple check to see what objects are intersecting a point
        //Point is where, includeTrigger is if trigger objects should be included,
        //ignoreTag is if it should ignore the tag or only include the tag,
        //tag is what GameObject tag to avoid/check for
        public static Collider CollisionCast(Vector2 point, bool includeTrigger = true)
        {
            for (int i = 0; i < colliders.Count; i += 1)
            {
                if (!includeTrigger && colliders[i].isTrigger)
                {
                    continue;
                }

                CollisionShape shape = colliders[i].Shape;
                if (shape == CollisionShape.CIRCLE)
                {
                    if (Collisions.IntersectCircle(point, 0.01f, colliders[i].GameObject.Position, colliders[i].GameObject.Size.X / 2))
                    {
                        return colliders[i];
                    }
                }
                else if (shape == CollisionShape.BOX)
                {
                    if (Collisions.IntersectCirclePolygon(point, 0.01f, colliders[i].GameObject.transformedVertices))
                    {
                        return colliders[i];
                    }
                }
            }
            return null;
        }

        public static Collider[] CollisionCastAll(Vector2 point, bool includeTrigger = true)
        {
            List<Collider> colliderList = new List<Collider>();
            for (int i = 0; i < colliders.Count; i += 1)
            {
                if (!includeTrigger && colliders[i].isTrigger)
                {
                    continue;
                }

                CollisionShape shape = colliders[i].Shape;
                if (shape == CollisionShape.CIRCLE)
                {
                    if (Collisions.IntersectCircle(point, 0.01f, colliders[i].GameObject.Position, colliders[i].GameObject.Size.X / 2))
                    {
                        colliderList.Add(colliders[i]);
                    }
                }
                else if (shape == CollisionShape.BOX)
                {
                    if (Collisions.IntersectCirclePolygon(point, 0.01f, colliders[i].GameObject.transformedVertices))
                    {
                        colliderList.Add(colliders[i]);
                    }
                }
            }
            return colliderList.ToArray();
        }

        public static Collider CollisionCast(Vector2 point, bool ignoreTag, string tag, bool includeTrigger = true)
        {
            for (int i = 0; i < colliders.Count; i += 1)
            {
                if (!includeTrigger && colliders[i].isTrigger)
                {
                    continue;
                }
                if(ignoreTag && colliders[i].GameObject.tag == tag)
                {
                    continue;
                }

                CollisionShape shape = colliders[i].Shape;
                if (shape == CollisionShape.CIRCLE)
                {
                    if (Collisions.IntersectCircle(point, 0.01f, colliders[i].GameObject.Position, colliders[i].GameObject.Size.X / 2))
                    {
                        if (colliders[i].GameObject.tag == tag)
                        {
                            return colliders[i];
                        }
                    }
                }
                else if (shape == CollisionShape.BOX)
                {
                    if (Collisions.IntersectCirclePolygon(point, 0.01f, colliders[i].GameObject.transformedVertices))
                    {
                        if (colliders[i].GameObject.tag == tag)
                        {
                            return colliders[i];
                        }
                    }
                }
            }
            return null;
        }

        public static Collider[] CollisionCastAll(Vector2 point, bool ignoreTag, string tag, bool includeTrigger = true)
        {
            List<Collider> colliderList = new List<Collider>();
            for (int i = 0; i < colliders.Count; i += 1)
            {
                if (!includeTrigger && colliders[i].isTrigger)
                {
                    continue;
                }
                if(ignoreTag && colliders[i].GameObject.tag == tag)
                {
                    continue;
                }
                CollisionShape shape = colliders[i].Shape;
                if (shape == CollisionShape.CIRCLE)
                {
                    if (Collisions.IntersectCircle(point, 0.01f, colliders[i].GameObject.Position, colliders[i].GameObject.Size.X / 2))
                    {
                        if (colliders[i].GameObject.tag == tag)
                        {
                            colliderList.Add(colliders[i]);
                        }
                    }
                }
                else if (shape == CollisionShape.BOX)
                {
                    if (Collisions.IntersectCirclePolygon(point, 0.01f, colliders[i].GameObject.transformedVertices))
                    {
                        if (colliders[i].GameObject.tag == tag)
                        {
                            colliderList.Add(colliders[i]);
                        }
                    }
                }
            }
            return colliderList.ToArray();
        }
        #endregion

        //Find all collisions (Push Away)
        public static void CheckCollisions()
        {
            for(int i=0; i<colliders.Count; i+=1)
            {
                if(colliders[i].manualDetectOnly)
                {
                    continue;
                }
                colliders[i].ClearCollisions();

                GameObject gameObjectA = colliders[i].GameObject;
                if(gameObjectA.OutOfBounds)
                {
                    continue;
                }
                CollisionShape shapeA = colliders[i].Shape;
                for (int j=i+1; j<colliders.Count; j+=1)
                {
                    colliders[j].ClearCollisions();
                    GameObject gameObjectB = colliders[j].GameObject;
                    if (gameObjectB.OutOfBounds)
                    {
                        continue;
                    }
                    CollisionShape shapeB = colliders[j].Shape;

                    bool colliding = false;
                    Vector2 normal = Vector2.Zero;
                    float depth = 0;

                    if(gameObjectA.Position == gameObjectB.Position && gameObjectA.GetComponent<RigidBody>() != null && gameObjectB.GetComponent<RigidBody>() != null)
                    {
                        depth = Engine.TileSize.Y;

                        int rot = Random.Range(0,359);
                        normal = new Vector2(0, 1).Rotate(rot);
                        gameObjectA.Position += -normal * depth / 2;
                        gameObjectB.Position += normal * depth / 2;

                        return;
                    }

                    if (shapeA == CollisionShape.BOX && shapeB == CollisionShape.BOX)
                    {
                        if (Collisions.IntersectPolygons(gameObjectA.transformedVertices, gameObjectB.transformedVertices, out depth, out normal))
                        {
                            colliding = true;
                        }
                    }
                    else if(shapeA == CollisionShape.CIRCLE && shapeB == CollisionShape.CIRCLE)
                    {
                        if (Collisions.IntersectCircle(gameObjectA.Position, gameObjectA.Size.X/2, gameObjectB.Position, gameObjectB.Size.X / 2, out normal, out depth))
                        {
                            colliding = true;
                        }
                    }
                    else if (shapeA == CollisionShape.CIRCLE && shapeB == CollisionShape.BOX)
                    {
                        if (Collisions.IntersectCirclePolygon(gameObjectA.Position, gameObjectA.Size.X / 2, gameObjectB.transformedVertices, out normal, out depth))
                        {
                            colliding = true;
                        }
                    }
                    else if (shapeA == CollisionShape.BOX && shapeB == CollisionShape.CIRCLE)
                    {
                        if (Collisions.IntersectCirclePolygon(gameObjectB.Position, gameObjectB.Size.X / 2, gameObjectA.transformedVertices, out normal, out depth))
                        {
                            colliding = true;
                            normal = -normal;
                        }
                    }

                    colliders[i].AddCollider(colliders[j], colliding);

                    if (colliding && !colliders[i].isTrigger && !colliders[j].isTrigger)
                    {
                        RigidBody bodyA = gameObjectA.GetComponent<RigidBody>();
                        RigidBody bodyB = gameObjectB.GetComponent<RigidBody>();
                        if (bodyA != null && bodyB != null)
                        {
                            gameObjectA.Position += -normal * depth / 2;
                            gameObjectB.Position += normal * depth / 2;

                            if (!gameObjectA.Destroyed && !gameObjectB.Destroyed)
                            {
                                ResolveCollision(bodyA, bodyB, normal, depth);
                                ResolveFriction(bodyA, colliders[i], bodyB, colliders[j], normal);
                            }
                        }
                        else if (bodyA != null && bodyA == null)
                        {
                            gameObjectA.Position += -normal * depth;

                            if (!gameObjectA.Destroyed)
                            {
                                ResolveCollision(bodyA, null, normal, depth);
                                ResolveFriction(bodyA, colliders[i], null, colliders[j], normal);
                            }
                        }
                        else if (bodyA == null && bodyB != null)
                        {
                            gameObjectB.Position += normal * depth;
                            if (!gameObjectB.Destroyed)
                            {
                                ResolveCollision(null, bodyB, normal, depth);
                                ResolveFriction(null, colliders[i], bodyB, colliders[j], normal);
                            }
                        }
                    }
                }
                colliders[i].CheckColliders();
            }
        }


        //Resolve all collisions (Bounce)
        public static void ResolveCollision(RigidBody bodyA, RigidBody bodyB, Vector2 normal, float depth)
        {
            Vector2 relativevelocity = Vector2.Zero;

            float e = 0;
            if (bodyA != null && bodyB != null)
            {
                relativevelocity = bodyB.Velocity - bodyA.Velocity;
                e = Math.Min(bodyA.bounciness, bodyB.bounciness);
            }
            else if (bodyA != null && bodyB == null)
            {
                relativevelocity = Vector2.Zero - bodyA.Velocity;
                e = bodyA.bounciness;
            }
            else if (bodyA == null && bodyB != null)
            {
                relativevelocity = bodyB.Velocity;
                e = bodyB.bounciness;
            }
            else
            {
                //Both bodies don't exist
                return;
            }

            float dot = Vector2.Dot(relativevelocity, normal);

            if (dot > 0)
            {
                return;
            }

            float j = -(1 + e) * dot;
            if (bodyA != null && bodyB != null)
            {
                j /= (1 / bodyA.mass) + (1 / bodyB.mass);
            }
            else if (bodyA != null && bodyB == null)
            {
                j /= (1 / bodyA.mass);
            }
            else if (bodyA == null && bodyB != null)
            {
                j /= (1 / bodyB.mass);
            }

            if (bodyA != null && bodyA.resolveCollisions)
            {
                bodyA.Velocity -= (j / bodyA.mass * normal);
            }
            if(bodyB != null && bodyB.resolveCollisions)
            {
                bodyB.Velocity += (j / bodyB.mass * normal);
            }
        }

        //Resolve all Friction (Slow Objects down)
        public static void ResolveFriction(RigidBody bodyA, Collider colliderA, RigidBody bodyB, Collider colliderB, Vector2 normal)
        {
            Vector2 relativevelocity = Vector2.Zero;
            if (bodyA != null && bodyB != null)
            {
                if(bodyA.Velocity == bodyB.Velocity)
                {
                    return;
                }
                relativevelocity = bodyB.Velocity - bodyA.Velocity;
            }
            else if (bodyA != null && bodyB == null)
            {
                relativevelocity = Vector2.Zero - bodyA.Velocity;
            }
            else if (bodyA == null && bodyB != null)
            {
                relativevelocity = bodyB.Velocity;
            }
            else
            {
                return;
            }

            if(float.IsNaN(relativevelocity.X) || float.IsNaN(relativevelocity.Y) || relativevelocity.Length() == 0)
            {
                return;
            }

            float friction = 0;
            if(colliderA != null && colliderB != null)
            {
                friction += (colliderA.friction * colliderB.friction)/2;
            }
            else if(colliderA != null && colliderB == null)
            {
                friction = colliderA.friction;
            }
            else if(colliderA == null && colliderB != null)
            {
                friction = colliderB.friction;
            }
            else
            {
                return;
            }


            if (bodyA != null && bodyA.resolveCollisions)
            {
                float gravity = bodyA.Gravity.Length();
                if(gravity == 0)
                {
                    gravity = 1f;
                }
                bodyA.Velocity += Vector2.Normalize(relativevelocity)* gravity * friction;
            }
            if (bodyB != null && bodyB.resolveCollisions)
            {
                float gravity = bodyB.Gravity.Length();
                if (gravity == 0)
                {
                    gravity = 1f;
                }
                bodyB.Velocity -= Vector2.Normalize(relativevelocity) * gravity * friction;
            }
        }
        
        //Updates Physics steps
        public static void UpdatePhysics()
        {
            for(int i=0; i< rigidbodies.Count; i+=1)
            {
                RigidBody rb = rigidbodies[i];
                if(rb == null || rb.GameObject.OutOfBounds)
                {
                    continue;
                }
                rb.Step(physicsIterations, Time.DeltaTime);
            }
        }

        public static void Update()
        {
            if (physicsIterations < 1)
                physicsIterations = 1;

            for (int it = 0; it < physicsIterations; it += 1)
            {
                UpdatePhysics();
                CheckCollisions();
            }
        }
    }
}
