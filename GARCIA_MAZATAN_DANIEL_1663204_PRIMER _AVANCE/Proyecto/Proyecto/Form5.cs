using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class Form5 : Form
    {
        Bitmap fotoTemp;
        public Form5(Bitmap fotoTemp)
        {
            InitializeComponent();
            this.fotoTemp = fotoTemp;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = fotoTemp;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form1 forma1 = new Form1();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
