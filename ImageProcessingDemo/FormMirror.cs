using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imageprocessing
{
    public partial class FormMirror : Form
    {
        public FormMirror()
        {
            InitializeComponent();
        }

        public void setframe(Mat frame)
        {
            //imageBox.Image = frame;
           
          //  imageBox.Invoke((MethodInvoker)(() => imageBox.Image = frame));
        }

        public void frame(Image frame)
        {

            //pictureBox1.Image = frame;
            pictureBox1.Invoke((MethodInvoker)(() => pictureBox1.Image = frame));
        }
    }
}
