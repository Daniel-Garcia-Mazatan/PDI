using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filtromania
{
    
    public partial class Form3 : Form
    {   
        Bitmap fotoOriginal, fotoEditada, fotoEscalaGrises, fotoTemp;
        bool existeFiltro = false;
        private int[,] conv3x3 = new int[3,3];
        private int[,] conv5x5 = new int[5, 5];
        private int factor;
        private int offset;


        public Form3(Bitmap fotoTempColor)
        {
            InitializeComponent();
            fotoOriginal = fotoTempColor;
            fotoEscalaGrises = fotoOriginal;
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = fotoOriginal;
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /////////////////////////NORTE-SUR///////////////////////////////
            conv3x3 = new int[,] {{1, 1, 1},
                                  {1, -2, 1},
                                  {-1, -1, -1}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();


            pictureBox2.Image = fotoEditada;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ///////////////////Y DE SOBEL///////////////////////////
            conv3x3 = new int[,] {{-1, -2, -1},
                                  {0, 0, 0},
                                  {1, 2, 1}};

            factor = 1;
            offset = 0;

            Convolucion2();

            this.Invalidate();

            //fotoEditada = Convolucion3(fotoEscalaGrises, conv3x3, factor, offset, true);

            pictureBox2.Image = fotoEditada;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ////////////////////////LAPLACIANO//////////////////////////////////
            conv3x3 = new int[,] {{0, 1, 0},
                                  {1, -4, 1},
                                  {0, -1, 0}};

            factor = 3;
            offset = 0;

            Convolucion2();

            //fotoEditada = Convolucion3(fotoEscalaGrises, Laplaciano3x3, factor, offset, false);
            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ////////////////////BORDES DE CANNY///////////////////////
            conv5x5 = new int[,] {{2, 4, 5, 4, 2},
                                  {4, 9, 12, 9, 4},
                                  {5, 12, 15, 12, 5},
                                  {4, 9, 12, 9, 4},
                                  {2, 4, 5, 4, 2} };

            factor = 1;
            offset = 0;

            Convolucion3();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /////////////////MENOS LAPLACIANO///////////////////////
            conv3x3 = new int[,] {{0, -1, 0},
                                  {-1, 5, -1},
                                  {0, -1, 0}};

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

            //Convolucion();

            this.Invalidate();

            pictureBox2.Image = fotoEditada;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fotoEditada = fotoEscalaGrises;
            existeFiltro = false;
            pictureBox2.Image = null;
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
            if (pictureBox2.Image != null)
            {
               
                SaveFileDialog guarda = new SaveFileDialog();
                guarda.Filter = "Imagen jpg|*.jpg";
                guarda.ShowDialog();
                if(guarda.FileName != "")
                {
                    FileStream flujo = new FileStream(guarda.FileName, FileMode.Create, FileAccess.Write);
                    fotoEditada.Save(flujo, System.Drawing.Imaging.ImageFormat.Bmp);
                    flujo.Close();
                    fotoEditada.Dispose();
                    this.Close();
                }
            }
            else
                MessageBox.Show("No ha aplicado ningun filtro a la imagen/foto.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        /* private void Convolucion()
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

          //separado
                      if (existeFiltro == false)
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
              //separado   
                  }
              }
          }*/

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

        private void Convolucion3()
        {
            escalaDeGrises(fotoEscalaGrises);

            Bitmap bm1 = new Bitmap(fotoEscalaGrises.Width, fotoEscalaGrises.Height);

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

                    colorRed = (colorRed )  * factor + offset;


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

                    colorGreen = (colorGreen ) * factor + offset;

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

                    colorBlue = (colorBlue ) * factor + offset;

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
    }
}
