namespace SharpAsteroids
{
    partial class render
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(render));
            this.loop = new System.Windows.Forms.Timer(this.components);
            this.fps_counter = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // loop
            // 
            this.loop.Interval = 15;
            this.loop.Tick += new System.EventHandler(this.loop_Tick);
            // 
            // fps_counter
            // 
            this.fps_counter.Enabled = true;
            this.fps_counter.Interval = 1000;
            this.fps_counter.Tick += new System.EventHandler(this.fps_counter_Tick);
            // 
            // render
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "render";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpAsteroids";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.render_Exit);
            this.Load += new System.EventHandler(this.render_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.engine_paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.engine_key_press);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.engine_mouse_click);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.engine_mouse_move);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer loop;
        private System.Windows.Forms.Timer fps_counter;
    }
}

