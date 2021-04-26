using System;
using System.Diagnostics;
using System.IO;

namespace SharpAsteroids
{
    class Music
    {
        System.Media.SoundPlayer player;
        bool missing = false;
        bool looper = false;

        /// <summary>
        /// Load sound
        /// </summary>
        /// <param name="file">Path to file</param>
        public Music(string file) {
            looper = false;
            string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\sounds\\" + file;

            //escape if missing
            if (!File.Exists(path)) { missing = true; Console.WriteLine("MISSING: " + path); return; }

            //preload wav
            player = new System.Media.SoundPlayer();
            player.SoundLocation = path;
            player.Load();
        }

        public Music(string file, bool loop) {
            looper = loop;
            string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\sounds\\" + file;

            //escape if missing
            if (!File.Exists(path)) { missing = true; Console.WriteLine("MISSING: " + path); return; }

            //preload wav
            player = new System.Media.SoundPlayer();
            player.SoundLocation = path;
            player.Load();
        }

        /// <summary>
        /// Play sound
        /// </summary>
        public void play()
        {
            if (missing || !render.music_opt) return;
            if (looper) player.PlayLooping();
            else player.Play();
        }

        public void stop()
        {
            if (missing || !render.music_opt) return;
            player.Stop();
        }
    }
}
