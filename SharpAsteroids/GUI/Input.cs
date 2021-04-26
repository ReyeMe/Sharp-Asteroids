using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class Input
    {
        bool selected = false;

        string write = "";
        string id;

        int max;

        Font use;
        Brush cl;
        Rectangle rect;
        Pen color;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public delegate void OnFill(object sender, string output, string from);
        public event OnFill Fill;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public Input(Point position, Size dimensions,Color border_color, Color text_color, string name, string font, float size, int max_char, bool select) {
            cl = new SolidBrush(text_color);

            use = new Font(font, size);

            color = new Pen(border_color,1);

            rect = new Rectangle(position,dimensions);

            max = max_char;

            id = name;

            selected = select;
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Select(MouseEventArgs e) {
            if (!rect.Contains(new Point(e.X, e.Y))) { selected = false; return; }
            selected = true;
        }

        public void update(KeyPressEventArgs e) {
            if (!selected) return;
            if (write.Length > 0 && e.KeyChar == (Int32)Keys.Enter) { Fill(this,write,id); return; }
            if (write.Length > 0 && e.KeyChar == (Int32)Keys.Back) { write = write.Remove(write.Length - 1, 1); return; }
            if (write.Length > max || e.KeyChar == (Int32)Keys.Back || e.KeyChar == (Int32)Keys.Enter) return;
            if (((Int32)e.KeyChar > 47 && (Int32)e.KeyChar < 58) || ((Int32)e.KeyChar > 64 && (Int32)e.KeyChar < 91) || ((Int32)e.KeyChar > 96 && (Int32)e.KeyChar < 123)) write += e.KeyChar;
        }

        public string Get() {
            return write;
        }

        public void Clear() {
            write = "";
        }

        public void Set(string text) {
            write = text;
        }
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e, bool effects) {

            e.Graphics.DrawRectangle(color, rect);

            e.Graphics.DrawString(write + ((selected)?"_":""), use, cl, rect.X, rect.Y);

            if (selected && effects)
            {
                int i, r = 255, g = 106,
                    step = 255 / 5;

                for (i = 0; i < 5; i++)
                {
                    r -= step;
                    g -= step;

                    if (r < 0) r = 0;
                    if (g < 0) g = 0;

                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(r, g, 0), 1), rect.X - i - 1, rect.Y - i - 1, rect.Width + 2 * i + 2, rect.Height + 2 * i + 2);
                }
            }

        }
    }
}
