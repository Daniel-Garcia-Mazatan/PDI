using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filtromania
{
    public partial class Form5 : Form
    {
        Bitmap fotoOriginal, fotoEditada, fotoTemp, fotoEscalaGrises;
        Image<Gray, Byte> imgGris, imgTemp;
        Image<Gray, float> imgTemp2;
        List<Image<Bgr, Byte>> arrayImagenes;
        List<Bitmap> arrayBitmaps;
        bool existeFiltro = false;
        private int[,] conv3x3 = new int[3, 3];
        private int[,] conv5x5 = new int[5, 5];
        private int factor;
        private int offset;
        public Form5()
        {
            InitializeComponent();
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            /////////////////////////////////NORTE-SUR//////////////////////////////////
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //////////////////////////////Y DE SOBEL///////////////////////////////////
            ///
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ////////////////////////LAPLACIANO//////////////////////////////////

            /*int i = 0;
            while ( i < arrayImagenes.Count)
            {
                imgGris = arrayImagenes[i].Convert<Gray, Byte>();
                imgTemp = imgGris;
                imgTemp2 = imgTemp.Laplace(3);
                imageBox2.Image = imgTemp2;

                i++;
            }*/

            conv3x3 = new int[,] {{0, 1, 0},
                                  {1, -4, 1},
                                  {0, -1, 0}};

            factor = 3;
            offset = 0;

            fotoEscalaGrises = arrayBitmaps[0];

            Convolucion2();

            //fotoEditada = Convolucion3(fotoEscalaGrises, Laplaciano3x3, factor, offset, false);
            this.Invalidate();

            pictureBox2.Image = fotoEditada;



            //imageBox2.Image = fotoEditada;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ////////////////////////////////BORDES DE CANNY////////////////////////////////////
            ///
            Image<Gray, byte> imgTemp3 = new Image<Gray, byte>(arrayImagenes[0].Width, arrayImagenes[0].Height, new Gray(0));
            //imgTemp3 = arrayImagenes[0].Canny(50, 20);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /////////////////////////////MENOS LAPLACIANO/////////////////////////////////////
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {

        }
        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            ///////////////////////////X DE SOBEL////////////////////////////////////////
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpenFileDialog = new OpenFileDialog();

            dlgOpenFileDialog.InitialDirectory = "C:\\";
            dlgOpenFileDialog.Filter = "Archivos de video MOV (*.mov)|*.mov|MP4 (*.mp4)|*.mp4";

            if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                
                //axWindowsMediaPlayer1.URL = dlgOpenFileDialog.FileName;
                arrayImagenes = GetVideoFrames(dlgOpenFileDialog.FileName);
                //imageBox1.Image = (IImage)arrayImagenes[0];
                arrayBitmaps = new List<Bitmap>();

                for(int i = 0; i < arrayImagenes.Count; i++)
                {
                    arrayBitmaps.Add(arrayImagenes[i].ToBitmap());
                }

                pictureBox1.Image = arrayBitmaps[0];
            }
            else
                MessageBox.Show("No seleccionó imagen", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer2_Enter(object sender, EventArgs e)
        {

        }

        private List<Image<Bgr, Byte>> GetVideoFrames(String Filename)
        {
            List<Image<Bgr, Byte>> ArrayImagenes = new List<Image<Bgr, Byte>>();
            Capture _capture = new Capture(Filename);

            bool Reading = true;

            while (Reading)
            {
                Image<Bgr, Byte> frame = _capture.QueryFrame();
                if (frame != null)
                {
                    ArrayImagenes.Add(frame.Copy());
                }
                else
                {
                    Reading = false;
                }
            }

            return ArrayImagenes;
        }

        private void escalaDeGrises(Bitmap fotoColor)
        {
            Color imgPixel;
            int Red, Green, Blue;
            int pixelGris;

            for (int i = 0; i < fotoColor.Width; i++)
            {
                for (int j = 0; j < fotoColor.Height; j++)
                {
                    imgPixel = fotoColor.GetPixel(i, j);

                    Red = imgPixel.R;
                    Green = imgPixel.G;
                    Blue = imgPixel.B;

                    pixelGris = (Red + Green + Blue) / 3;

                    if (pixelGris > 255)
                        pixelGris = 255;
                    else if (pixelGris < 0)
                        pixelGris = 0;

                    fotoEscalaGrises.SetPixel(i, j, Color.FromArgb(pixelGris, pixelGris, pixelGris));
                }
            }
        }

        private Bitmap adicion(Bitmap imagen1, Bitmap imagen2)
        {
            Bitmap bm3 = new Bitmap(imagen2.Width, imagen2.Height);
            Color imgPixel1, imgPixel2;
            int Red1, Green1, Blue1, Red2, Green2, Blue2, sumR, sumG, sumB;
            int pixelGris1, pixelGris2, pixelResultante;

            for (int i = 0; i < bm3.Width; i++)
            {
                for (int j = 0; j < bm3.Height; j++)
                {
                    imgPixel1 = imagen1.GetPixel(i, j);
                    imgPixel2 = imagen2.GetPixel(i, j);

                    Red1 = imgPixel1.R;
                    Green1 = imgPixel1.G;
                    Blue1 = imgPixel1.B;

                    Red2 = imgPixel2.R;
                    Green2 = imgPixel2.G;
                    Blue2 = imgPixel2.B;

                    pixelGris1 = (Red1 + Green1 + Blue1) / 3;
                    pixelGris2 = (Red2 + Green2 + Blue2) / 3;

                    pixelResultante = pixelGris1 + pixelGris2;

                    if (pixelResultante < 0)
                        pixelResultante = 0;
                    else if (pixelResultante > 255)
                        pixelResultante = 255;

                    bm3.SetPixel(i, j, Color.FromArgb(pixelResultante, pixelResultante, pixelResultante));
                }
            }

            return bm3;
        }

        private void Convolucion2()
        {
            escalaDeGrises(fotoEscalaGrises);

            Bitmap bm1 = new Bitmap(fotoEscalaGrises.Width, fotoEscalaGrises.Height);

            for (int x = 1; x < fotoEscalaGrises.Width - 1; x++)
            {
                for (int y = 1; y < fotoEscalaGrises.Height - 1; y++)
                {
                    Color color1, color2, color3, color4, color5, color6, color7, color8, color9;


                    color1 = fotoEscalaGrises.GetPixel(x - 1, y - 1);
                    color2 = fotoEscalaGrises.GetPixel(x, y - 1);
                    color3 = fotoEscalaGrises.GetPixel(x + 1, y - 1);
                    color4 = fotoEscalaGrises.GetPixel(x - 1, y);
                    color5 = fotoEscalaGrises.GetPixel(x, y);
                    color6 = fotoEscalaGrises.GetPixel(x + 1, y);
                    color7 = fotoEscalaGrises.GetPixel(x - 1, y + 1);
                    color8 = fotoEscalaGrises.GetPixel(x, y + 1);
                    color9 = fotoEscalaGrises.GetPixel(x + 1, y + 1);

                    int colorRed = color1.R * conv3x3[0, 0]
                                    + color2.R * conv3x3[1, 0]
                                    + color3.R * conv3x3[2, 0]
                                    + color4.R * conv3x3[0, 1]
                                    + color5.R * conv3x3[1, 1]
                                    + color6.R * conv3x3[2, 1]
                                    + color7.R * conv3x3[0, 2]
                                    + color8.R * conv3x3[1, 2]
                                    + color9.R * conv3x3[2, 2];

                    colorRed = colorRed * factor + offset;


                    if (colorRed < 0)
                        colorRed = 0;
                    else if (colorRed > 255)
                        colorRed = 255;

                    int colorGreen = color1.G * conv3x3[0, 0]
                                    + color2.G * conv3x3[1, 0]
                                    + color3.G * conv3x3[2, 0]
                                    + color4.G * conv3x3[0, 1]
                                    + color5.G * conv3x3[1, 1]
                                    + color6.G * conv3x3[2, 1]
                                    + color7.G * conv3x3[0, 2]
                                    + color8.G * conv3x3[1, 2]
                                    + color9.G * conv3x3[2, 2];

                    colorGreen = colorGreen * factor + offset;

                    if (colorGreen < 0)
                        colorGreen = 0;
                    else if (colorGreen > 255)
                        colorGreen = 255;

                    int colorBlue = color1.B * conv3x3[0, 0]
                                    + color2.B * conv3x3[1, 0]
                                    + color3.B * conv3x3[2, 0]
                                    + color4.B * conv3x3[0, 1]
                                    + color5.B * conv3x3[1, 1]
                                    + color6.B * conv3x3[2, 1]
                                    + color7.B * conv3x3[0, 2]
                                    + color8.B * conv3x3[1, 2]
                                    + color9.B * conv3x3[2, 2];

                    colorBlue = colorBlue * factor + offset;

                    if (colorBlue < 0)
                        colorBlue = 0;
                    else if (colorBlue > 255)
                        colorBlue = 255;

                    int avr = (colorRed + colorGreen + colorBlue) / 3;

                    bm1.SetPixel(x, y, Color.FromArgb(avr, avr, avr));
                }
            }

            if (existeFiltro == false)
            {
                fotoEditada = bm1;
                existeFiltro = true;
            }
            else
            {
                fotoEditada = adicion(bm1, fotoEditada);
            }
        }

        private void Convolution()
        {
            for (int i = 0; i < arrayImagenes.Count; i++) {
                Image<Gray, Byte> imgGris = arrayImagenes[i].Convert<Gray, Byte>(); 
                Image<Gray, Byte> imgTemp = imgGris;
                Image<Gray, float> imgTemp2 = imgTemp.Laplace(3);
                imageBox2.Image = imgTemp2;
            }
        }
    }
}
