using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpAsteroids
{
    public class Sound
    {
        bool missing = false;

        string path = "";

        /// <summary>
        /// Load sound
        /// </summary>
        /// <param name="file"></param>
        /// <param name="volume"></param>
        public Sound(string file) {
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\sounds\\" + file;
            if (!File.Exists(path)) { missing = true; Console.WriteLine("Missing: " + path); return; }
        }

        public void play() {
            if (missing || !render.sound_opt || render.sounds > 6) return;

            
            new System.Threading.Thread(() =>
            {
                try {
#if DEBUG

#else
                    Linsft.FmodSharp.Debug.Level = Linsft.FmodSharp.DebugLevel.Error;
#endif

                    var SoundSystem = new Linsft.FmodSharp.SoundSystem.SoundSystem();
                    SoundSystem.Init(8, Linsft.FmodSharp.SoundSystem.InitFlags.Normal);

                    //SoundSystem.ReverbProperties = Linsft.FmodSharp.Reverb.Presets.Off;

                    Linsft.FmodSharp.Sound.Sound SoundFile;
                    SoundFile = SoundSystem.CreateSound(path, Linsft.FmodSharp.Mode.Hardware);

                    Linsft.FmodSharp.Channel.Channel Chan;
                    Chan = SoundSystem.PlaySound(SoundFile);
                    render.sounds++;
                    while (Chan.IsPlaying)
                    { System.Threading.Thread.Sleep(10); }

                    SoundFile.Dispose();
                    Chan.Dispose();

                    SoundSystem.CloseSystem();
                    SoundSystem.Dispose();
                    render.sounds--;
                } catch { }

            }).Start();
                
        }

        public void stop() { }
    }
}
