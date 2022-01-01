using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEngine
{
    //Keep track of external files so the game engine doesn't have to locate them again later
    public static class DirectoryManager
    {
        //Images for Renderers
        public static Dictionary<string, Image> ImageDirectories = new Dictionary<string, Image>();
        //Uri for AudioPlayers
        public static Dictionary<string, Uri> AudioDirectories = new Dictionary<string, Uri>();
        //Fonts for....Fonts? Don't trust. Text is iffy
        public static Dictionary<string, Font> FontDirectories = new Dictionary<string, Font>();
    }
}
