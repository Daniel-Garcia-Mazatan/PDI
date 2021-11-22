using Emgu.CV;
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
    
    public partial class Form3 : Form
    {   
        Bitmap fotoOriginal, fotoEditada, fotoEscalaGrises, fotoTemp;
        bool existeFiltro;
        private int[,] conv3x3 = new int[3,3];
        private int factor;
        private int offset;
        private int[,] newImage;


        public Form3(Bitmap fotoTempColor)
        {
            InitializeComponent();
            fotoOriginal = fotoTempColor;
            fotoEscalaGrises = fotoOriginal;
            fotoEditada = fotoOriginal;
            //existeFiltro = false;
            escalaDeGrises(fotoOriginal);
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = fotoOriginal;
            pictureBox2.Image = fotoOriginal;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /////////////////////////NORTE-SUR///////////////////////////////
            conv3x3 = new int[,] {{1,1,1},
                                   {1,-2,1},
                                   {-1,-1,-1}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();


            pictureBox2.Image = fotoEditada;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ///////////////////Y DE SOBEL///////////////////////////
            conv3x3 = new int[,] {{-1,-2,-1},
                                   {0,0,0},
                                   {1,2,1}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ////////////////////////LAPLACIANO//////////////////////////////////
            conv3x3 = new int[,] {{0,1,0},
                                   {1,-4,1},
                                   {0,-1,0}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ////////////////////BORDES DE CANNY///////////////////////
            conv3x3 = new int[,] {{1,1,1},
                                   {1,-2,1},
                                   {-1,-1,-1}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /////////////////MENOS LAPLACIANO///////////////////////
            conv3x3 = new int[,] {{0,-1,0},
                                   {-1,5,-1},
                                   {0,-1,0}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            conv3x3 = new int[,] {{1,1,1},
                                   {1,1,1},
                                   {1,1,1}};

            factor = 9;
            offset = 0;

            Convolucion();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fotoEditada = fotoOriginal;
            pictureBox2.Image = fotoOriginal;
        }
        
        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void escalaDeGrises(Bitmap fotoColor)
        {
            Color imgPixel;
            int Red, Green, Blue;
            int pixelGris;

            for (int i = 0; i < fotoColor.Width; i++)
            {
                for(int j = 0; j < fotoColor.Height; j++)
                {
                    imgPixel = fotoColor.GetPixel(i, j);

                    Red = imgPixel.R;
                    Green = imgPixel.G;
                    Blue = imgPixel.B;

                    pixelGris = (Red + Green + Blue) / 3;

                    if (pixelGris > 255)
                        pixelGris = 255;
                    else if(pixelGris < 0)
                        pixelGris = 0;

                    fotoEscalaGrises.SetPixel(i, j, Color.FromArgb(pixelGris, pixelGris, pixelGris));
                }
            }
        }

        private void adicion(Bitmap imagen1, Bitmap imagen2)
        {
            Color imgPixel1, imgPixel2;
            int Red1, Green1, Blue1, Red2, Green2, Blue2;
            int pixelGris1, pixelGris2, pixelResultante;

            for (int i = 0; i < imagen1.Width; i++)
            {
                for (int j = 0; j < imagen1.Height; j++)
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

                    fotoEditada.SetPixel(i, j, Color.FromArgb(pixelResultante, pixelResultante, pixelResultante));
                }
            }
        }

       private void Convolucion()
        {
            Color oColor;
            int sumaR, sumaG, sumaB;

            for(int x = 1; x < fotoOriginal.Width - 1; x++)
            {
                for(int y = 1; y < fotoOriginal.Height - 1; y++)
                {
                    sumaR = 0;
                    sumaG = 0;
                    sumaB = 0;

                    for(int i = -1; i < 2; i++)
                    {
                        for(int j = -1; j < 2; j++)
                        {
                            oColor = fotoOriginal.GetPixel(x + i, y + j);

                            sumaR += (oColor.R * conv3x3[i + 1, j + 1]);
                            sumaG += (oColor.G * conv3x3[i + 1, j + 1]);
                            sumaB += (oColor.B * conv3x3[i + 1, j + 1]);
                        }
                    }

                    sumaR = (sumaR / factor) + offset;
                    sumaG = (sumaG / factor) + offset;
                    sumaB = (sumaB / factor) + offset;

                    if (sumaR < 0)
                        sumaR = 0;
                    else if (sumaR > 255)
                        sumaR = 255;

                    if (sumaG < 0)
                        sumaG = 0;
                    else if (sumaG > 255)
                        sumaG = 255;

                    if (sumaB < 0)
                        sumaB = 0;
                    else if (sumaB > 255)
                        sumaB = 255;

                    fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));

                    /*if (existeFiltro == false)
                    {
                        fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                        existeFiltro = true;
                    }
                    else
                    {
                        fotoTemp = fotoEditada;
                        fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                        adicion(fotoTemp, fotoEditada);
                    }
                        */
                }
            }
        }

        private void Convolucion2()
        {
            Color oColor2;
            int sumaR, sumaG, sumaB, suma;

            for (int x = 1; x < fotoEscalaGrises.Width - 1; x++)
            {
                for (int y = 1; y < fotoEscalaGrises.Height - 1; y++)
                {
                    sumaR = 0;
                    sumaG = 0;
                    sumaB = 0;
                    suma = 0;

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            oColor2 = fotoEscalaGrises.GetPixel(x + i, y + j);
                            
                            sumaR += (oColor2.R * conv3x3[i + 1, j + 1]);
                            sumaG += (oColor2.G * conv3x3[i + 1, j + 1]);
                            sumaB += (oColor2.B * conv3x3[i + 1, j + 1]);
                        }
                    }

                    sumaR = (sumaR / factor) + offset;
                    sumaG = (sumaG / factor) + offset;
                    sumaB = (sumaB / factor) + offset;

                    if (sumaR < 0)
                        sumaR = 0;
                    else if (sumaR > 255)
                        sumaR = 255;

                    if (sumaG < 0)
                        sumaG = 0;
                    else if (sumaG > 255)
                        sumaG = 255;

                    if (sumaB < 0)
                        sumaB = 0;
                    else if (sumaB > 255)
                        sumaB = 255;


                    fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                    //fotoEditada.SetPixel(x, y, Color.Gray);
                    /*if (existeFiltro == false)
                    {
                        fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                        existeFiltro = true;
                    }
                    else
                    {
                        fotoTemp = fotoEditada;
                        fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                        adicion(fotoTemp, fotoEditada);
                    }*/
                }
            }
        }
    }
}
