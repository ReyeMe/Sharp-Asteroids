using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class Picture
    {
        Point pos;
        Image grp;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public Picture(Point position, Image bitmap) {

            pos = position;

            grp = bitmap;

        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e)
        {
            e.Graphics.DrawImage(grp, pos);
        }
    }
}
