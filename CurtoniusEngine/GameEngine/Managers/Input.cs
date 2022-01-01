using System.Collections.Generic;
using System.Windows.Forms;
using System.Numerics;

namespace GameEngine
{
    //Mouse
    public struct Mouse
    {
        //Button pressed
        public MouseButtons button;
        //Mouse position
        public Vector2 position;
        public Vector2 screenPosition;
        //Scroll Wheel movement
        public float mouseWheelDelta;

        //Create a new mouse
        public Mouse(MouseButtons b, Vector2 l, float m)
        {
            button = b;
            Vector2 cameraSize = Engine.screenSize / Camera.Scale;
            screenPosition = l;
            position = (((l+Camera.Position) / Engine.screenSize) * cameraSize) - (cameraSize * (1-Camera.Scale))/2;
            mouseWheelDelta = m;
        }
    }

    //Axis
    public struct Axis
    {
        //Keys that will yield either a -1 or 1
        public Keys positive;
        public Keys negative;

        public Keys altPositive;
        public Keys altNegative;

        //Create a new Axis with two pos/neg combos
        public Axis(Keys pos, Keys neg, Keys altP, Keys altN)
        {
            positive = pos;
            negative = neg;
            altPositive = altP;
            altNegative = altN;
        }

        //Create a new Axis with only one pos/neg combo
        public Axis(Keys pos, Keys neg)
        {
            positive = altPositive = pos;
            negative = altNegative = neg;
        }
    }

    public static class Input
    {
        //Keyboard Input
        public static HashSet<Keys> keysDown = new HashSet<Keys>();
        public static HashSet<Keys> keysUp = new HashSet<Keys>();
        public static Dictionary<Keys,KeyEventArgs> keys =  new Dictionary<Keys,KeyEventArgs>();
        private static Dictionary<string, Axis> axes = new Dictionary<string, Axis>()
        {
            { "Horizontal", new Axis(Keys.D, Keys.A, Keys.Right, Keys.Left) },
            { "Vertical", new Axis(Keys.S, Keys.W, Keys.Down, Keys.Up) },
            { "HorizontalArrows", new Axis(Keys.Right, Keys.Left) },
            { "VerticalArrows", new Axis(Keys.Down, Keys.Up) },
            { "HorizontalWASD", new Axis(Keys.D, Keys.A) },
            { "VerticalWASD", new Axis(Keys.S, Keys.W) },
        };

        //Mouse Input
        public static Mouse mouse = new Mouse(MouseButtons.None, Vector2.Zero, 0);
        public static HashSet<MouseButtons> mouseDown = new HashSet<MouseButtons>();
        public static HashSet<MouseButtons> mouseUp = new HashSet<MouseButtons>();
        public static Dictionary<MouseButtons, MouseEventArgs> mouseButtons = new Dictionary<MouseButtons, MouseEventArgs>();

        private static HashSet<Collider> mouseInteractables = new HashSet<Collider>();
        private static HashSet<Collider> mouseInside = new HashSet<Collider>();
        private static HashSet<Collider> mouseDownOn = new HashSet<Collider>();

        private static HashSet<UI> mouseInteractablesUI = new HashSet<UI>();
        private static HashSet<UI> mouseInsideUI = new HashSet<UI>();
        private static HashSet<UI> mouseDownOnUI = new HashSet<UI>();
        //Get Axis value
        public static int GetAxis(string axisName)
        {
            //Does the axis exist
            if(axes.ContainsKey(axisName))
            {
                Axis axis = axes[axisName];

                //Go through list of all keys being pressed
                foreach (KeyEventArgs e in keys.Values)
                {
                    //If the key is one of the axis positive keys, return 1
                    if (e.KeyCode == axis.positive || e.KeyCode == axis.altPositive)
                    {
                        return 1;
                    }
                    //If the key is one of the axis negative keys, return -1
                    else if (e.KeyCode == axis.negative || e.KeyCode == axis.altNegative)
                    {
                        return -1;
                    }
                }

                //If the axis isn't being used return 0
                return 0;
            }
            else
            {
                //If the axis doesn't exist return 0
                return 0;
            }
        }

        //Was this key just pressed down this frame
        public static bool GetKeyDown(Keys key)
        {
            if (keysDown.Contains(key))
            {
                return true;
            }
            return false;
        }

        //Was this key just released this frame
        public static bool GetKeyUp(Keys key)
        {
            if (keysUp.Contains(key))
            {
                return true;
            }
            return false;
        }

        //Is this key being pressed down
        public static bool GetKey(Keys key)
        {
            if (keys.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        
        //Was this mouse button just pressed down this frame
        public static bool GetMouseButtonDown(MouseButtons b)
        {
            if (mouseDown.Contains(b))
            {
                return true;
            }
            return false;
        }

        //Was this mouse button just released this frame
        public static bool GetMouseButtonUp(MouseButtons b)
        {
            if (mouseUp.Contains(b))
            {
                return true;
            }
            return false;
        }

        //Is this mouse button being pressed down
        public static bool GetMouseButton(MouseButtons b)
        {
            if (mouseButtons.ContainsKey(b))
            {
                return true;
            }
            return false;
        }

        //Completely reset Input
        public static void ResetEventsFromLoadScene()
        {
            mouseDown.Clear();
            mouseUp.Clear();
            mouseButtons.Clear();

            mouseInteractables.Clear();
            mouseInside.Clear();
            mouseDownOn.Clear();

            mouseInteractablesUI.Clear();
            mouseInsideUI.Clear();
            mouseDownOnUI.Clear();

            keysDown.Clear();
            keysUp.Clear();
            keys.Clear();
        }

        //At the end of the frame clear the events that require only a singular frame for response
        public static void ClearEvents()
        {
            foreach (UI ui in mouseInteractablesUI)
            {
                bool colliding = Collisions.IntersectCirclePolygon(mouse.screenPosition, 0.01f, ui.GameObject.transformedVertices);

                if (colliding && !mouseInsideUI.Contains(ui))
                {
                    ui.OnMouseEnter();
                    mouseInsideUI.Add(ui);
                }
                else if (colliding && mouseInsideUI.Contains(ui))
                {
                    ui.OnMouseStay();
                }
                else if (!colliding && mouseInsideUI.Contains(ui))
                {
                    ui.OnMouseExit();
                    mouseInsideUI.Remove(ui);
                }

                if (colliding && mouseDown.Contains(MouseButtons.Left))
                {
                    if (!mouseDownOnUI.Contains(ui))
                    {
                        mouseDownOnUI.Add(ui);
                    }
                    ui.OnMouseDown();
                }
                else if (colliding && mouseUp.Contains(MouseButtons.Left))
                {
                    ui.OnMouseUp();
                }

                if (colliding && mouseDownOnUI.Contains(ui) && mouseUp.Contains(MouseButtons.Left))
                {
                    ui.OnMouseClick();
                }
            }
            foreach (Collider collider in mouseInteractables)
            {
                bool colliding = false;
                if (collider.Shape == CollisionShape.BOX)
                {
                    colliding = Collisions.IntersectCirclePolygon(mouse.position, 0.01f, collider.GameObject.transformedVertices);
                }
                else
                {
                    colliding = Collisions.IntersectCircle(mouse.position, 0.01f, collider.GameObject.Position, collider.GameObject.Size.X / 2);
                }

                if(colliding && !mouseInside.Contains(collider))
                {
                    collider.OnMouseEnter();
                    mouseInside.Add(collider);
                }
                else if(colliding && mouseInside.Contains(collider))
                {
                    collider.OnMouseStay();
                }
                else if(!colliding && mouseInside.Contains(collider))
                {
                    collider.OnMouseExit();
                    mouseInside.Remove(collider);
                }

                if (colliding && mouseDown.Contains(MouseButtons.Left))
                {
                    if(!mouseDownOn.Contains(collider))
                    {
                        mouseDownOn.Add(collider);
                    }
                    collider.OnMouseDown();
                }
                else if (colliding && mouseUp.Contains(MouseButtons.Left))
                {
                    collider.OnMouseUp();
                }

                if (colliding && mouseDownOn.Contains(collider) && mouseUp.Contains(MouseButtons.Left))
                {
                    collider.OnMouseClick();
                }
            }

            if(mouseUp.Contains(MouseButtons.Left))
            {
                mouseDownOnUI.Clear();
                mouseDownOn.Clear();
            }

            keysDown.Clear();
            keysUp.Clear();
            mouseDown.Clear();
            mouseUp.Clear();
            mouse.mouseWheelDelta = 0;
        }

        //Add Mouse Interactables for Colliders and UI
        public static void AddMouseInteractable(Collider interactable)
        {
            if (!mouseInteractables.Contains(interactable))
            {
                mouseInteractables.Add(interactable);
            }
        }
        public static void RemoveMouseInteractable(Collider interactable)
        {
            if (mouseInteractables.Contains(interactable))
            {
                mouseInteractables.Remove(interactable);
            }
        }
        public static void AddMouseInteractableUI(UI interactable)
        {
            if (!mouseInteractablesUI.Contains(interactable))
            {
                mouseInteractablesUI.Add(interactable);
            }
        }
        public static void RemoveMouseInteractableUI(UI interactable)
        {
            if (mouseInteractablesUI.Contains(interactable))
            {
                mouseInteractablesUI.Remove(interactable);
            }
        }
    }
}
