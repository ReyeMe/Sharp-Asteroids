using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.Entities
{
    class Bullet
    {
        double dir;
        double velocity;
        PointF position;

        public Bullet(double angle, PointF start, double speed) {
            position = start;
            velocity = speed;
            dir = angle;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void remove() {
            position = new PointF(900,900);
        }

        public PointF GetPosition() {
            return position;
        }

        public void Update() {
            position = CalcPoint(position,dir,velocity);
        }

        public void Draw(ref PaintEventArgs e) {
            Pen laser = new Pen(Color.Yellow,1);
            e.Graphics.DrawLine(laser, position, CalcPoint(position, dir, velocity));
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private PointF CalcPoint(PointF a,double angle, double speed)
        {
            angle = (angle * Math.PI) / 180;

            double Y = speed * Math.Sin(angle);
            double X = speed * Math.Cos(angle);

            PointF c = new PointF(a.X + (float)X, a.Y + (float)Y);

            return c;
        }
    }
}
