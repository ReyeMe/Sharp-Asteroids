using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class Label
    {
        Point pos;

        string lb;

        Font use;

        Brush cl;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public Label(Point position, Color color, string text, string font, float size) {

            pos = position;

            lb = text;

            use = new Font(font,size);

            cl = new SolidBrush(color);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e) {
            e.Graphics.DrawString(lb, use, cl, pos.X, pos.Y);
        }
    }
}
