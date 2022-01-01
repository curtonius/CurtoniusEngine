using System;
using System.Collections.Generic;
using System.Numerics;

namespace GameEngine
{
    //Shape of collider
    public enum CollisionShape { BOX, CIRCLE };

    //Delegates for Mouse interactions / Collision interactions
    public delegate void CollisionHandle(Collider other);
    public delegate void InteractionHandle();

    //Default Collider
    public class Collider : Component
    {
        //Shape of Collider
        public CollisionShape Shape = CollisionShape.BOX;

        //Does this Collider have a Rigidbody
        public bool hasPhysics = false;

        //Does this Collider have Collision Response or is it like Air
        public bool isTrigger = false;

        //Should this Collider only detect collision with an non-engine script, or include Physics Collision
        public bool manualDetectOnly = false;

        //Friction of Collider
        public float friction = 0;

        //Collision Events
        //When the collision happens
        private Delegate onCollisionEnter = null;
        //While the collision happens
        private Delegate onCollisionStay = null;
        //When the collision stops
        private Delegate onCollisionExit = null;

        //Information about what colliders are interacting
        private HashSet<Collider> collidingWith = new HashSet<Collider>();
        private HashSet<Collider> notCollidingWith = new HashSet<Collider>();
        private HashSet<Collider> colliders = new HashSet<Collider>();

        //If this Collider can be interacted with, with the mouse
        public bool Interactable
        {
            get { return interactable; }
            set
            {
                if (value != interactable)
                {
                    interactable = value;
                    if (interactable)
                    {
                        Input.AddMouseInteractable(this);
                    }
                    else
                    {
                        Input.RemoveMouseInteractable(this);
                    }
                }
            }
        }
        private bool interactable;

        //Mouse Interaction Events
        //When the mouse hovers over
        private Delegate mouseEnter = null;
        //When the mouse stops hovering over
        private Delegate mouseExit = null;
        //While the mouse is hovering over
        private Delegate mouseStay = null;
        //When the mouse presses down over
        private Delegate mouseDown = null;
        //When the mouse button is released over
        private Delegate mouseUp = null;
        //When the mouse fully clicks over
        private Delegate mouseClick = null;

        #region Mouse Interaction Events
        //Adding, Removing, and Calling Mouse Interactions
        public void AddMouseEnterEvent(InteractionHandle listener)
        {
            mouseEnter = (InteractionHandle)mouseEnter + listener;
        }
        public void RemoveMouseEnterEvent()
        {
            if (mouseEnter == null) return;
            foreach (Delegate d in mouseEnter.GetInvocationList())
            {
                mouseEnter = (InteractionHandle)mouseEnter - (InteractionHandle)d;
            }
        }
        public void AddMouseExitEvent(InteractionHandle listener)
        {
            mouseExit = (InteractionHandle)mouseExit + listener;
        }
        public void RemoveMouseExitEvent()
        {
            if (mouseExit == null) return;
            foreach (Delegate d in mouseExit.GetInvocationList())
            {
                mouseExit = (InteractionHandle)mouseExit - (InteractionHandle)d;
            }
        }
        public void AddMouseStayEvent(InteractionHandle listener)
        {
            mouseStay = (InteractionHandle)mouseStay + listener;
        }
        public void RemoveMouseStayEvent()
        {
            if (mouseStay == null) return;
            foreach (Delegate d in mouseStay.GetInvocationList())
            {
                mouseStay = (InteractionHandle)mouseStay - (InteractionHandle)d;
            }
        }
        public void AddMouseDownEvent(InteractionHandle listener)
        {
            mouseDown = (InteractionHandle)mouseDown + listener;
        }
        public void RemoveMouseDownEvent()
        {
            if (mouseDown == null) return;
            foreach (Delegate d in mouseDown.GetInvocationList())
            {
                mouseDown = (InteractionHandle)mouseDown - (InteractionHandle)d;
            }
        }
        public void AddMouseUpEvent(InteractionHandle listener)
        {
            mouseUp = (InteractionHandle)mouseUp + listener;
        }
        public void RemoveMouseUpEvent()
        {
            if (mouseUp == null) return;
            foreach (Delegate d in mouseUp.GetInvocationList())
            {
                mouseUp = (InteractionHandle)mouseUp - (InteractionHandle)d;
            }
        }
        public void AddMouseClickEvent(InteractionHandle listener)
        {
            mouseClick = (InteractionHandle)mouseClick + listener;
        }
        public void RemoveMouseClickEvent()
        {
            if (mouseClick == null) return;
            foreach (Delegate d in mouseClick.GetInvocationList())
            {
                mouseClick = (InteractionHandle)mouseClick - (InteractionHandle)d;
            }
        }

        public void OnMouseEnter()
        {
            if (mouseEnter == null)
            {
                return;
            }
            (mouseEnter as InteractionHandle)();
        }
        public void OnMouseExit()
        {
            if (mouseExit == null)
            {
                return;
            }
            (mouseExit as InteractionHandle)();
        }
        public void OnMouseStay()
        {
            if (mouseStay == null)
            {
                return;
            }
            (mouseStay as InteractionHandle)();
        }
        public void OnMouseDown()
        {
            if (mouseDown == null)
            {
                return;
            }
            (mouseDown as InteractionHandle)();
        }
        public void OnMouseUp()
        {
            if (mouseUp == null)
            {
                return;
            }
            (mouseUp as InteractionHandle)();
        }
        public void OnMouseClick()
        {
            if (mouseClick == null)
            {
                return;
            }
                    (mouseClick as InteractionHandle)();
        }
        #endregion

        //Simple Collider
        public Collider()
        {
            ClassName = "Collider";
        }

        //Clear all Collision information
        public void ClearCollisions()
        {
            collidingWith.Clear();
            notCollidingWith.Clear();
        }

        //Start calling OnCollisionEnter and OnCollisionStay events
        public void AddCollider(Collider other, bool isColliding)
        {
            if(isColliding)
            {
                collidingWith.Add(other);
                if(!colliders.Contains(other))
                {
                    other.OnCollisionEnter(this);
                    OnCollisionEnter(other);
                    colliders.Add(other);
                }
                else
                {
                    other.OnCollisionStay(this);
                    OnCollisionStay(other);
                }
            }
            else
            {
                notCollidingWith.Add(other);
            }
        }

        //Call OnCollisionExit events
        public void CheckColliders()
        {
            foreach(Collider not in notCollidingWith)
            {
                if(colliders.Contains(not))
                {
                    not.OnCollisionExit(this);
                    OnCollisionExit(not);
                    colliders.Remove(not);
                }
            }
        }

        #region Collision Events
        //Add, Remove, and Call Collision Events
        public void AddCollisionEnterEvent(CollisionHandle listener)
        {
            onCollisionEnter = (CollisionHandle)onCollisionEnter + listener;
        }
        public void AddCollisionExitEvent(CollisionHandle listener)
        {
            onCollisionExit = (CollisionHandle)onCollisionExit + listener;
        }
        public void AddCollisionStayEvent(CollisionHandle listener)
        {
            onCollisionStay = (CollisionHandle)onCollisionStay + listener;
        }

        public void RemoveCollisionEnterEvents()
        {
            if (onCollisionEnter == null) return;
            foreach (Delegate d in onCollisionEnter.GetInvocationList())
            {
                onCollisionEnter = (CollisionHandle)onCollisionEnter - (CollisionHandle)d;
            }
        }
        public void RemoveCollisionStayEvents()
        {
            if (onCollisionStay == null) return;
            foreach (Delegate d in onCollisionStay.GetInvocationList())
            {
                onCollisionStay = (CollisionHandle)onCollisionStay - (CollisionHandle)d;
            }
        }
        public void RemoveCollisionExitEvents()
        {
            if (onCollisionExit == null) return;
            foreach (Delegate d in onCollisionExit.GetInvocationList())
            {
                onCollisionExit = (CollisionHandle)onCollisionExit - (CollisionHandle)d;
            }
        }

        public void OnCollisionEnter(Collider other)
        {
            if (onCollisionEnter == null)
            {
                return;
            }
            (onCollisionEnter as CollisionHandle)(other);
        }
        public void OnCollisionExit(Collider other)
        {
            if (onCollisionExit == null)
            {
                return;
            }
            (onCollisionExit as CollisionHandle)(other);
        }
        public void OnCollisionStay(Collider other)
        {
            if (onCollisionStay == null)
            {
                return;
            }
            (onCollisionStay as CollisionHandle)(other);
        }
        #endregion
        
        //Add Collider and check if it has a Rigidbody
        public override void ComponentAdded()
        {
            if(GameObject.GetComponent<RigidBody>() != null)
            {
                hasPhysics = true;
            }
        }

        //
        public override void ComponentRemoved()
        {

        }
    }
}
