using System;
using System.Collections.Generic;
using System.Numerics;


namespace GameEngine
{
    //A collider that is in the shape of a rectangle, rotates with GameObject, resizes with GameObject
    public class BoxCollider : Collider
    {
        //Create a simple Box Collider
        public BoxCollider()
        {
            ClassName = "BoxCollider";
            Shape = CollisionShape.BOX;
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
