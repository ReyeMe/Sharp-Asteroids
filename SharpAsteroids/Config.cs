/*
 *  Reye (David Jurík)
 *	http://reye.me/
 *  PIXADRON ©2016
 *  
 *  .NET4.0
 *
 */

using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

//req
using System.Windows.Forms;

namespace SharpAsteroids
{
    [Serializable()]
    public class Config
    {
        /// <summary>
        ///  Path and name of file
        /// </summary>
        public string path;

        /// <summary>
        ///  Show error messages
        /// </summary>
        public Message Messages = Message.NONE;

        /// <summary>
        ///  Error output type
        /// </summary>
        public enum Message
        {
            NONE = 1,
            CONSOLE,
            BOX
        };

        /// <summary>
        ///  Direct access to keyChain
        /// </summary>
        public List<string[]> keychain = new List<string[]>();

        //whole file
        string[] lines;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        /// <summary>
        ///  Creates keychain
        /// </summary>
        /// <param name="filePath">Path and name of file</param>
        public Config(string filePath)
        {
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + filePath;
            keychain.Clear();
        }

        /// <summary>
        ///  Creates keychain
        /// </summary>
        /// <param name="filePath">Path and name of file</param>
        /// <param name="type">Type of error output</param>
        public Config(string filePath, Message type)
        {
            Messages = type;
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + filePath;
            keychain.Clear();
        }

        /// <summary>
        ///  Creates keychain
        /// </summary>
        /// <param name="filePath">Path and name of file</param>
        /// <param name="AutoLoad">Load file automaticly</param>
        public Config(string filePath, bool AutoLoad)
        {
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + filePath;
            keychain.Clear();

            if (AutoLoad) Load();
        }

        /// <summary>
        ///  Creates keychain
        /// </summary>
        /// <param name="filePath">Path and name of file</param>
        /// <param name="type">Type of error output</param>
        /// <param name="AutoLoad">Load file automaticly</param>
        public Config(string filePath, Message type, bool AutoLoad)
        {
            Messages = type;
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + filePath;
            keychain.Clear();

            if (AutoLoad) Load();
        }

        /// <summary>
        ///  DeepClone object
        /// </summary>
        /// <returns>Copy of this object</returns>
        public Config Clone()
        {
            return (Config)DeepClone(this);
        }

        /// <summary>
        ///  Dispose object
        /// </summary>
        public virtual void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        /// <summary>
        ///  Returns Version as a string
        /// </summary>
        /// <returns>String</returns>
        public string Version()
        {
            return "0.3";
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        /// <summary>
        ///  Returns total count of keys in keychain
        /// </summary>
        /// <returns>Int</returns>
        public int Count()
        {
            return keychain.Count;
        }

        /// <summary>
        ///  Returns list of avalibe keys
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> Keys()
        {
            List<string> keys = new List<string>();

            keys.Clear();

            foreach (string[] line in keychain)
            {
                keys.Add(line[0]);
            }

            return keys;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        /// <summary>
        ///  Load or Reload keychain
        /// </summary>
        /// <returns>False on error</returns>
        public bool Load()
        {
            keychain.Clear();

            if (!File.Exists(path)) {
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.Write(";DO NOT EDIT\neffects = True\nsound = True\nmusic = True\nshowfps = False");
                    }
                }
                catch (Exception ex)
                {
                    if (Messages == Message.CONSOLE)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    else if (Messages == Message.BOX)
                    {
                        MessageBox.Show("ex.Message");
                    }

                    return false;
                }
            };

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

                }
            }
            catch (Exception ex)
            {
                if (Messages == Message.CONSOLE)
                {
                    Console.WriteLine(ex.Message);
                }
                else if (Messages == Message.BOX)
                {
                    MessageBox.Show("ex.Message");
                }

                return false;
            }

            foreach (string line in lines)
            {
                string[] Key = decode(line);
                if (Key[0] == ";") { continue; }

                keychain.Add(Key);
            }

            return true;
        }

        /// <summary>
        ///  Save changes to file
        /// </summary>
        /// <returns>False on error</returns>
        public bool Save()
        {
            if (!File.Exists(path)) return false;

            //construct file
            int activeLine = 0;
            foreach (string[] key in keychain)
            {
                bool escape = false;
                while (lines[activeLine] == null || lines[activeLine] == "" || lines[activeLine][0] == ';')
                {
                    activeLine++;

                    if (!(activeLine < lines.Length))
                    {
                        escape = true;
                        break;
                    }
                }

                if (escape) { break; }

                var regex = new Regex(Regex.Escape(decode(lines[activeLine])[1]));

                string[] ln = lines[activeLine].Split('=');

                ln[1] = regex.Replace(ln[1], key[1], 1);

                lines[activeLine] = ln[0] + "=" + ln[1];

                ln = null;
                regex = null;

                activeLine++;
            }

            string file = "";

            foreach (string line in lines)
            {
                file += line + Environment.NewLine;
            }

            file = file.Remove(file.Length - 2, 2);

            //save file
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(file);
                }
            }
            catch (Exception ex)
            {
                if (Messages == Message.CONSOLE)
                {
                    Console.WriteLine(ex.Message);
                }
                else if (Messages == Message.BOX)
                {
                    MessageBox.Show("ex.Message");
                }

                return false;
            }

            return true;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        /// <summary>
        ///  Get a value of key from keychain
        /// </summary>
        /// <param name="Key">Name of Key</param>
        /// <returns>Value of key</returns>
        public string Get(string Key)
        {
            string value = "";

            for (int i = 0; i < keychain.Count; i++)
            {
                if (keychain[i][0] == Key)
                {
                    value = keychain[i][1];
                    break;
                }
            }

            return value;
        }

        /// <summary>
        ///  Set a key value in keychain
        /// </summary>
        /// <param name="Key">Name of Key</param>
        /// <param name="Value">Value of key</param>
        /// <returns>False if not found</returns>
        public bool Set(string Key, string Value)
        {
            bool pass = false;

            for (int i = 0; i < keychain.Count; i++)
            {
                if (keychain[i][0] == Key)
                {
                    keychain[i][1] = Value;
                    pass = true;
                    break;
                }
            }

            return pass;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        //is disposed?
        bool disposed = false;

        //decode lines
        private string[] decode(string line)
        {
            string[] Key = new string[2] { ";", "" };

            if (line != null && line != "" && line[0] != ';')
            {
                if (line.Contains(";")) line = line.Remove(line.IndexOf(';'), line.Length - line.IndexOf(';'));

                string[] ln = line.Split('=');

                //fix key
                ln[0] = Regex.Replace(ln[0], "[^a-zA-Z0-9 _]", string.Empty, RegexOptions.IgnoreCase).Trim();
                if (ln[0] == "") { return Key; }

                //fix Value
                ln[1] = ln[1].Trim();

                if (ln[1] == "") { return Key; }

                //return decoded
                Key = ln;
            }

            return Key;
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // Instantiate a SafeHandle instance.
            SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.

                keychain.Clear();
                lines = null;
                path = null;
                Messages = Message.NONE;
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        //object clone
        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }
    }
}