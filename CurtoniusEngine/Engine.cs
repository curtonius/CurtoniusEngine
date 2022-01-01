using System;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace GameEngine
{
    //The window for the game to render
    class Canvas : Form
    {
        public Canvas()
        {
            //prevent flicker
            this.DoubleBuffered = true;
        }
    }

    //The Game Engine
    public abstract class Engine
    {
        //The background color of the window
        public Color backgroundColor = Color.Aqua;

        //Window variables
        public static Vector2 screenSize = new Vector2(800, 600);
        private string title = "New Game";
        private Canvas window = null;

        //Thread to host gameloop
        private Thread GameLoopThread = null;
        static bool finishedRendering = true;

        //World Variables
        public static Vector2 TileSize = new Vector2(64, 64);
        public static Vector2 worldBoundsMin = Vector2.Zero;
        public static Vector2 worldBoundsMax = new Vector2(600, 400);
        public static Scene currentScene;

        public Engine(Vector2 ScreenSize, string Title)
        {
            //Set default variables for Screen
            screenSize = ScreenSize;
            title = Title;

            //Create a new Window
            window = new Canvas();
            window.Size = new Size((int)screenSize.X, (int)screenSize.Y);
            window.Text = title;

            window.ClientSize = window.Size;

            //Set events for window
            //Draw everything
            window.Paint += Renderer;
            //Handle Keyboard Input
            window.KeyDown += KeyDown;
            window.KeyUp += KeyUp;
            //Handle Mouse Input
            window.MouseDown += MouseDown;
            window.MouseUp += MouseUp;
            window.MouseMove += MouseMove;
            window.MouseWheel += MouseWheel;

            //Non-Resiazble
            window.FormBorderStyle = FormBorderStyle.FixedDialog;
            window.MaximizeBox = false;

            //Start GameLoop
            GameLoopThread = new Thread(GameLoop);
            GameLoopThread.Start();

            Application.Run(window);
        }

        void GameLoop()
        {
            //Load everything
            OnLoad();

            Stopwatch stopwatch = Stopwatch.StartNew();
            long currentTime = 0;
            IAsyncResult method =null;
            //While this game is running
            while(GameLoopThread.IsAlive)
            {
                try
                {
                    //get elapsed time
                    Time.TimeElapsed = stopwatch.ElapsedMilliseconds;
                    Time.DeltaTime = (float)((double)Time.TimeElapsed - (double)currentTime) / 1000;
                    currentTime = Time.TimeElapsed;

                    PhysicsManager.Update();
                    
                    AudioManager.UpdateAudioPlayers();

                    //Begin to Start the Thread again
                    method = window.BeginInvoke((MethodInvoker)delegate { window.Refresh(); });
                    
                    //Update Game
                    OnUpdate();

                    //Clear Input for next Frame
                    Input.ClearEvents();

                    //Sleep until rendering has finished
                    if (!finishedRendering)
                    {
                        while (!finishedRendering)
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
                catch
                {
                    //Abort loop
                    GameLoopThread.Abort();
                    Console.WriteLine("Game is loading..");
                }
            }
            stopwatch.Stop();

            //End thread
            if (method != null)
            {
                window.EndInvoke(method);
            }
        }

        //Called from "window.KeyDown += KeyDown;"
        //Whenever a key on the keyboard is pressed
        private void KeyDown(object sender, KeyEventArgs e)
        {
            //If the Input has not been added yet to the Input class
            if(!Input.keys.ContainsKey(e.KeyCode))
            {
                //Add key to the keysDown and add key to keys
                Input.keysDown.Add(e.KeyCode);
                Input.keys.Add(e.KeyCode,e);
            }
        }

        //Called from "window.KeyUp += KeyUp;"
        //Whenever a key on the keyboard is released
        private void KeyUp(object sender, KeyEventArgs e)
        {
            //If the Input has been added yet to the Input class
            if (Input.keys.ContainsKey(e.KeyCode))
            {
                //Remove key from the keysUp and remove key from keys
                Input.keysUp.Add(e.KeyCode);
                Input.keys.Remove(e.KeyCode);
            }
        }

        //Clamp values to a magnitude of 1, unless 0
        int ConvertDelta(float delta)
        {
            if(delta < 0)
            {
                return -1;
            }
            else if(delta > 0)
            {
                return 1;
            }
            return 0;
        }

        //Called from "window.MouseDown += MouseDown;"
        //Whenever a mouse button on the mouse is pressed
        private void MouseDown(object sender, MouseEventArgs e)
        {
            //Create a new Mouse to use in Input functions
            int delta = ConvertDelta(e.Delta);
            Input.mouse = new Mouse(e.Button, new Vector2(e.X, e.Y), -delta);

            //Add mouseDown button to mouseDown
            Input.mouseDown.Add(e.Button);
            //If the mouseButtons does not contain this button, add it
            if (!Input.mouseButtons.ContainsKey(e.Button))
            {
                Input.mouseButtons.Add(e.Button, e);
            }
        }

        //Called from "window.MouseUp += MouseUp;"
        //Whenever a mouse button on the mouse is released
        private void MouseUp(object sender, MouseEventArgs e)
        {
            //Create a new Mouse to use in Input functions
            int delta = ConvertDelta(e.Delta);
            Input.mouse = new Mouse(e.Button, new Vector2(e.X, e.Y), -delta); 
            Input.mouseUp.Add(e.Button);

            //Add mouseDown button to mouseUp
            Input.mouseUp.Add(e.Button);
            //If the mouseButtons does contain this button, remove it
            if (Input.mouseButtons.ContainsKey(e.Button))
            {
                Input.mouseButtons.Remove(e.Button);
            }
        }

        //Called from "window.MouseMove += MouseMove;"
        //Whenever the mouse is moved
        private void MouseMove(object sender, MouseEventArgs e)
        {
            //Create a new Mouse to use in Input functions
            int delta = ConvertDelta(e.Delta);
            Input.mouse = new Mouse(e.Button, new Vector2(e.X, e.Y), -delta);
        }

        //Called from "window.MouseWheel += MouseWheel;"
        //Whenever the mouse scrollwheel is rotated
        private void MouseWheel(object sender, MouseEventArgs e)
        {
            //Create a new Mouse to use in Input functions
            int delta = ConvertDelta(e.Delta);
            Input.mouse = new Mouse(e.Button, new Vector2(e.X, e.Y), -delta);
        }

        //Called from "window.Paint += Renderer;"
        //Whenever the scene repaints on the Canvas
        private void Renderer(object sender, PaintEventArgs e)
        {
            finishedRendering = false;
            screenSize = new Vector2(window.Size.Width, window.Size.Height);

            Graphics g = e.Graphics;
            //Create a background for our game
            g.Clear(backgroundColor);

            //Set Camera Transforms
            if(Camera.Follow != null)
            {
                Vector2 pixelPerfect = Vector2.Lerp(Camera.Position, Camera.Follow.Position * Camera.Scale, 0.05f);
                pixelPerfect = new Vector2((float)Math.Round(pixelPerfect.X), (float)Math.Round(pixelPerfect.Y));
                Camera.Position = pixelPerfect;
            }

            //Keep camera centered
            Vector2 offset = -Camera.Position + ((new Vector2(screenSize.X, screenSize.Y) * Camera.Scale) / 2) + (new Vector2(screenSize.X, screenSize.Y)*(1-Camera.Scale))/2;
            g.TranslateTransform(offset.X , offset.Y);
            g.RotateTransform(Camera.Rotation);
            g.ScaleTransform(Camera.Scale, Camera.Scale);

            //Draw Objects in Scene
            RenderManager.Draw(g);

            g.ResetTransform();

            //Draw Objects in UI
            UIManager.Draw(g);
            finishedRendering = true;
        }

        //Start
        public abstract void OnLoad();
        //Update
        public abstract void OnUpdate();
    }
}
