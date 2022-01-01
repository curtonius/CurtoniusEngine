using System;
using System.Collections.Generic;
using System.Numerics;


namespace GameEngine
{
    //A collider that is in the shape of a circl, resizes with GameObject
    public class CircleCollider : Collider
    {
        //Create a simple Circle Collider
        public CircleCollider()
        {
            ClassName = "CircleCollider";
            Shape = CollisionShape.CIRCLE;
        }

        //Add Collider to PhysicsManager
        public override void ComponentAdded()
        {
            PhysicsManager.AddCollider(this);
        }

        //Remove Collider from PhysicsManager
        public override void ComponentRemoved()
        {
            PhysicsManager.RemoveCollider(this);
        }
    }
}
