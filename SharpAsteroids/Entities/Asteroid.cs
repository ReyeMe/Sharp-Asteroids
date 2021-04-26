using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.Entities
{
    class Asteroid {
        int type;
        double dir;
        double velocity;
        public PointF position;


        double[] geometry;

        public Asteroid( double angle, PointF start, double speed, int size )
        {
            position = start;
            velocity = speed;
            dir = angle;
            type = size;

            CreateGeometry(out geometry);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public PointF GetPosition()
        {
            return position;
        }

        public void remove()
        {
            position = new PointF(900, 900);
        }

        public int GetSize()
        {
            return type;
        }

        public double GetAngle()
        {
            return velocity;
        }

        public void Update()
        {
            position = CalcPoint(position, dir, velocity);
        }

        public bool Collide( PointF thing )
        {

            double dist = Math.Sqrt(Math.Pow((position.X - thing.X), 2) + Math.Pow((position.Y - thing.Y), 2));

            return dist < (type * 10);
        }

        public void Draw( ref PaintEventArgs e )
        {
            Pen stone = new Pen(Color.Gray, 2);

            int cons = 10;
            int size = type * cons;

            PointF p1 = CalcPoint(new PointF(0, 1), 0, size*geometry[0]);
            PointF p2 = CalcPoint(new PointF(0, 1), 45, size * geometry[1]);
            e.Graphics.DrawLine(stone, position.X - p1.X , position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 90, size * geometry[2]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 135, size * geometry[3]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 180,  size * geometry[4]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 225, size * geometry[5]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 270, size * geometry[6]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 315, size * geometry[7]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            p1 = p2;
            p2 = CalcPoint(new PointF(0, 1), 360, size * geometry[0]);
            e.Graphics.DrawLine(stone, position.X - p1.X, position.Y - p1.Y, position.X - p2.X, position.Y - p2.Y);

            //e.Graphics.DrawEllipse(stone, position.X - size, position.Y - size, size * 2, size * 2);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        private void CreateGeometry( out double[] geometry )
        {
            Random gen = new Random(DateTime.Now.Second);

            geometry = new double[8];

            int i = 0;
            foreach (double deform in geometry)
            {
                geometry[i++] = gen.Next(8,13)/10.0;
            }
        }

        private PointF CalcPoint( PointF a, double angle, double speed )
        {
            angle = (angle * Math.PI) / 180;

            double Y = speed * Math.Sin(angle);
            double X = speed * Math.Cos(angle);
            PointF c = new PointF(a.X + (float)X, a.Y + (float)Y);

            return c;
        }
    }
}
