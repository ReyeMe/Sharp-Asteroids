using System.Drawing;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class Button
    {
        Rectangle BOX;

        int f_size;

        string label;
        string id;

        bool over = false;

        Color[] events;

        Sound mouseOver;
        Sound mouseClick;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public delegate void OnClick(object sender, string GOTO);
        public event OnClick Clicked;

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public Button(int x, int y, int width, int height, int text_size, string text, string GOTO, Color idle, Color hover) {

            BOX = new Rectangle(new Point(x,y),new Size(width,height));

            label = text;

            id = GOTO;

            f_size = text_size;

            events = new Color[2] { idle, hover };

            mouseOver = new Sound("buttonrollover.wav");
            mouseClick = new Sound("buttonclick.wav");
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void onMouseOver(MouseEventArgs e) {

            if (!BOX.Contains(new Point(e.X, e.Y))) { over = false; return; }

            if(!over) mouseOver.play();
            over = true;
        }

        public void OnMouse(MouseEventArgs e) {

            if (!BOX.Contains(new Point(e.X, e.Y))) return;

            mouseClick.play();
            Clicked.Invoke(this, id);
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e, bool effects) {

            Color mouse = (over) ? events[1] : events[0];

            Pen color = new Pen(mouse, 1);
            Font text = new Font("Arial", f_size);
            Brush t_color = new SolidBrush(mouse);

            e.Graphics.DrawRectangle(color, BOX);

            e.Graphics.DrawString(label, text, t_color, BOX.X, BOX.Y);

            if (over && effects) {
                int i, a = 255,
                    step = 255/5;

                for (i = 0; i < 5; i++) {
                    a -= step;

                    if (a < 0) a = 0;

                   e.Graphics.DrawRectangle(new Pen(Color.FromArgb(a,mouse.R,mouse.G,mouse.B),1), BOX.X-i-1,BOX.Y-i-1,BOX.Width+2*i+2,BOX.Height+2*i+2);
                }
            }

        }
    }
}
