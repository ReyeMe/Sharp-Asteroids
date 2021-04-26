using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SharpAsteroids.Properties;
using System.Runtime.InteropServices;

namespace SharpAsteroids
{
    public partial class render : Form
    {
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  WinAPI
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //TODO
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  INITIALIZE, EXIT    (640x480)
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        Config Sys;

        GUI.Game Engine;
        List<GUI.Screen> SCREENS;
        int ActiveScreen = -1;

        public static bool effects_opt = true;
        bool fps_opt = false;

        public static bool sound_opt = true;
        public static bool music_opt = true;
        public static int sounds = 0;

        ScoreSave ScoreCeeper;

        Music intro;
        Music menu;

        public render()
        {
            InitializeComponent();

            ScoreCeeper = new ScoreSave("scores.bin");

            Sys = new Config("config.cfg",true);

            bool.TryParse(Sys.Get("effects"), out effects_opt);
            bool.TryParse(Sys.Get("sound"), out sound_opt);
            bool.TryParse(Sys.Get("music"), out music_opt);
            bool.TryParse(Sys.Get("showfps"), out fps_opt);

            SCREENS = new List<GUI.Screen>();

            intro = new Music("jingle.wav");
            menu = new Music("menu.wav",true);

            init();
        }

        private void render_Load(object sender, EventArgs e)
        {
            loop.Enabled = true;
            intro.play();
        }

        private void render_Exit(object sender, FormClosingEventArgs e)
        {
            Sys.Save();
            fps_counter.Enabled = false;
            loop.Enabled = false;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  SCREENS
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private void init() {

            //version
            GUI.Label version = new GUI.Label(new Point(560, 465), Color.Aqua, "Version 1.0a", "Consolas", 8);

            //Back to main menu
            GUI.Button hlp_back = new GUI.Button(25, 430, 200, 22, 14, "Back", "main", Color.OrangeRed, Color.Orange);
            hlp_back.Clicked += OnButton;

            //ID 0
            //Main Menu SCREEN
            GUI.Screen MainMenu = new GUI.Screen();

            int MainMenuPos = 280;

            //New Game Button
            GUI.Button newGame = new GUI.Button(25, MainMenuPos, 200, 22, 14, "NEW GAME", "newgame", Color.OrangeRed, Color.Orange);
            newGame.Clicked += OnButton;
            MainMenu.BUTTONS.Add(newGame);

            //Options
            GUI.Button options = new GUI.Button(25, 30 + MainMenuPos, 200, 22, 14, "OPTIONS", "options", Color.OrangeRed, Color.Orange);
            options.Clicked += OnButton;
            MainMenu.BUTTONS.Add(options);
            
            //Top Score
            GUI.Button score = new GUI.Button(25, 60 + MainMenuPos, 200, 22, 14, "TOP SCORES", "showscore", Color.OrangeRed, Color.Orange);
            score.Clicked += OnButton;
            MainMenu.BUTTONS.Add(score);

            //Help button
            GUI.Button help = new GUI.Button(25, 90 + MainMenuPos, 200, 22, 14, "HELP", "help", Color.OrangeRed, Color.Orange);
            help.Clicked += OnButton;
            MainMenu.BUTTONS.Add(help);

            //Credits button
            GUI.Button credits_btn = new GUI.Button(25, 120 + MainMenuPos, 200, 22, 14, "CREDITS", "credits", Color.OrangeRed, Color.Orange);
            credits_btn.Clicked += OnButton;
            MainMenu.BUTTONS.Add(credits_btn);

            //Exit button
            GUI.Button exit = new GUI.Button(25, 150 + MainMenuPos, 200, 22, 14, "EXIT", "exit", Color.OrangeRed, Color.Orange);
            exit.Clicked += OnButton;
            MainMenu.BUTTONS.Add(exit);

            //logo
            GUI.Picture logo = new GUI.Picture(new Point(0, 100), Resources.logo);
            MainMenu.PICTURES.Add(logo);

            MainMenu.LABELS.Add(version);

            SCREENS.Add(MainMenu);

            //ID 1
            //Help SCREEN
            GUI.Screen HelpMenu = new GUI.Screen();

            HelpMenu.BUTTONS.Add(hlp_back);

            GUI.Label helpMe = new GUI.Label(new Point(25, 25), Color.Orange, "Rules:\n\tShoot every asteroid to gain score. If you get hit\n\tby an Asteroid, you lose.\n\tBe careful, Asteroids will split into smaller ones\n\tafter they explode.\n\n\tHave fun :)\n\nControls:\n\tUp arrow - Forward\n\tLeft & Right arrow - turning\n\tSpacebar or CTRL - Shoot", "Consolas", 12);
            HelpMenu.LABELS.Add(helpMe);

            HelpMenu.LABELS.Add(version);

            SCREENS.Add(HelpMenu);

            //ID 2
            //ingame menu CLOSE
            GUI.Screen inGame = new GUI.Screen();

            GUI.Button ingame_open_menu = new GUI.Button(575, 15, 53, 18, 12, "MENU", "ingameOpen", Color.OrangeRed, Color.Orange);
            ingame_open_menu.Clicked += OnButton;
            inGame.BUTTONS.Add(ingame_open_menu);

            inGame.LABELS.Add(version);
            SCREENS.Add(inGame);

            //ID 3
            //Add score SCREEN
            GUI.Screen addScore = new GUI.Screen();

            GUI.Label nick_lbl = new GUI.Label(new Point(205, 190), Color.OrangeRed, "  Write your nick name.\n Press enter to continue.", "Consolas", 12);
            addScore.LABELS.Add(nick_lbl);

            GUI.Input nick = new GUI.Input(new Point(220,230), new Size(200,20), Color.Orange, Color.OrangeRed, "addscore", "Arial", 12, 15,true);
            nick.Fill += OnInput;
            addScore.INPUTS.Add(nick);

            SCREENS.Add(addScore);

            //ID 4
            //Show score
            GUI.Screen showScore = new GUI.Screen();

            GUI.Label score_title = new GUI.Label(new Point(225, 15), Color.Yellow, "TOP SCORES", "Consolas", 24);
            showScore.LABELS.Add(score_title);

            showScore.LABELS.Add(version);

            showScore.BUTTONS.Add(hlp_back);

            SCREENS.Add(showScore);

            //ID 5
            //Options SCREEN
            GUI.Screen OptionsMenu = new GUI.Screen();

            GUI.Label setup_title = new GUI.Label(new Point(250, 15), Color.Yellow, "OPTIONS", "Consolas", 24);
            OptionsMenu.LABELS.Add(setup_title);

            GUI.CheckBox effects_setup = new GUI.CheckBox(25, 60, 25, "effects", effects_opt, Color.Orange, Color.OrangeRed);
            effects_setup.Clicked += onChange;
            OptionsMenu.CHECKS.Add(effects_setup);

            GUI.Label effects_lbl = new GUI.Label(new Point(60, 63), Color.Yellow, "Enable effects", "Consolas", 12);
            OptionsMenu.LABELS.Add(effects_lbl);

            GUI.CheckBox sound_setup = new GUI.CheckBox(25, 95, 25, "sound", sound_opt, Color.Orange, Color.OrangeRed);
            sound_setup.Clicked += onChange;
            OptionsMenu.CHECKS.Add(sound_setup);

            GUI.Label sound_lbl = new GUI.Label(new Point(60, 98), Color.Yellow, "Enable sound", "Consolas", 12);
            OptionsMenu.LABELS.Add(sound_lbl);

            GUI.CheckBox music_setup = new GUI.CheckBox(25, 130, 25, "music", music_opt, Color.Orange, Color.OrangeRed);
            music_setup.Clicked += onChange;
            OptionsMenu.CHECKS.Add(music_setup);

            GUI.Label music_lbl = new GUI.Label(new Point(60, 133), Color.Yellow, "Enable music", "Consolas", 12);
            OptionsMenu.LABELS.Add(music_lbl);

            GUI.CheckBox fps_setup = new GUI.CheckBox(25, 165, 25, "fps", fps_opt, Color.Orange, Color.OrangeRed);
            fps_setup.Clicked += onChange;
            OptionsMenu.CHECKS.Add(fps_setup);

            GUI.Label fps_lbl = new GUI.Label(new Point(60, 168), Color.Yellow, "Show FPS", "Consolas", 12);
            OptionsMenu.LABELS.Add(fps_lbl);

            OptionsMenu.BUTTONS.Add(hlp_back);

            OptionsMenu.LABELS.Add(version);

            SCREENS.Add(OptionsMenu);

            //ID 6
            //ingame menu OPEN
            GUI.Screen inGameMenu = new GUI.Screen();

            GUI.Label ingame_title = new GUI.Label(new Point(262, 105), Color.Yellow, "Paused", "Consolas", 24);
            inGameMenu.LABELS.Add(ingame_title);

            GUI.Button ingame_close_menu = new GUI.Button(220, 150, 200, 23, 14, "RETURN", "ingame", Color.OrangeRed, Color.Orange);
            ingame_close_menu.Clicked += OnButton;
            inGameMenu.BUTTONS.Add(ingame_close_menu);

            GUI.Button ingame_exit_menu = new GUI.Button(220, 183, 200, 23, 14, "EXIT TO MAIN MENU", "main", Color.OrangeRed, Color.Orange);
            ingame_exit_menu.Clicked += OnButton;
            inGameMenu.BUTTONS.Add(ingame_exit_menu);

            GUI.Button ingame_exit = new GUI.Button(220, 216, 200, 23, 14, "EXIT TO WINDOWS", "exit", Color.OrangeRed, Color.Orange);
            ingame_exit.Clicked += OnButton;
            inGameMenu.BUTTONS.Add(ingame_exit);

            inGameMenu.LABELS.Add(version);

            SCREENS.Add(inGameMenu);

            //ID 7
            GUI.Screen credits = new GUI.Screen();

            GUI.Label credits_title = new GUI.Label(new Point(250, 15), Color.Yellow, "CREDITS", "Consolas", 24);
            credits.LABELS.Add(credits_title);

            GUI.Label credits_lbl = new GUI.Label(new Point(25, 60), Color.Orange, "Programming/Graphics:\n\tReye (http://www.reye.me)\n\nSound:\n\tCerx (https://cerx.pw)", "Consolas", 12);
            credits.LABELS.Add(credits_lbl);

            GUI.Label copyright_lbl = new GUI.Label(new Point(250, 435), Color.OrangeRed, "© 2016 PIXADRON Released under MIT licence", "Consolas", 9);
            credits.LABELS.Add(copyright_lbl);

            credits.BUTTONS.Add(hlp_back);

            credits.LABELS.Add(version);

            SCREENS.Add(credits);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  IO
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private void engine_mouse_move(object sender, MouseEventArgs e)
        {
            if(ActiveScreen > -1)SCREENS[ActiveScreen].MouseMove(e);
        }

        private void engine_mouse_click(object sender, MouseEventArgs e)
        {
            if (ActiveScreen > -1) SCREENS[ActiveScreen].MouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                catch { }
            }
        }

        bool playing = false;
        private void OnButton(object sender, string name) {

            //GOTO
            switch (name) {
                case "credits":
                    ActiveScreen = 7;
                    break;
                case "ingameOpen":
                    ActiveScreen = 6;
                    break;
                case "options":
                    ActiveScreen = 5;
                    break;
                case "showscore":
                    ActiveScreen = 4;
                    break;
                case "addscore":
                    if (ScoreCeeper.score == 0) { ActiveScreen = 4; break; }
                    ActiveScreen = 3;
                    break;
                case "ingame":
                    ActiveScreen = 2;
                    break;
                case "newgame":
                    if (playing) menu.stop();
                    playing = false;
                    ScoreCeeper.score = 0;
                    Engine = new GUI.Game();
                    Engine.Change += OnButton;
                    ActiveScreen = 2;
                    break;
                case "help":
                    ActiveScreen = 1;
                    break;
                case "main":
                    if (!playing) menu.play();
                    playing = true;
                    ActiveScreen = 0;
                    break;
                case "exit":
                    Close();
                    break;
                default:
                    break;
            }

        }

        private void OnInput(object sender, string output, string ID) {
            switch (ID) {
                case "addscore":
                    ActiveScreen = 4;
                    ScoreCeeper.Write(output);
                    ((GUI.Input)sender).Clear();
                    break;
                default:
                    break;
            }            
        }

        private void engine_key_press(object sender, KeyPressEventArgs e)
        {
            if (ActiveScreen > -1) SCREENS[ActiveScreen].KeyPress(e);
            else if (e.KeyChar == (char)Keys.Enter) { keyframe = 1000; intro.stop(); }

            if (e.KeyChar == (char)Keys.Escape && ActiveScreen == 2) ActiveScreen = 6;
        }

        private void onChange(object sender, bool state, string ID) {
            switch (ID) {
                case "music":
                    if (music_opt) { menu.stop(); playing = false; music_opt = !music_opt; Sys.Set("music", music_opt.ToString()); break; }
                    if (!music_opt) { music_opt = !music_opt; menu.play(); playing = true; Sys.Set("music", music_opt.ToString()); break; }
                    Sys.Set("music", music_opt.ToString());
                    break;
                case "fps":
                    fps_opt = !fps_opt;
                    Sys.Set("showfps", fps_opt.ToString());
                    break;
                case "sound":
                    sound_opt = !sound_opt;
                    Sys.Set("sound",sound_opt.ToString());
                    break;
                case "effects":
                    effects_opt = state;
                    Sys.Set("effects",effects_opt.ToString());
                    break;
                default:
                    break;
            }
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  RENDER
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        int frames = 0, fps = 0, keyframe = 0;
        private void engine_paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            //ingame
            if (ActiveScreen == 2 || ActiveScreen == 6)
            {
                Engine.Draw(ref e);
                if (ActiveScreen == 2)
                {
                    Engine.LogicUpdate(ref ScoreCeeper.score);
                    GUI.Label score = new GUI.Label(new Point(7, 8), Color.Orange, ScoreCeeper.score.ToString("00000000"), "Consolas", 20);
                    score.Draw(ref e);
                }
                else e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0, 0)), new Rectangle(new Point(0, 0), new Size(640, 480)));

            }

            if (ActiveScreen == 0 && !playing) { menu.play(); playing = true; }

            //intro
            if (ActiveScreen == -1) {

                GUI.Picture logo = new GUI.Picture(new Point(132, 210), Resources.pixadron);
                logo.Draw(ref e);

                GUI.Label presents = new GUI.Label(new Point(132, 275), Color.Orange, "presents", "Impact", 14);
                presents.Draw(ref e);

                int alpha = 255 + ((keyframe>500)?keyframe-755:-keyframe);
                if (alpha < 0) alpha = 0;
                if (alpha > 255) alpha = 255;
                Brush fade = new SolidBrush(Color.FromArgb(alpha,0,0,0));
                e.Graphics.FillRectangle(fade,new Rectangle(new Point(132, 210),new Size(376,100)));

                int pos = keyframe-900;
                if (pos > 100) pos = 100;
                GUI.Picture game = new GUI.Picture(new Point(0, pos), Resources.logo);
                game.Draw(ref e);

                if (keyframe >= 1000) ActiveScreen = 0;
                keyframe+=3;
                fps++;

                if (fps_opt)
                {
                    GUI.Label fps_lbl = new GUI.Label(new Point(5, 5), Color.Azure, "FPS: " + frames.ToString(), "Consolas", 8);
                    fps_lbl.Draw(ref e);
                }

                Pen round = new Pen(Color.GreenYellow, 1);
                e.Graphics.DrawRectangle(round, 0, 0, Width - 1, Height - 1);
                return;
            }

            SCREENS[ActiveScreen].Draw(ref e, effects_opt);

            if (ActiveScreen == 4) {
                string file = "";
                file = ScoreCeeper.Read();

                int i = 1;
                if (file != "") {

                    string[] score_txt = file.Remove(file.Length - 1, 1).Split(';');
                    foreach (string str in score_txt)
                    {
                        string[] data = str.Split('-');

                        int num = 0;
                        int.TryParse(data[1], out num);

                        if (num != 0)
                        {
                            GUI.Label nick = new GUI.Label(new Point(35, 40 + (20 * i)), Color.Orange, data[0], "Consolas", 14);
                            GUI.Label score = new GUI.Label(new Point(515, 40 + (20 * i)), Color.Orange, num.ToString("00000000"), "Consolas", 14);
                            score.Draw(ref e);
                            nick.Draw(ref e);
                            i++;
                        }
                    }
                }

                int j;
                for (j = i; j < 11; j++) {
                    GUI.Label nick = new GUI.Label(new Point(35, 40+(20*j)), Color.Orange, "PIXADRON", "Consolas", 14);
                    GUI.Label score = new GUI.Label(new Point(515, 40 + (20 * j)), Color.Orange, "00000000", "Consolas", 14);
                    score.Draw(ref e);
                    nick.Draw(ref e);
                }
            }

            if (keyframe < 1255)
            {
                Brush fade = new SolidBrush(Color.FromArgb(255-(keyframe - 1000), 0, 0, 0));
                e.Graphics.FillRectangle(fade, new Rectangle(new Point(0, 0), new Size(Width, Height)));
                GUI.Picture game = new GUI.Picture(new Point(0, 100), Resources.logo);
                game.Draw(ref e);
                keyframe += 5;
            }

            if (fps_opt) {
                GUI.Label fps_lbl = new GUI.Label(new Point(5,5), Color.Azure, "FPS: "+ frames.ToString(), "Consolas", 8);
                fps_lbl.Draw(ref e);
            }

            Pen Around = new Pen(Color.GreenYellow, 1);
            e.Graphics.DrawRectangle(Around, 0, 0, Width - 1, Height - 1);

            fps++;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //  OTHER
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void setVolume(int volume)
        {
            uint vol = 0;
            if (volume > 100) vol = 4294967295;
            else if (volume < 0) vol = 0;
            else vol = (uint)volume * 42949672;

            uint NewVolumeAllChannels = (((uint)vol & 0x0000ffff) | ((uint)vol << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }
        
        private void fps_counter_Tick(object sender, EventArgs e)
        {
            frames = fps;
            fps = 0;
        }

        private void loop_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

    }
}
