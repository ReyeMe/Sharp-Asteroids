using System;
using System.Drawing;

namespace SharpAsteroids.FX
{
    class Particles {
        double velocity;
        double ang;
        int lifetime;
        PointF current;

        public bool IsDead;
        public bool Pause;

        public Particles( PointF start, double direction, double speed, int decay ) {
            ang = direction;
            velocity = speed/25;
            current = start;

            lifetime = decay;
            IsDead = false;
            Pause = false;
        }

        public PointF Update() {
            if (!Pause && lifetime > 0) lifetime--;
            if (lifetime == 0) IsDead = true;

            if (!Pause)
            {
                double angle = (ang * Math.PI) / 180;

                double Y = velocity * Math.Sin(angle);
                double X = velocity * Math.Cos(angle);

                current = new PointF(current.X + (float)X, current.Y + (float)Y);

            }

            return current;
        }
    }
}
