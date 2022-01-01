using System;
using System.Windows.Media;
using System.IO;
using System.Reflection;

namespace GameEngine
{
    //Can maybe be improved
    //Type of Component for playing audio
    public class AudioPlayer : Component
    {
        //Variables for the Volume and Speed of the Audio File
        private double volume = 1;
        private double speed = 1;
        public double Volume { get { return volume; } set{ volume = value; ChangeVolume(); } }
        public double Speed { get { return speed; } set { speed = value; ChangeSpeed(); } }

        //Is this AudioPlayer paused
        public bool paused=false;
        //Does this AudioPlayer loop;
        public bool loop = false;

        MediaPlayer player = null;
        TimeSpan originalPosition;
        private bool IsPlaying = false;

        public AudioPlayer()
        {
            ClassName = "AudioPlayer";
            player = new MediaPlayer();
        }

        public AudioPlayer(string directory)
        {
            ClassName = "AudioPlayer";
            SetClip(directory);
        }

        //Find the clip in the directory
        public void SetClip(string directory)
        {
            //If the clip already exists, use it
            if (DirectoryManager.AudioDirectories.ContainsKey(directory))
            {
                player.Open(DirectoryManager.AudioDirectories[directory]);
            }
            //If the clip doesn't exist, create one
            else
            {
                player = new MediaPlayer();
                string executableDirectoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string audioFilePath = Path.Combine(executableDirectoryPath, "Assets/Audio/" + directory);
                player.Open(new Uri(audioFilePath));

                originalPosition = player.Position;

                DirectoryManager.AudioDirectories.Add(directory, player.Source);
            }
        }

        //Change volume of the clip
        void ChangeVolume()
        {
            player.Volume = volume;
        }

        //Change speed of the clip
        void ChangeSpeed()
        {
            player.SpeedRatio = speed;
        }

        //Play the clip
        public void Play()
        {
            IsPlaying = true;
            //If the clip was paused do not start over, continue from where it left off
            //Otherwise start the clip over
            if (!paused)
            {
                player.Position = originalPosition;
            }
            paused = false;
            player.Volume = volume;
            player.SpeedRatio = speed;

            player.Play();
        }

        //Stop playing the clip
        public void Stop()
        {
            IsPlaying = false;
            player.Position = originalPosition;
            paused = false;
            player.Stop();
        }

        //Pause the clip
        public void Pause()
        {
            IsPlaying = false;
            paused = true;
            player.Pause();
        }

        //Check if the clip is finished
        public void CheckTime()
        {
            if (player.NaturalDuration.HasTimeSpan)
            {
                //If the clip is finished
                if (IsPlaying && player.Position == player.NaturalDuration.TimeSpan)
                {
                    //If the clip should loop, play it again
                    //Otherwise stop it
                    if (loop)
                    {
                        Play();
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

        //Add component to the AudioManager
        public override void ComponentAdded()
        {
            AudioManager.AddPlayer(this);
        }

        //Remove component from the AudioManager
        public override void ComponentRemoved()
        {
            AudioManager.RemovePlayer(this);
        }
    }
}
