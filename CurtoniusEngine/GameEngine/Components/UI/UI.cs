using System;
using System.Drawing;

namespace GameEngine
{
    //Base class of all renderer objects
    public class UI : Component
    {
        //What order to draw renderer in. 0 is first
        public int layer { get { return Layer; } set { int oldLayer = Layer; if (oldLayer != value) { Layer = value; UpdateLayer(oldLayer, Layer); } } }
        private int Layer = 0;

        //Color of UI component
        public Color Color = Color.White;

        //If this UI should render or not
        public bool Visible = true;

        //Should this UI have mouse interactions
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
                        Input.AddMouseInteractableUI(this);
                    }
                    else
                    {
                        Input.RemoveMouseInteractableUI(this);
                    }
                }
            }
        }
        private bool interactable;

        //Mouse interaction delegates
        //When the mouse hovers over
        private Delegate mouseEnter = null;
        //When the mouse stops hovering over
        private Delegate mouseExit = null;
        //While the mouse hovers over
        private Delegate mouseStay = null;
        //When the mouse is pressed down over
        private Delegate mouseDown = null;
        //When the mouse button is released over
        private Delegate mouseUp = null;
        //When the mouse fully clicks over
        private Delegate mouseClick = null;

        #region Mouse Interaction Events
        //Add, Remove, and Call Mouse interaction events
        public void AddMouseEnterEvent(InteractionHandle listener)
        {
            mouseEnter = (InteractionHandle)mouseEnter + listener;
        }
        public void AddMouseExitEvent(InteractionHandle listener)
        {
            mouseExit = (InteractionHandle)mouseExit + listener;
        }
        public void AddMouseStayEvent(InteractionHandle listener)
        {
            mouseStay = (InteractionHandle)mouseStay + listener;
        }
        public void AddMouseDownEvent(InteractionHandle listener)
        {
            mouseDown = (InteractionHandle)mouseDown + listener;
        }
        public void AddMouseUpEvent(InteractionHandle listener)
        {
            mouseUp = (InteractionHandle)mouseUp + listener;
        }
        public void AddMouseClickEvent(InteractionHandle listener)
        {
            mouseClick = (InteractionHandle)mouseClick + listener;
        }

        public void ClearMouseEnterEvents()
        {
            if (mouseEnter == null) return;
            foreach (Delegate d in mouseEnter.GetInvocationList())
            {
                mouseEnter = (InteractionHandle)mouseEnter - (InteractionHandle)d;
            }
        }
        public void ClearMouseExitEvents()
        {
            if (mouseExit == null) return;
            foreach (Delegate d in mouseExit.GetInvocationList())
            {
                mouseExit = (InteractionHandle)mouseExit - (InteractionHandle)d;
            }
        }
        public void ClearMouseStayEvents()
        {
            if (mouseStay == null) return;
            foreach (Delegate d in mouseStay.GetInvocationList())
            {
                mouseStay = (InteractionHandle)mouseStay - (InteractionHandle)d;
            }
        }
        public void ClearMouseDownEvents()
        {
            if (mouseDown == null) return;
            foreach (Delegate d in mouseDown.GetInvocationList())
            {
                mouseDown = (InteractionHandle)mouseDown - (InteractionHandle)d;
            }
        }
        public void ClearMouseUpEvents()
        {
            if (mouseUp == null) return;
            foreach (Delegate d in mouseUp.GetInvocationList())
            {
                mouseUp = (InteractionHandle)mouseUp - (InteractionHandle)d;
            }
        }
        public void ClearMouseClickEvents()
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

        public UI()
        {
            ClassName = "UI";
            layer = 0;
        }

        public UI(int layerValue)
        {
            ClassName = "UI";
            layer = layerValue;
        }

        //Update layer in UIManager
        public void UpdateLayer(int old, int now)
        {
            UIManager.UpdateLayer(this, old, now);
        }

        //Add Component to RenderManager
        public override void ComponentAdded()
        {
            UIManager.AddUI(this);
        }

        //Remove Component from RenderManager
        public override void ComponentRemoved()
        {
            Input.RemoveMouseInteractableUI(this);
            UIManager.RemoveUI(this);
        }

        //Virtual draw function to draw the renderer
        public virtual void Draw(Graphics g) { }
    }
}
