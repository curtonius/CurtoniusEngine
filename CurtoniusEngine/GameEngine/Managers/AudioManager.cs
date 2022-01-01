using System.Collections.Generic;

namespace GameEngine
{
    static class AudioManager
    {
        //List of all AudioPlayers
        static List<AudioPlayer> audioPlayers;

        //Add new AudioPlayer to the List
        public static void AddPlayer(AudioPlayer player)
        {
            if(audioPlayers == null)
            {
                audioPlayers = new List<AudioPlayer>();
            }

            audioPlayers.Add(player);
        }

        //Remove AudioPlayer from the List
        public static void RemovePlayer(AudioPlayer player)
        {
            if (audioPlayers == null)
            {
                audioPlayers = new List<AudioPlayer>();
            }

            audioPlayers.Remove(player);
        }

        //Update Audio Players to check for Looping
        public static void UpdateAudioPlayers()
        {
            if (audioPlayers != null)
            {
                foreach (AudioPlayer player in audioPlayers)
                {
                    player.CheckTime();
                }
            }
        }
    }
}
