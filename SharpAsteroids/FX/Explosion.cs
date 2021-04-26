using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.FX
{
    class Explosion {

        List<Particles> dots;
        PointF start;
        float scale;
        int maxScale;

        public bool IsDead;
        public bool Pause;

        public Explosion(PointF position, int amount, int size) {
            dots = new List<Particles>();
            start = position;
            scale = 0f;
            maxScale = size;
            IsDead = false;
            Pause = false;

            Random rnd = new Random(DateTime.Now.Millisecond);

            int i;
            for (i = 0; i < amount; i++) {
                Particles dot = new Particles(position,rnd.Next(0,360), rnd.Next(size, size*3), rnd.Next(size*2, size*8));
                dots.Add(dot);
            }
        }

        public void Draw(ref PaintEventArgs e) {
            if (dots.Count == 0 && scale >= maxScale) {
                IsDead = true;
                return;
            }

            foreach (Particles dot in dots) {
                dot.Pause = Pause;
                PointF particle = dot.Update();
                
                Pen stones = new Pen(Color.Snow,1);
                e.Graphics.DrawLine(stones,particle.X, particle.Y,particle.X+1, particle.Y+1);
                stones.Dispose();
            }

            int i;
            for (i = 0; i < dots.Count; i++) {
                if (dots[i].IsDead) {
                    dots.RemoveAt(i);
                    i--;
                }
            }

            Pen ring = new Pen(Color.Yellow,2);

            if(!Pause) scale += 1.2f;
            if(scale < maxScale) e.Graphics.DrawEllipse(ring, new RectangleF(new PointF(start.X-scale,start.Y-scale), new SizeF(scale*2, scale*2)));

            ring.Dispose();

            Pause = true;
        }

    }
}
