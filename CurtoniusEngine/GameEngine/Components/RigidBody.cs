using System.Numerics;

namespace GameEngine
{
    //Give GameObject physics
    class RigidBody : Component
    {
        //How should this Object move when no other forces are acting on it
        public Vector2 Gravity = Vector2.Zero;

        //Should we lock a specific aspect of the movement of this object
        public bool LockXPos = false;
        public bool LockYPos = false;
        public bool LockRot = false;

        //Should this bounce or just move away from collisions
        public bool resolveCollisions = true;

        //How fast should this object be moving
        public Vector2 Velocity = Vector2.Zero;
        //How fast should this object be rotating
        public float AngularVelocity = 0;

        //How fast should this object be slowing down
        public float linearDrag = 2f;
        public float angularDrag = 2f;

        //How much mass does this object have?
        public float mass = 1;
        public float bounciness = 0.5f;

        //Forces being applied
        private Vector2 force;

        public RigidBody()
        {
            ClassName = "Rigidbody";
        }

        //Add force to the Rigidbody
        public void AddForce(Vector2 f)
        {
            force += f;
        }

        //Do physics step
        public void Step(int iterations, float delta)
        {
            float time = (delta / iterations)/10;

            Vector2 acceleration = force / mass;
            Vector2 vel = Velocity + (acceleration + Gravity);
            if (vel != Vector2.Zero)
            {
                vel += Vector2.Normalize(-vel) * linearDrag;
            }
            if (AngularVelocity < 0)
            {
                AngularVelocity += linearDrag;
            }
            else if (AngularVelocity > 0)
            {
                AngularVelocity -= linearDrag;
            }

            if(LockXPos)
            {
                vel.X = 0;
            }
            if(LockYPos)
            {
                vel.Y = 0;
            }
            if(LockRot)
            {
                AngularVelocity = 0;
            }

            GameObject.Position += vel * time;
            GameObject.Rotation += AngularVelocity * time;

            force = Vector2.Zero;
        }

        //Add to PhysicsManager
        public override void ComponentAdded()
        {
            PhysicsManager.AddPhysics(this);
            if (GameObject.GetComponent<Collider>() != null)
            {
                GameObject.GetComponent<Collider>().hasPhysics = true;
            }
        }

        //Remove from PhysicsManager
        public override void ComponentRemoved()
        {
            PhysicsManager.RemovePhysics(this);
            if (GameObject.GetComponent<Collider>() != null)
            {
                GameObject.GetComponent<Collider>().hasPhysics = false;
            }
        }
    }
}
