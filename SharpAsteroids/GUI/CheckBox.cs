using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class CheckBox
    {
        Rectangle BOX;

        bool over;
        bool check;
        string ID;

        Color[] events = new Color[2];

        Sound mouseOver;
        Sound mouseClick;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public delegate void OnChange(object sender, bool state, string GOTO);
        public event OnChange Clicked;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public CheckBox(int x, int y, int size, string name, bool enabled, Color idle, Color hover) {
            BOX = new Rectangle(x,y,size,size);

            check = enabled;
            over = false;

            ID = name;

            events[0] = idle;
            events[1] = hover;

            mouseClick = new Sound("checkboxclick.wav");
            mouseOver = new Sound("buttonrollover.wav");
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void onMouseOver(MouseEventArgs e)
        {
            if (!BOX.Contains(new Point(e.X, e.Y))) { over = false; return; }

            if (!over) mouseOver.play();
            over = true;
        }

        public void OnMouse(MouseEventArgs e)
        {
            if (!BOX.Contains(new Point(e.X, e.Y))) return;

            check = !check;

            mouseClick.play();
            Clicked.Invoke(this, check, ID);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e, bool effects)
        {

            Color mouse = (over) ? events[1] : events[0];

            Pen color = new Pen(mouse, 1);

            e.Graphics.DrawRectangle(color, BOX);

            if (over && effects)
            {
                int i, r = 255, g = 106,
                    step = 255 / 5;

                for (i = 0; i < 5; i++)
                {
                    r -= step;
                    g -= step;

                    if (r < 0) r = 0;
                    if (g < 0) g = 0;

                    e.Graphics.DrawRectangle(new Pen(Color.FromArgb(r, g, 0), 1), BOX.X - i - 1, BOX.Y - i - 1, BOX.Width + 2 * i + 2, BOX.Height + 2 * i + 2);
                }
            }

            if (check) {
                e.Graphics.FillRectangle(new SolidBrush(mouse),BOX.X+5,BOX.Y+5,BOX.Width-9,BOX.Height-9);
            }

        }
    }
}
