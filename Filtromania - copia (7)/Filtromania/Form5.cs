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
        private int[,] conv3x3, conv3x3dos = new int[3, 3];
        private int[,] conv5x5 = new int[5, 5];
        private int factor;
        private int offset;

        private string filtro = "";
        Capture _capture;

        Image<Bgr, Byte> frameActual;
        public Form5()
        {
            InitializeComponent();
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            /////////////////////////NORTE-SUR///////////////////////////////
            
            filtro = "norte-sur";

            conv3x3 = new int[,] {{1, 1, 1},
                                  {1, -2, 1},
                                  {-1, -1, -1}};

            factor = 1;
            offset = 0;

            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ///////////////////Y DE SOBEL///////////////////////////

            filtro = "sobelY";

            conv3x3 = new int[,] {{-1, -2, -1},
                                  {0, 0, 0},
                                  {1, 2, 1}};

            factor = 1;
            offset = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ////////////////////////LAPLACIANO//////////////////////////////////

            filtro = "laplaciano";

            conv3x3 = new int[,] {{0, 1, 0},
                                  {1, -4, 1},
                                  {0, -1, 0}};

            factor = 1;
            offset = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ////////////////////BORDES DE CANNY///////////////////////

            filtro = "Canny";

            conv5x5 = new int[,] {{2, 4, 5, 4, 2},
                                  {4, 9, 12, 9, 4},
                                  {5, 12, 15, 12, 5},
                                  {4, 9, 12, 9, 4},
                                  {2, 4, 5, 4, 2} };

            factor = 115;
            offset = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /////////////////MENOS LAPLACIANO///////////////////////

            filtro = "menoslaplaciano";

            conv3x3 = new int[,] {{0, -1, 0},
                                  {-1, 5, -1},
                                  {0, -1, 0}};

            factor = 1;
            offset = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void button10_Click(object sender, EventArgs e)
        {
            filtro = "reproducir";
            timer1.Enabled = true;
        }
        private void button11_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
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
            ///////////////////KIRSCH HORIZONTAL///////////////////////////
            
            filtro = "Kirsch";

            conv3x3 = new int[,] {{ 5,  5, 5},
                                  {-3,  0, -3},
                                  {-3, -3, -3}};

            factor = 1;
            offset = 0;

            Convolucion4();

            this.Invalidate();

            pictureBox1.Image = fotoEditada;
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            OpenFileDialog dlgOpenFileDialog = new OpenFileDialog();

            dlgOpenFileDialog.InitialDirectory = "C:\\";
            dlgOpenFileDialog.Filter = "Archivos de video MOV (*.mov)|*.mov|MP4 (*.mp4)|*.mp4";

            if (dlgOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                _capture = new Capture(dlgOpenFileDialog.FileName);
                arrayBitmaps = new List<Bitmap>();

                frameActual = _capture.QueryFrame();
                fotoEscalaGrises = frameActual.Bitmap;

                pictureBox1.Image = frameActual.Bitmap;

                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                button10.Enabled = true;
                button11.Enabled = true;
            }
            else
                MessageBox.Show("No seleccionó video", "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            frameActual = _capture.QueryFrame();
            if (frameActual != null)
            {
                pictureBox1.Image = frameActual.Bitmap;
            }
            else
            {
                timer1.Enabled = false;
            }

            frameActual = _capture.QueryFrame();
            if(frameActual != null)
            {
                if (filtro == "norte-sur" || filtro == "laplaciano" || filtro == "menoslaplaciano")
                {
                    Convolucion2();
                    pictureBox1.Image = fotoEditada;
                }
                else if(filtro == "sobelY")
                {
                    Convolucion2();
                    Umbral2();
                    pictureBox1.Image = fotoEditada;
                }
                else if(filtro == "Canny")
                {
                    Convolucion3();
                    pictureBox1.Image = fotoEditada;
                }
                else if(filtro == "Kirsch")
                {
                    Convolucion4();
                    pictureBox1.Image = fotoEditada;
                }
                else if(filtro == "reproducir")
                {
                    pictureBox1.Image = frameActual.Bitmap;
                }
            }
            else
            {
                timer1.Enabled = false;
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
            escalaDeGrises(frameActual.Bitmap);

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

                    colorRed = colorRed / factor + offset;


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

                    colorGreen = colorGreen / factor + offset;

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

                    colorBlue = colorBlue / factor + offset;

                    if (colorBlue < 0)
                        colorBlue = 0;
                    else if (colorBlue > 255)
                        colorBlue = 255;

                    int avr = (colorRed + colorGreen + colorBlue) / 3;

                    bm1.SetPixel(x, y, Color.FromArgb(avr, avr, avr));
                }
            }

            fotoEditada = bm1;
        }

        private void Convolucion3()
        {
            escalaDeGrises(fotoEscalaGrises);

            Bitmap bm1 = new Bitmap(fotoEscalaGrises.Width, fotoEscalaGrises.Height);

            //////////////////////////////// PASO 1 ////////////////////////////////////////////////////

            for (int x = 2; x < fotoEscalaGrises.Width - 2; x++)
            {
                for (int y = 2; y < fotoEscalaGrises.Height - 2; y++)
                {
                    Color color1, color2, color3, color4, color5, color6, color7, color8, color9, color10,
                        color11, color12, color13, color14, color15, color16, color17, color18, color19,
                        color20, color21, color22, color23, color24, color25;


                    color1 = fotoEscalaGrises.GetPixel(x - 2, y - 2);
                    color2 = fotoEscalaGrises.GetPixel(x - 1, y - 2);
                    color3 = fotoEscalaGrises.GetPixel(x, y - 2);
                    color4 = fotoEscalaGrises.GetPixel(x + 1, y - 2);
                    color5 = fotoEscalaGrises.GetPixel(x + 2, y - 2);
                    color6 = fotoEscalaGrises.GetPixel(x - 2, y - 1);
                    color7 = fotoEscalaGrises.GetPixel(x - 1, y - 1);
                    color8 = fotoEscalaGrises.GetPixel(x, y - 1);
                    color9 = fotoEscalaGrises.GetPixel(x + 1, y - 1);
                    color10 = fotoEscalaGrises.GetPixel(x + 2, y - 1);
                    color11 = fotoEscalaGrises.GetPixel(x - 2, y);
                    color12 = fotoEscalaGrises.GetPixel(x - 1, y);
                    color13 = fotoEscalaGrises.GetPixel(x, y);
                    color14 = fotoEscalaGrises.GetPixel(x + 1, y);
                    color15 = fotoEscalaGrises.GetPixel(x + 2, y);
                    color16 = fotoEscalaGrises.GetPixel(x - 2, y + 1);
                    color17 = fotoEscalaGrises.GetPixel(x - 1, y + 1);
                    color18 = fotoEscalaGrises.GetPixel(x, y + 1);
                    color19 = fotoEscalaGrises.GetPixel(x + 1, y + 1);
                    color20 = fotoEscalaGrises.GetPixel(x + 2, y + 1);
                    color21 = fotoEscalaGrises.GetPixel(x - 2, y + 2);
                    color22 = fotoEscalaGrises.GetPixel(x - 1, y + 2);
                    color23 = fotoEscalaGrises.GetPixel(x, y + 2);
                    color24 = fotoEscalaGrises.GetPixel(x + 1, y + 2);
                    color25 = fotoEscalaGrises.GetPixel(x + 2, y + 2);


                    int colorRed = color1.R * conv5x5[0, 0]
                                    + color2.R * conv5x5[1, 0]
                                    + color3.R * conv5x5[2, 0]
                                    + color4.R * conv5x5[3, 0]
                                    + color5.R * conv5x5[4, 0]
                                    + color6.R * conv5x5[0, 1]
                                    + color7.R * conv5x5[1, 1]
                                    + color8.R * conv5x5[2, 1]
                                    + color9.R * conv5x5[3, 1]
                                    + color10.R * conv5x5[4, 1]
                                    + color11.R * conv5x5[0, 2]
                                    + color12.R * conv5x5[1, 2]
                                    + color13.R * conv5x5[2, 2]
                                    + color14.R * conv5x5[3, 2]
                                    + color15.R * conv5x5[4, 2]
                                    + color16.R * conv5x5[0, 3]
                                    + color17.R * conv5x5[1, 3]
                                    + color18.R * conv5x5[2, 3]
                                    + color19.R * conv5x5[3, 3]
                                    + color20.R * conv5x5[4, 3]
                                    + color21.R * conv5x5[0, 4]
                                    + color22.R * conv5x5[1, 4]
                                    + color23.R * conv5x5[2, 4]
                                    + color24.R * conv5x5[3, 4]
                                    + color25.R * conv5x5[4, 4];

                    colorRed = colorRed / factor + offset;


                    if (colorRed < 0)
                        colorRed = 0;
                    else if (colorRed > 255)
                        colorRed = 255;

                    int colorGreen = color1.G * conv5x5[0, 0]
                                    + color2.G * conv5x5[1, 0]
                                    + color3.G * conv5x5[2, 0]
                                    + color4.G * conv5x5[3, 0]
                                    + color5.G * conv5x5[4, 0]
                                    + color6.G * conv5x5[0, 1]
                                    + color7.G * conv5x5[1, 1]
                                    + color8.G * conv5x5[2, 1]
                                    + color9.G * conv5x5[3, 1]
                                    + color10.G * conv5x5[4, 1]
                                    + color11.G * conv5x5[0, 2]
                                    + color12.G * conv5x5[1, 2]
                                    + color13.G * conv5x5[2, 2]
                                    + color14.G * conv5x5[3, 2]
                                    + color15.G * conv5x5[4, 2]
                                    + color16.G * conv5x5[0, 3]
                                    + color17.G * conv5x5[1, 3]
                                    + color18.G * conv5x5[2, 3]
                                    + color19.G * conv5x5[3, 3]
                                    + color20.G * conv5x5[4, 3]
                                    + color21.G * conv5x5[0, 4]
                                    + color22.G * conv5x5[1, 4]
                                    + color23.G * conv5x5[2, 4]
                                    + color24.G * conv5x5[3, 4]
                                    + color25.G * conv5x5[4, 4];

                    colorGreen = colorGreen / factor + offset;

                    if (colorGreen < 0)
                        colorGreen = 0;
                    else if (colorGreen > 255)
                        colorGreen = 255;

                    int colorBlue = color1.B * conv5x5[0, 0]
                                    + color2.B * conv5x5[1, 0]
                                    + color3.B * conv5x5[2, 0]
                                    + color4.B * conv5x5[3, 0]
                                    + color5.B * conv5x5[4, 0]
                                    + color6.B * conv5x5[0, 1]
                                    + color7.B * conv5x5[1, 1]
                                    + color8.B * conv5x5[2, 1]
                                    + color9.B * conv5x5[3, 1]
                                    + color10.B * conv5x5[4, 1]
                                    + color11.B * conv5x5[0, 2]
                                    + color12.B * conv5x5[1, 2]
                                    + color13.B * conv5x5[2, 2]
                                    + color14.B * conv5x5[3, 2]
                                    + color15.B * conv5x5[4, 2]
                                    + color16.B * conv5x5[0, 3]
                                    + color17.B * conv5x5[1, 3]
                                    + color18.B * conv5x5[2, 3]
                                    + color19.B * conv5x5[3, 3]
                                    + color20.B * conv5x5[4, 3]
                                    + color21.B * conv5x5[0, 4]
                                    + color22.B * conv5x5[1, 4]
                                    + color23.B * conv5x5[2, 4]
                                    + color24.B * conv5x5[3, 4]
                                    + color25.B * conv5x5[4, 4];

                    colorBlue = colorBlue / factor + offset;

                    if (colorBlue < 0)
                        colorBlue = 0;
                    else if (colorBlue > 255)
                        colorBlue = 255;

                    int avr = (colorRed + colorGreen + colorBlue) / 3;

                    bm1.SetPixel(x, y, Color.FromArgb(avr, avr, avr));
                }
            }

            ///////////////////////////////////// PASO 2 //////////////////////////////////////////
            Bitmap bm2 = new Bitmap(bm1.Width, bm1.Height);

            factor = 1;

            conv3x3 = new int[,] {{-1, 0, 1},
                                  {-2, 0, 2},
                                  {-1, 0, 1}};

            conv3x3dos = new int[,] {{-1, -2, -1},
                                     { 0,  0,  0},
                                     { 1,  2,  1}};

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

                    colorRed = colorRed / factor + offset;

                    int colorRed2 = color1.R * conv3x3dos[0, 0]
                                    + color2.R * conv3x3dos[1, 0]
                                    + color3.R * conv3x3dos[2, 0]
                                    + color4.R * conv3x3dos[0, 1]
                                    + color5.R * conv3x3dos[1, 1]
                                    + color6.R * conv3x3dos[2, 1]
                                    + color7.R * conv3x3dos[0, 2]
                                    + color8.R * conv3x3dos[1, 2]
                                    + color9.R * conv3x3dos[2, 2];

                    colorRed2 = colorRed2 / factor + offset;

                    if (colorRed < 0)
                        colorRed = 0;
                    else if (colorRed > 255)
                        colorRed = 255;

                    if (colorRed2 < 0)
                        colorRed2 = 0;
                    else if (colorRed2 > 255)
                        colorRed2 = 255;

                    int colorGreen = color1.G * conv3x3[0, 0]
                                    + color2.G * conv3x3[1, 0]
                                    + color3.G * conv3x3[2, 0]
                                    + color4.G * conv3x3[0, 1]
                                    + color5.G * conv3x3[1, 1]
                                    + color6.G * conv3x3[2, 1]
                                    + color7.G * conv3x3[0, 2]
                                    + color8.G * conv3x3[1, 2]
                                    + color9.G * conv3x3[2, 2];

                    colorGreen = colorGreen / factor + offset;

                    int colorGreen2 = color1.G * conv3x3dos[0, 0]
                                    + color2.G * conv3x3dos[1, 0]
                                    + color3.G * conv3x3dos[2, 0]
                                    + color4.G * conv3x3dos[0, 1]
                                    + color5.G * conv3x3dos[1, 1]
                                    + color6.G * conv3x3dos[2, 1]
                                    + color7.G * conv3x3dos[0, 2]
                                    + color8.G * conv3x3dos[1, 2]
                                    + color9.G * conv3x3dos[2, 2];

                    colorGreen2 = colorGreen2 / factor + offset;

                    if (colorGreen < 0)
                        colorGreen = 0;
                    else if (colorGreen > 255)
                        colorGreen = 255;

                    if (colorGreen2 < 0)
                        colorGreen2 = 0;
                    else if (colorGreen2 > 255)
                        colorGreen2 = 255;

                    int colorBlue = color1.B * conv3x3[0, 0]
                                    + color2.B * conv3x3[1, 0]
                                    + color3.B * conv3x3[2, 0]
                                    + color4.B * conv3x3[0, 1]
                                    + color5.B * conv3x3[1, 1]
                                    + color6.B * conv3x3[2, 1]
                                    + color7.B * conv3x3[0, 2]
                                    + color8.B * conv3x3[1, 2]
                                    + color9.B * conv3x3[2, 2];

                    colorBlue = colorBlue / factor + offset;

                    int colorBlue2 = color1.B * conv3x3dos[0, 0]
                                    + color2.B * conv3x3dos[1, 0]
                                    + color3.B * conv3x3dos[2, 0]
                                    + color4.B * conv3x3dos[0, 1]
                                    + color5.B * conv3x3dos[1, 1]
                                    + color6.B * conv3x3dos[2, 1]
                                    + color7.B * conv3x3dos[0, 2]
                                    + color8.B * conv3x3dos[1, 2]
                                    + color9.B * conv3x3dos[2, 2];

                    colorBlue2 = colorBlue2 / factor + offset;

                    if (colorBlue < 0)
                        colorBlue = 0;
                    else if (colorBlue > 255)
                        colorBlue = 255;

                    if (colorBlue2 < 0)
                        colorBlue2 = 0;
                    else if (colorBlue2 > 255)
                        colorBlue2 = 255;

                    int avr1 = (colorRed + colorGreen + colorBlue) / 3;
                    int avr2 = (colorRed2 + colorGreen2 + colorBlue2) / 3;

                    double sqrt = Math.Sqrt(Math.Pow(avr1, 2) + Math.Pow(avr2, 2));

                    if (sqrt < 0)
                        sqrt = 0;
                    else if (sqrt > 255)
                        sqrt = 255;

                    bm2.SetPixel(x, y, Color.FromArgb(Convert.ToInt32(sqrt), Convert.ToInt32(sqrt), Convert.ToInt32(sqrt)));
                }
            }

            ////////////////////////////////PASO 3//////////////////////////////////////////////////
            ///
            Bitmap bm3 = new Bitmap(bm2.Width, bm2.Height);

            for (int x = 1; x < bm2.Width - 1; x++)
            {
                for (int y = 1; y < bm2.Height - 1; y++)
                {
                    Color colorTemp = bm2.GetPixel(x, y);
                    int avr = (colorTemp.R + colorTemp.G + colorTemp.B) / 3;
                    if (avr < 170)
                        avr = 0;
                    else if (avr >= 170)
                        avr = 255;

                    bm3.SetPixel(x, y, Color.FromArgb(avr, avr, avr));
                }
            }

            fotoEditada = bm3;
        }

        private void Convolucion4()
        {
            //escalaDeGrises(fotoEscalaGrises);

            Bitmap bm1 = new Bitmap(frameActual.Width, frameActual.Height);

            for (int x = 1; x < frameActual.Width - 1; x++)
            {
                for (int y = 1; y < frameActual.Height - 1; y++)
                {
                    Color color1, color2, color3, color4, color5, color6, color7, color8, color9;


                    color1 = fotoOriginal.GetPixel(x - 1, y - 1);
                    color2 = fotoOriginal.GetPixel(x, y - 1);
                    color3 = fotoOriginal.GetPixel(x + 1, y - 1);
                    color4 = fotoOriginal.GetPixel(x - 1, y);
                    color5 = fotoOriginal.GetPixel(x, y);
                    color6 = fotoOriginal.GetPixel(x + 1, y);
                    color7 = fotoOriginal.GetPixel(x - 1, y + 1);
                    color8 = fotoOriginal.GetPixel(x, y + 1);
                    color9 = fotoOriginal.GetPixel(x + 1, y + 1);

                    int colorRed = color1.R * conv3x3[0, 0]
                                    + color2.R * conv3x3[1, 0]
                                    + color3.R * conv3x3[2, 0]
                                    + color4.R * conv3x3[0, 1]
                                    + color5.R * conv3x3[1, 1]
                                    + color6.R * conv3x3[2, 1]
                                    + color7.R * conv3x3[0, 2]
                                    + color8.R * conv3x3[1, 2]
                                    + color9.R * conv3x3[2, 2];

                    colorRed = colorRed / factor + offset;


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

                    colorGreen = colorGreen / factor + offset;

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

                    colorBlue = colorBlue / factor + offset;

                    if (colorBlue < 0)
                        colorBlue = 0;
                    else if (colorBlue > 255)
                        colorBlue = 255;

                    bm1.SetPixel(x, y, Color.FromArgb(colorRed, colorGreen, colorBlue));
                }
            }

            fotoEditada = bm1;
        }

        private void Umbral2()
        {
            Bitmap bmtemp = new Bitmap(fotoEditada.Width, fotoEditada.Height);
            bmtemp = fotoEditada;
            Bitmap bm6 = new Bitmap(bmtemp.Width, bmtemp.Height);

            for (int x = 1; x < bmtemp.Width - 1; x++)
            {
                for (int y = 1; y < bmtemp.Height - 1; y++)
                {
                    Color colorTemp = bmtemp.GetPixel(x, y);
                    int avr = (colorTemp.R + colorTemp.G + colorTemp.B) / 3;
                    if (avr <= 80)
                        //avr = avr + ((127 - avr) * 2);
                        avr = 255;
                    else if (avr > 80)
                        //avr = avr + ((128 - avr) * 2);
                        avr = 0;

                    bm6.SetPixel(x, y, Color.FromArgb(avr, avr, avr));
                }
            }

            fotoEditada = bm6;
        }
    }
}
