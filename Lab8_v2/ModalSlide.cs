using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8_v2
{
    public partial class ModalSlide : Form
    {

        public ModalSlide()
        {
            InitializeComponent();
        }

        public int slideIndex = 0;                          // index of image in lsit box of images 

        protected override void OnPaint(PaintEventArgs e)       // cycle through images 
        {
            Graphics g = e.Graphics;

            string disPic = (string)((Form1)Owner).fileNameListBox.Items[slideIndex];           // img fname
            try                                               // open and center images in modal window

            {
                Image hawtPic = Image.FromFile(disPic);
                float sz = Math.Min(ClientSize.Height / (float)hawtPic.Height, ClientSize.Width / (float)hawtPic.Width);
                e.Graphics.DrawImage(hawtPic, (ClientSize.Width - (float)hawtPic.Width * sz) / 2f, (ClientSize.Height - (float)hawtPic.Height * sz) / 2f, (float)hawtPic.Width * sz, (float)hawtPic.Height * sz);
            }
            catch                                            // display cray image if not an image file
            {
                e.Graphics.DrawString("Not an image file!", new Font("Arial", 40), Brushes.Red, 25, 25);
            }
        }
            
        private void ModalSlide_KeyDown(object sender, KeyEventArgs e)                  // close when escape is pressed 
        {
            if (e.KeyData == Keys.Escape)
                Close();
        }

        private void timer1_Tick(object sender, EventArgs e)                            // use timer to track when to change images               
        {
            ++slideIndex;                                           
            if (slideIndex == ((Form1)Owner).fileNameListBox.Items.Count)               // if exceed index of image list, close window and reset
            {
                timer1.Enabled = false;
                DialogResult = DialogResult.OK;
                return;
            }
            else
                base.Invalidate();                                                      // refresh
        }

        private void ModalSlide_Activated(object sender, EventArgs e)
        {
            timer1.Interval = ((Form1)Owner).slideInterval * 1000;
            timer1.Enabled = true;
        }
    }
}
