using System.Collections.Generic;
using System.Windows.Forms;

namespace SharpAsteroids.GUI
{
    class Screen
    {
        public List<Button> BUTTONS;
        public List<CheckBox> CHECKS;
        public List<Picture> PICTURES;
        public List<Label> LABELS;
        public List<Input> INPUTS;
        
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public Screen() {
            BUTTONS = new List<Button>();
            CHECKS = new List<CheckBox>();
            PICTURES = new List<Picture>();
            LABELS = new List<Label>();
            INPUTS = new List<Input>();
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void MouseMove(MouseEventArgs e) {
            foreach (GUI.Button btn in BUTTONS) {
                btn.onMouseOver(e);
            }

            foreach (CheckBox chck in CHECKS)
            {
                chck.onMouseOver(e);
            }
        }

        public void MouseClick(MouseEventArgs e) {
            foreach (GUI.Button btn in BUTTONS)
            {
                btn.OnMouse(e);
            }

            foreach (CheckBox chck in CHECKS)
            {
                chck.OnMouse(e);
            }

            foreach (GUI.Input inp in INPUTS)
            {
                inp.Select(e);
            }
        }

        public void KeyPress(KeyPressEventArgs e) {
            foreach (GUI.Input inp in INPUTS)
            {
                inp.update(e);
            }
        }

        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...
        //... ... ... ... ... ... ... ... ... ... ... ... ... ... ... ...

        public void Draw(ref PaintEventArgs e, bool effects) {
            foreach (Button btn in BUTTONS)
            {
                btn.Draw(ref e, effects);
            }

            foreach (CheckBox chck in CHECKS)
            {
                chck.Draw(ref e, effects);
            }

            foreach (Picture pic in PICTURES)
            {
                pic.Draw(ref e);
            }

            foreach (Label lab in LABELS)
            {
                lab.Draw(ref e);
            }

            foreach (Input inp in INPUTS)
            {
                inp.Draw(ref e, effects);
            }

        }
    }
}
