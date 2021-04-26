using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Linq;

namespace SharpAsteroids.GUI
{
    class Game
    {
        public delegate void OnChange(object sender, string GOTO);
        public event OnChange Change;

        Entities.Player player;
        List<Entities.Asteroid> Asteroids;
        List<Entities.Bullet> Bullets;
        List<FX.Explosion> boom;

        Sound shoot;
        Sound explode1;
        Sound explode2;
        Sound explode3;
        Sound explode4;
        Sound die;

        Random rnd;

        public Game()
        {
            player = new Entities.Player(new Point(320, 240), 1.2);

            Asteroids = new List<Entities.Asteroid>();
            Bullets = new List<Entities.Bullet>();
            boom = new List<FX.Explosion>();

            shoot = new Sound("pew.wav");

            explode1 = new Sound("boom1.wav");
            explode2 = new Sound("boom2.wav");
            explode3 = new Sound("boom3.wav");
            explode4 = new Sound("boom4.wav");

            die = new Sound("die.wav");

            rnd = new Random();
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        int timer = 0;
        private void KeyHold()
        {
            foreach (Key input in GetDownKeys())
            {
                if (input == Key.Up) player.Forward();
                else if (input == Key.Down) player.Backward();

                if (input == Key.Left) player.Left();
                else if (input == Key.Right) player.Right();

                if ((input == Key.Space || input == Key.LeftCtrl || input == Key.RightCtrl) && timer == 0)
                {
                    timer = 5;
                    shoot.play();

                    Entities.Bullet create = new Entities.Bullet(player.GetAngle(), player.GetPosition(), 10);
                    Bullets.Add(create);
                }
            }

            timer--;
            if (timer < 0) timer = 0;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        int spawntimer = 0;
        public void LogicUpdate(ref int score)
        {
            foreach (FX.Explosion exp in boom) exp.Pause = false;

            player.Active();

            //check keyboard
            KeyHold();

            int i = 0;

            //clean up
            for (i = 0; i < Bullets.Count; i++)
            {
                PointF position = Bullets[i].GetPosition();
                if ((position.X > 700 || position.X < -80) && (position.Y > 560 || position.Y < -80))
                {
                    Bullets.RemoveAt(i);
                    i--;
                }

            }

            for (i = 0; i < Asteroids.Count; i++)
            {
                PointF position = Asteroids[i].GetPosition();
                if ((position.X > 700 || position.X < -80) && (position.Y > 560 || position.Y < -80))
                {
                    if (position.X > 700) position.X = -80;
                    else if (position.X < -80) position.X = 700;

                    if (position.Y > 560) position.Y = -80;
                    else if (position.Y < -80) position.Y = 560;

                    Asteroids[i].position = position;
                }
                else if (Asteroids[i].GetSize() == 0)
                {
                    Asteroids.RemoveAt(i);
                    i--;
                }

            }

            //Update entities
            foreach (Entities.Bullet bullet in Bullets) bullet.Update();
            foreach (Entities.Asteroid asteroid in Asteroids) asteroid.Update();

            //check for collisions
            List<Entities.Asteroid> toAdd = new List<Entities.Asteroid>();
            toAdd.Clear();
            foreach (Entities.Bullet bullet in Bullets)
            {
                foreach (Entities.Asteroid asteroid in Asteroids)
                {
                    if (asteroid.Collide(bullet.GetPosition()))
                    {
                        bullet.remove();

                        if (render.effects_opt) {
                            FX.Explosion exp = new FX.Explosion(asteroid.GetPosition(), rnd.Next(50, 180), asteroid.GetSize() * 10);
                            boom.Add(exp);
                        }
                        
                        int toSpawn = 0;
                        switch (asteroid.GetSize())
                        {
                            case 1:
                                explode1.play();
                                score += 50;
                                break;
                            case 2:
                                explode2.play();
                                toSpawn = 4;
                                score += 25;
                                break;
                            case 3:
                                explode3.play();
                                toSpawn = 3;
                                score += 12;
                                break;
                            default:
                                explode4.play();
                                toSpawn = 2;
                                score += 6;
                                break;
                        }

                        if (asteroid.GetSize() == 1) { asteroid.remove(); continue; }

                        PointF spawn = asteroid.GetPosition();

                        double speed;
                        switch (asteroid.GetSize())
                        {
                            case 1:
                                speed = 1.6;
                                break;
                            case 2:
                                speed = 1.1;
                                break;
                            case 3:
                                speed = 0.8;
                                break;
                            default:
                                speed = 0.4;
                                break;
                        }

                        int j;
                        for (j = 0; j < toSpawn; j++)
                        {
                            Entities.Asteroid create = new Entities.Asteroid(rnd.Next(0, 360), spawn, speed, asteroid.GetSize() - 1);
                            toAdd.Add(create);
                        }
                        
                        asteroid.remove();
                    }
                }
            }

            foreach (Entities.Asteroid asteroid in toAdd) Asteroids.Add(asteroid);

            //spawn
            if (spawntimer == 0)
            {
                PointF start;
                switch (rnd.Next(1, 5))
                {
                    case 1:
                        start = new PointF(rnd.Next(-40, 680), -40);
                        break;
                    case 2:
                        start = new PointF(-40, rnd.Next(-40, 520));
                        break;
                    case 3:
                        start = new PointF(rnd.Next(-40, 680), 520);
                        break;
                    default:
                        start = new PointF(680, rnd.Next(-40, 520));
                        break;
                }

                double speed;
                int size = rnd.Next(1, 5);
                switch (size)
                {
                    case 1:
                        speed = 1.6;
                        break;
                    case 2:
                        speed = 1.1;
                        break;
                    case 3:
                        speed = 0.7;
                        break;
                    default:
                        speed = 0.3;
                        break;
                }

                double angle = GetAngle(player.GetPosition(), start);
                Entities.Asteroid create = new Entities.Asteroid(angle, start, speed, size);
                Asteroids.Add(create);

                spawntimer = 160;
            }
            spawntimer--;
            if (spawntimer < 0) spawntimer = 0;

            foreach (Entities.Asteroid asteriod in Asteroids)
            {
                if (player.ColideAsteroid(asteriod.GetPosition(), asteriod.GetSize()))
                {
                    die.play();
                    Change.Invoke(this, "addscore");
                    break;
                }
            }
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e)
        {
            foreach (FX.Explosion exp in boom) exp.Draw(ref e);
            foreach (Entities.Bullet bullet in Bullets) bullet.Draw(ref e);
            foreach (Entities.Asteroid asteroid in Asteroids) asteroid.Draw(ref e);
            player.Draw(ref e);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private double GetAngle(PointF a, PointF b)
        {
            double angle = Math.Atan2(a.Y - b.Y, a.X - b.X);

            return (angle * 180 / Math.PI);
        }

        private static readonly byte[] DistinctVirtualKeys = Enumerable
            .Range(0, 256)
            .Select(KeyInterop.KeyFromVirtualKey)
            .Where(item => item != Key.None)
            .Distinct()
            .Select(item => (byte)KeyInterop.VirtualKeyFromKey(item))
            .ToArray();

        /// <summary>
        /// Gets all keys that are currently in the down state.
        /// </summary>
        /// <returns>
        /// A collection of all keys that are currently in the down state.
        /// </returns>
        public IEnumerable<Key> GetDownKeys()
        {
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            var downKeys = new List<Key>();
            for (var index = 0; index < DistinctVirtualKeys.Length; index++)
            {
                var virtualKey = DistinctVirtualKeys[index];
                if ((keyboardState[virtualKey] & 0x80) != 0)
                {
                    downKeys.Add(KeyInterop.KeyFromVirtualKey(virtualKey));
                }
            }

            return downKeys;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetKeyboardState(byte[] keyState);
    }
}
