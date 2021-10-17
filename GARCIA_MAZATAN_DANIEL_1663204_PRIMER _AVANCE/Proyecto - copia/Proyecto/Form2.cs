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
    public partial class Form2 : Form
    {

        private string path = @"C:\Users\ASUS\Documents\GitHub\PDI\GARCIA_MAZATAN_DANIEL_1663204_PRIMER _AVANCE\Proyecto\Proyecto\Fotos-Videos\";
        private bool hayDispositivos;
        private FilterInfoCollection misDispositivos;
        private VideoCaptureDevice miWebCam;

        Bitmap fotoTemp;

        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        //HaarCascade detectorDeRostro;
        
        /*
        Image<Bgr, Byte> Frame;
        Capture camara;
        Image<Gray, byte> resultado;
        Image<Gray, byte> trainedFace = null;
        Image<Gray, byte> grayFace = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> users = new List<string>();
        int Cont, numLabels, t;
        string nombre, nombres = null;
        */
        //MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        public Form2()
        {
            InitializeComponent();
            //detectorDeRostros = new HaarCascade("haarcascade_frontalface_default.xml
            /*
            detectorDeRostro = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                string labelsInf = File.ReadAllText(Application.StartupPath + "/Rostros/Rostros.txt");
                string[] Labels = labelsInf.Split(',');
                numLabels = Convert.ToInt16(Labels[0]);
                Cont = numLabels;
                string cargaRostros;
                for (int i = 1; i < numLabels; i++)
                {
                    cargaRostros = "rostro" + i + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/Rostros/Rostros.txt"));
                    labels.Add(labels[i]);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("No hay datos.");
            }
            */
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cargaDispositivos();
        }

        public void cargaDispositivos()
        {
            misDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if(misDispositivos.Count > 0)
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

           

            byte[] imagen2 = new byte[imagen.Width * imagen.Height * 3];

            

            


            //Image<Bgr, byte> grayImage = new Image<Bgr, byte>(imagen2);
            /*
            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(imagen.Width, imagen.Height);
            Rectangle[] rectangles = detectorDeRostro.DetectMultiScale(grayImage, 1.2, 1);
            foreach (Rectangle rectangle in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(imagen))
                {
                    using (Pen pen = new Pen(Color.White, 1))
                    {
                        graphics.DrawRectangle(pen, rectangle);
                    }
                }
            }
            */

            pictureBox1.Image = imagen;
            //pictureBox1.Image = m.ToImage<Bgr, byte>().Bitmap;
        }

        public static byte[] ImageToByteArray(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        

        private void apagarWebCam()
        {
            if(miWebCam != null && miWebCam.IsRunning)
            {
                miWebCam.SignalToStop();
                miWebCam = null;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 forma3 = new Form3(fotoTemp);
            forma3.ShowDialog();
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
            

            /*
            camara = new Capture();
            camara.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
            */
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            /*users.Add("");
            Frame = camara.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            grayFace = Frame.Convert<Gray, byte>();
            MCvAvgComp[][] rostrosDetectadosAhora = grayFace.DetectHaarCascade(detectorDeRostro, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            foreach(MCvAvgComp f in rostrosDetectadosAhora[0])
            {
                resultado = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                Frame.Draw(f.rect, new Bgr(Color.Green), 3);
                if(trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriterias = new MCvTermCriteria(Cont, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labels.ToArray(), 1500, ref termCriterias);
                    nombre = recognizer.Recognize(resultado);
                    //Frame.Draw(nombre, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
                users[t - 1] = nombre;
                users.Add("");
            }
            //pictureBox1.Image = Frame;
            nombres = "";
            users.Clear();
            */
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(miWebCam != null && miWebCam.IsRunning)
            {
                fotoTemp = (Bitmap)pictureBox1.Image;

                //string fecha = DateTime.Now.ToString("yyyyMMdd");
                //string hora = DateTime.Now.ToString("hhmmss");
                //pictureBox1.Image.Save(path + fecha + hora + ".jpg", ImageFormat.Jpeg);
                apagarWebCam();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            apagarWebCam();
            this.Close();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            apagarWebCam();
        }

  
    }
}
