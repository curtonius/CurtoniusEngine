using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Drawing;

namespace GameEngine
{
    //Standard GameObject that can hold Components
    public class GameObject
    {
        //Transform variables
        public Vector2 Position { get { return position; } set { position = value; UpdateTransformedVertices(); CheckCameraBounds();  CheckPhysicsBounds(); } }
        public Vector2 Size { get { return size; } set { size = value; UpdateVertices(); UpdateTransformedVertices(); } }
        public float Rotation { get { return rotation; } set { rotation = value; UpdateTransformedVertices(); } }
        public Vector2 Pivot { get{ return pivot; } set { pivot = value; UpdateVertices(); UpdateTransformedVertices(); } }

        //Directional Variables based on Rotation
        public Vector2 Right { get { return new Vector2(1, 0).Rotate(rotation); } }
        public Vector2 Up { get { return new Vector2(0, 1).Rotate(rotation); } }

        //Rendering points
        public Vector2[] vertices = new Vector2[4];
        public Vector2[] transformedVertices = new Vector2[4];
        public Point[] points = new Point[4];

        //Physics and Camera bounds checks
        public bool OutOfBounds = false;
        public bool OutOfCameraBounds = false;

        //Components added to GameObject
        private Dictionary<string, Component> components = new Dictionary<string, Component>();

        //Transform variables again
        private Vector2 pivot = new Vector2(0.5f,0.5f);
        private Vector2 position = Vector2.Zero;
        private float rotation = 0;
        private Vector2 size = Vector2.Zero;

        //Has this GameObject been destroyed
        public bool Destroyed = false;

        //Tag of GameObject
        public string tag = "Default";

        //Should this GameObject have an update
        public bool UpdateGameObject = true;

        //Should this GameObject be destroyed on Load
        public bool DontDestroyOnLoad = false;

        //Add GameObject to GameObjectManager
        public virtual void Initialize()
        {
            GameObjectManager.gameObjects.Add(this);
        }

        #region Constructors
        //Default Constructor
        public GameObject()
        {
            Position = Vector2.Zero;
            Size = Engine.TileSize;
            Rotation = 0;
            UpdateVertices();
            Initialize();
        }

        //Constructor with Position
        public GameObject(Vector2 position)
        {
            Position = position;
            Size = Engine.TileSize;
            Rotation = 0;
            UpdateVertices();
            Initialize();
        }

        //Constructor with Position and Size
        public GameObject(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = Engine.TileSize;
            Rotation = 0;
            UpdateVertices();
            Initialize();
        }

        //Constructor with Position, Size, and Rotation
        public GameObject(Vector2 position, Vector2 size, float rotation)
        {
            Position = position;
            Size = size;
            Rotation = rotation;
            UpdateVertices();
            GameObjectManager.gameObjects.Add(this);
        }

        #endregion

        #region Copy Constructor
        //Copy Constructor for a GameObject
        public GameObject(GameObject clone)
        {
            Position = clone.Position;
            Size = clone.Size;
            Rotation = clone.Rotation;
            components = clone.GetAllComponents();
        }
        #endregion

        #region Components
        //Return Dictionary for all Components in this GameObject
        public Dictionary<string, Component> GetAllComponents()
        {
            return components;
        }

        //Find the first component in this GameObject of this type
        public T GetComponent<T>() where T : Component
        {
            //Go through all Components
            foreach(Component component in components.Values)
            {
                //If this component is the same type as what we're trying to find, return it
                if (component.GetType().IsSubclassOf(typeof(T)) || component.GetType() == typeof(T))
                {
                    return component as T;
                }
            }

            return null;
        }

        //Find the component in this GameObject with this class name
        public Component GetComponent(string className)
        {
            //If the components dictionary contains this className then return it
            if(components.ContainsKey(className))
            {
                return components[className];
            }
            return null;
        }

        //Add component of type T to GameObject and then return it
        public T AddComponent<T>() where T : Component
        {
            T addComponent = (T)Activator.CreateInstance(typeof(T));

            //If this component is not already on the GameObject
            if (!components.ContainsKey(addComponent.ClassName))
            {
                addComponent.GameObject = this;
                addComponent.ComponentAdded();
                components.Add(addComponent.ClassName, addComponent);
                return addComponent;
            }
            else
            {
                return components[addComponent.ClassName] as T;
            }
        }

        //Remove component of type T from GameObject
        public void RemoveComponent<T>() where T : Component
        {
            T testRemove = (T)Activator.CreateInstance(typeof(T));
            //Search through GameObject components
            foreach(Component component in components.Values)
            {
                //Check if this Component is typeof T or a Subclass of T
                if(component.GetType().IsSubclassOf(typeof(T)) || component.GetType() == typeof(T))
                {
                    component.ComponentRemoved();
                    components.Remove(component.ClassName);
                }
            }
        }

        //Remove component with this classname
        public void RemoveComponent(string className)
        {
            if(components.ContainsKey(className))
            {
                components[className].ComponentRemoved();
                components.Remove(className);
            }
        }
        #endregion

        //Update gameObject
        public virtual void Update() { }
        
        //Destroy GameObject and remove all components
        public virtual void Destroy()
        {
            List<string> keys = components.Keys.ToList();
            foreach (string c in keys)
            {
                RemoveComponent(c);
            }
            GameObjectManager.gameObjects.Remove(this);
            Destroyed = true;
        }
        
        //Update rendering
        public void UpdateVertices()
        {
            Vector2 topLeft = -new Vector2(pivot.X * Size.X, -pivot.Y * Size.Y);
            //Top Left
            vertices[0] = new Vector2(topLeft.X, topLeft.Y);
            //Top Right
            vertices[1] = new Vector2((topLeft.X + Size.X), topLeft.Y);
            //Bottom Right
            vertices[2] = new Vector2((topLeft.X + Size.X), (topLeft.Y - Size.Y));
            //Bottom Left
            vertices[3] = new Vector2((topLeft.X), (topLeft.Y - Size.Y));
        }
        public void UpdateTransformedVertices()
        {
            transformedVertices = new Vector2[4]
            {
                    Position + new Vector2(vertices[0].X,vertices[0].Y).Rotate(rotation),
                    Position + new Vector2(vertices[1].X,vertices[1].Y).Rotate(rotation),
                    Position + new Vector2(vertices[2].X,vertices[2].Y).Rotate(rotation),
                    Position + new Vector2(vertices[3].X,vertices[3].Y).Rotate(rotation),
            };

            points = new Point[4]
            {
                    new Point((int)(transformedVertices[0].X), (int)(transformedVertices[0].Y)),
                    new Point((int)(transformedVertices[1].X), (int)(transformedVertices[1].Y)),
                    new Point((int)(transformedVertices[2].X), (int)(transformedVertices[2].Y)),
                    new Point((int)(transformedVertices[3].X), (int)(transformedVertices[3].Y))
            };
        }

        public void CheckCameraBounds()
        {
            Vector2 actualSize = GameObjectManager.actualSize;
            Vector2 posCheck = GameObjectManager.posCheck;

            bool onScreen = false;
            for (int i = 0; i < transformedVertices.Length; i += 1)
            {
                if (transformedVertices[i].X > -actualSize.X + posCheck.X - Size.X &&
                    transformedVertices[i].X < actualSize.X + posCheck.X + Size.X &&
                    transformedVertices[i].Y > -actualSize.Y + posCheck.Y - Size.Y &&
                    transformedVertices[i].Y < actualSize.Y + posCheck.Y + Size.Y)
                {
                    onScreen = true;
                }
            }

            if (!onScreen)
            {
                OutOfCameraBounds = true;
            }
            else
            {
                OutOfCameraBounds = false;
            }
        }

        void CheckPhysicsBounds()
        {
            if (GetComponent<RigidBody>() == null)
                return;

            if (Position.X < Engine.worldBoundsMin.X || Position.X > Engine.worldBoundsMax.X
                || Position.Y < Engine.worldBoundsMin.Y || Position.Y > Engine.worldBoundsMax.Y)
            {
                OutOfBounds = true;

                switch(PhysicsManager.outOfBoundsHandling)
                {
                    case OutOfBoundsHandling.Destroy:
                        Destroy();
                        break;
                    case OutOfBoundsHandling.Wrap:
                        float moveX = Position.X;
                        float moveY = Position.Y;

                        if (Position.X < Engine.worldBoundsMin.X)
                        {
                            moveX = Engine.worldBoundsMax.X;
                        }
                        else if (Position.X > Engine.worldBoundsMax.X)
                        {
                            moveX = Engine.worldBoundsMin.X;
                        }
                        if (Position.Y < Engine.worldBoundsMin.Y)
                        {
                            moveY = Engine.worldBoundsMax.Y;
                        }
                        else if (Position.Y > Engine.worldBoundsMax.Y)
                        {
                            moveY = Engine.worldBoundsMin.Y;
                        }

                        Position = new Vector2(moveX, moveY);
                        break;
                }
            }

            OutOfBounds = false;
        }
    }
}
