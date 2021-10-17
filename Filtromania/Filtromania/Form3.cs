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
        Image<Bgr, Byte> fotoOriginal, fotoFinal;
        Image<Gray, Byte> fotoTempGray;
        private int[,] conv3x3 = new int[3,3];
        private int factor;
        private int offset;
        Bitmap fotoOri, fotoOriGray, fotoEditada;
        public Form3(Image<Bgr, Byte> fotoTemp)
        {
            InitializeComponent();
            this.fotoOriginal = fotoTemp;
            fotoTempGray = fotoTemp.Convert<Gray, Byte>();
            fotoOri = fotoTemp.ToBitmap();
            fotoOriGray = fotoTempGray.ToBitmap();
            fotoEditada = fotoOriGray;
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            imageBox1.Image = fotoOriginal;
            
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
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

            fotoFinal = new Image<Bgr, Byte>(fotoEditada);
            imageBox2.Image = fotoFinal;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fotoEditada = fotoOriGray;
            fotoFinal = null;
            imageBox2.Image = null;
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
                            oColor = fotoOri.GetPixel(x + i, y + j);

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
                }
            }
        }

        private void Convolucion2()
        {
            Color oColor2;
            int sumaR, sumaG, sumaB;

            for (int x = 1; x < fotoTempGray.Width - 1; x++)
            {
                for (int y = 1; y < fotoTempGray.Height - 1; y++)
                {
                    sumaR = 0;
                    sumaG = 0;
                    sumaB = 0;

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            oColor2 = fotoOriGray.GetPixel(x + i, y + j);

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

                    //fotoEditada.SetPixel(x, y, Color.FromArgb(sumaR, sumaG, sumaB));
                    fotoEditada.SetPixel(x, y, Color.Gray);
                }
            }
        }
    }
}
