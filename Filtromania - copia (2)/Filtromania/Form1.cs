using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Filtromania
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = "https://github.com/Daniel-Garcia-Mazatan/PDI/blob/main/GARCIA_MAZATAN_DANIEL_1663204_PRIMER%20_AVANCE/MANUAL_DE_USUARIO.pdf";
            linkLabel1.Links.Add(link);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"C:\Users\ASUS\Documents\GitHub\PDI\GARCIA_MAZATAN_DANIEL_1663204_PRIMER _AVANCE\MANUAL_DE_USUARIO.pdf");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 forma2 = new Form2();
            forma2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 forma4 = new Form4();
            forma4.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }
    }
}
