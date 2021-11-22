using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filtromania
{
    public partial class Form2 : Form
    {
        HaarCascade detectorDeRostro;
        Image<Bgr, Byte> Frame, Frame2;
        Capture camara;
        Image<Gray, byte> resultado;
        Image<Gray, byte> trainedFace = null;
        Image<Gray, byte> grayFace = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> users = new List<string>();
        int Cont, numLabels, t;
        string nombre, nombres = null;
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);

        bool yasetomo = false;
        string sujeto = "sujeto";

        //Bitmap fotoTemp;
        Image<Bgr, Byte> fotoTemp;
        Bitmap fotoTempColor;
        int rostros;
        bool estaCapturando = false;
        public Form2()
        {
            InitializeComponent();

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
            catch (Exception ex)
            {
                //MessageBox.Show("No hay datos.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yasetomo = false;

            if(estaCapturando == false)
                estaCapturando = true;
            else
            {
                Application.Idle -= FrameProcedure;
                camara.Dispose();
            }
            button2.Enabled = true;
            camara = new Capture();
            camara.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
            //Application.Idle += FrameProcedure;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (estaCapturando == true)
            {
                Application.Idle -= FrameProcedure;
                camara.Dispose();
            }
     
            OpenFileDialog dlgOpenFileDialog = new OpenFileDialog();

            dlgOpenFileDialog.InitialDirectory = "C:\\";
            dlgOpenFileDialog.Filter = "Archivos de imagen (*.jpg)(*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|GIF (*.gif)|*.gif";

            if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageBox1.ImageLocation = dlgOpenFileDialog.FileName;
                imageBox1.Image = new Emgu.CV.Image<Bgr, byte> (dlgOpenFileDialog.FileName);
                fotoTemp = new Emgu.CV.Image<Bgr, byte>(dlgOpenFileDialog.FileName);

                yasetomo = true;
            }
            else
                MessageBox.Show("No seleccionó imagen", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            rostros = 0;
            users.Add("");
            Frame = camara.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            Frame2 = Frame.Convert<Bgr, Byte>();
            grayFace = Frame.Convert<Gray, byte>();
            MCvAvgComp[][] rostrosDetectadosAhora = grayFace.DetectHaarCascade(detectorDeRostro, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            foreach (MCvAvgComp f in rostrosDetectadosAhora[0]) { 
                resultado = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                Frame.Draw(f.rect, new Bgr(Color.DarkGoldenrod), 3);
                Frame.Draw(sujeto + " " + (rostros + 1).ToString(), ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.DarkGoldenrod));
                
                users.Add("");
                
                rostros += 1;
            }

            imageBox1.Image = Frame;
            label3.Text = rostros.ToString();
            nombres = "";
            users.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yasetomo = true;

            Application.Idle -= FrameProcedure;
            camara.Dispose();
            button2.Enabled = false;
            fotoTemp = Frame2;
            
            label3.Text = "";
            estaCapturando = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (imageBox1.Image != null && yasetomo == true)
            {
                fotoTempColor = fotoTemp.ToBitmap();
                Form3 forma3 = new Form3(fotoTempColor);
                forma3.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("No ha seleccionado o tomado una imagen/foto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
