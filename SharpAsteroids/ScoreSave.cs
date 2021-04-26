using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SharpAsteroids
{
    class ScoreSave
    {
        public int score;

        string path;

        public ScoreSave(string file) {
            path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\" + file;
            score = 0;
        }

        public void Write(string nick) {
            string file = "";
            if (File.Exists(path)) {
                file = Read();
                if(file == null) file = "";
            }

            string toWrite = "";

            int pos = 0;
            string key = "51s#íáfg.běs6f/gb1..'vž";
            
            nick += '-';

            if (file == "")
            {
                file = nick + score.ToString() + ';';
            }
            else {
                string[] search = file.Remove(file.Length - 1, 1).Split(';');

                int i = 0;
                bool found = false;
                foreach (string scr in search) {
                    int num = 0;
                    int.TryParse(scr.Split('-')[1], out num);

                    if (num <= score) {
                        found = true;
                        break;
                    }
                    i++;
                }

                if (!found && search.Length > 10) { score = 0; return; }

                List<string> paste = new List<string>();

                int j;
                for (j = 0; j < i; j++) {
                    paste.Add(search[j]);
                }

                paste.Add(nick + score.ToString());

                for (j = i; j < search.Length; j++) {
                    paste.Add(search[j]);
                }

                if (paste.Count > 10) paste.RemoveAt(paste.Count-1);

                file = "";
                foreach (string scr in paste) {
                    file += scr+';';
                }
            }

            foreach (char sym in file)
            {
                char end = (char)(key[pos] ^ sym);
                toWrite += end;
                pos++;
                if (pos > key.Length - 1) pos = 0;
            }

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(toWrite);
                }
            }
            catch
            { }

            score = 0;
        }

        public string Read() {
            int pos = 0;
            string key = "51s#íáfg.běs6f/gb1..'vž";
            string file = "";

            try {
                if (File.Exists(path))
                {
                    string raw = File.ReadAllText(path);
                    int len = raw.Length;
                    foreach (char sym in raw)
                    {
                        char end = (char)(key[pos] ^ sym);
                        file += end;
                        pos++;
                        if (pos > key.Length - 1) pos = 0;
                    }
                }
            } catch { return null; }

            return file;
        }
    }
}
