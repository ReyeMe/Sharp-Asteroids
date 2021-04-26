using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.Entities
{
    class Player
    {
        double dir;
        double lastdir;
        double velocity;
        double thuster;
        double max;
        PointF position;

        bool update = false;

        public Player(PointF start, double speed)
        {
            position = start;
            max = speed;

            velocity = 0;
            thuster = 0;
            dir = 0;
            lastdir = 0;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Left()
        {
            dir -= 4;
        }

        public void Right()
        {
            dir += 4;
        }

        public void Forward()
        {
            if (velocity < max) velocity += 0.05;
            if (thuster < max) thuster += 0.04;
            lastdir = dir;
        }

        public void Backward()
        {
            //velocity = -max;
            //position = CalcPoint(position, dir, (velocity/1.5));
        }
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public bool ColideAsteroid(PointF thing, int size)
        {
            double dist = Math.Sqrt(Math.Pow((position.X - thing.X), 2) + Math.Pow((position.Y - thing.Y), 2));

            return dist < 5 + (size * 10);

        }

        public void Active()
        {
            update = true;
        }

        public PointF GetPosition()
        {
            return position;
        }

        public double GetAngle()
        {
            return dir;
        }

        double frame = 0;
        public void Draw(ref PaintEventArgs e)
        {
            if (velocity > 0 && update) position = CalcPoint(position, lastdir, velocity);

            if (position.X > 652) position.X = -12;
            else if (position.X < -12) position.X = 652;

            if (position.Y > 492) position.Y = -12;
            else if (position.Y < -12) position.Y = 492;

            Pen ship = new Pen(Color.White, 1);
            Pen engine = new Pen(Color.Yellow, 1);

            PointF p1 = RotPoint(new PointF(position.X, 10 + position.Y), position, 90 + dir);
            PointF p2 = RotPoint(new PointF(-10 + position.X, -10 + position.Y), position, 90 + dir);
            PointF p3 = RotPoint(new PointF(position.X, -5 + position.Y), position, 90 + dir);
            PointF p4 = RotPoint(new PointF(10 + position.X, -10 + position.Y), position, 90 + dir);

            float thuster_grp = (float)((((-10 * thuster) - 5) + position.Y + (Math.Sin(frame))));
            PointF p5 = RotPoint(new PointF(-5f + position.X, -7f + position.Y), position, 90 + dir);
            PointF p6 = RotPoint(new PointF(position.X,( (thuster_grp > 0) ? thuster_grp : (position.Y-5) )), position, 90 + dir);
            PointF p7 = RotPoint(new PointF(5 + position.X, -7 + position.Y), position, 90 + dir);

            if (thuster > 0)
            {
                e.Graphics.DrawLine(engine, p5, p6);
                e.Graphics.DrawLine(engine, p6, p7);
            }

            e.Graphics.DrawLine(ship, p1, p2);
            e.Graphics.DrawLine(ship, p2, p3);
            e.Graphics.DrawLine(ship, p3, p4);
            e.Graphics.DrawLine(ship, p4, p1);

            if (update) {
                if (velocity >= 0) velocity -= 0.003;
                if (thuster >= 0) thuster -= 0.016;

                if (frame > 3.14) frame = 0;
                frame += 0.1;
            }
            update = false;

            ship.Dispose();
            engine.Dispose();
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private PointF CalcPoint(PointF a, double angle, double speed)
        {
            angle = (angle * Math.PI) / 180;

            double Y = speed * Math.Sin(angle);
            double X = speed * Math.Cos(angle);

            PointF c = new PointF(a.X + (float)X, a.Y + (float)Y);

            return c;
        }

        private PointF RotPoint(PointF center, PointF a, double angle)
        {
            angle = (angle * Math.PI) / 180;

            double s = Math.Sin(angle);
            double c = Math.Cos(angle);

            a.X -= center.X;
            a.Y -= center.Y;

            PointF newp = new PointF(
                (float)(a.X * c - a.Y * s),
                (float)(a.X * s + a.Y * c));

            a.X += newp.X + center.X;
            a.Y += newp.Y + center.Y;

            return a;
        }
    }
}
