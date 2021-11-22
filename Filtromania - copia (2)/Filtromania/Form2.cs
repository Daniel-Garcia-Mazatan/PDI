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
        bool masRostros = false;

        //Bitmap fotoTemp;
        Image<Bgr, Byte> fotoTemp;
        Bitmap fotoTempColor;
        int rostros;
        bool estaCapturando = false;

        int RostroR = 0, RostroG = 255, RostroB = 150;
        int[,] mat3x100 = new int[3,100];
        int sigRostro = 0;
        public Form2()
        {
            InitializeComponent();

            for (int i = 0; i < 100; i++) {
                mat3x100[0, i] = RostroR;
                mat3x100[1, i] = RostroG;
                mat3x100[2, i] = RostroB;

                RostroR += 100;
                if (RostroR >= 255)
                    RostroR = RostroR - 255;
                RostroG -= 60;
                if (RostroG <= 0)
                    RostroG = RostroG + 255;
                RostroB += 11;
                if (RostroB >= 255)
                    RostroB = RostroB - 255;
            }

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
            MCvAvgComp[] array = new MCvAvgComp[rostrosDetectadosAhora.Length];
            foreach (MCvAvgComp f in rostrosDetectadosAhora[0])
            {
                resultado = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                for (int j = 0; j < rostrosDetectadosAhora.Length; j++)
                {
                    Rectangle rectangulo = f.rect;
                    Frame.Draw(rectangulo, new Bgr(mat3x100[0, j], mat3x100[1, j], mat3x100[2, j]), 3);
                }
                if (trainingImages.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriterias = new MCvTermCriteria(Cont, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labels.ToArray(), 1500, ref termCriterias);
                    nombre = recognizer.Recognize(resultado);
                    Frame.Draw(nombre, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
                //users[t - 1] = nombre;
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
