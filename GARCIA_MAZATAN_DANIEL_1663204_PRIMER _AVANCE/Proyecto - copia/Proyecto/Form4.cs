using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class Form4 : Form
    {
        private string path = @"C:\Users\ASUS\Documents\GitHub\PDI\GARCIA_MAZATAN_DANIEL_1663204_PRIMER _AVANCE\Proyecto\Proyecto\Fotos-Videos\";
        private bool hayDispositivos;
        private FilterInfoCollection misDispositivos;
        private VideoCaptureDevice miWebCam;

        Bitmap fotoTemp;

        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            cargaDispositivos();
        }

        public void cargaDispositivos()
        {
            misDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (misDispositivos.Count > 0)
            {
                hayDispositivos = true;
                for (int i = 0; i < misDispositivos.Count; i++)
                    comboBox1.Items.Add(misDispositivos[i].Name.ToString());
                comboBox1.Text = misDispositivos[0].ToString();
            }
            else
                hayDispositivos = false;
        }

        private void camaraCaptura(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap imagen = (Bitmap)eventArgs.Frame.Clone();

            pictureBox1.Image = imagen;
            
        }

        private void apagarWebCam()
        {
            if (miWebCam != null && miWebCam.IsRunning)
            {
                miWebCam.SignalToStop();
                miWebCam = null;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (miWebCam != null && miWebCam.IsRunning)
            {
                fotoTemp = (Bitmap)pictureBox1.Image;

                //string fecha = DateTime.Now.ToString("yyyyMMdd");
                //string hora = DateTime.Now.ToString("hhmmss");
                //pictureBox1.Image.Save(path + fecha + hora + ".jpg", ImageFormat.Jpeg);
                apagarWebCam();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            apagarWebCam();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form5 forma5 = new Form5(fotoTemp);
            forma5.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            apagarWebCam();
            int i = comboBox1.SelectedIndex;
            string nombre = misDispositivos[i].MonikerString;
            miWebCam = new VideoCaptureDevice(nombre);
            miWebCam.NewFrame += new NewFrameEventHandler(camaraCaptura);
            miWebCam.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            apagarWebCam();
        }
    }
}
