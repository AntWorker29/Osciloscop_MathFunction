using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lucrarea9_cs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        System.Drawing.Graphics desen;
        public osciloscop oscil_m;
        int pozx = 10, pozy = 10, n_maxx = 400, n_maxy = 300;
        double val_max = 1.2; //valoarea maxima
        double val_min = -1.2; //valoarea minima
        double x0 = -2, x1 = 2;
        double[] valori = new double[0];

        private void Form1_Load(object sender, EventArgs e)
        {
            Array.Resize(ref valori, n_maxx + 1);
            desen = this.CreateGraphics();
            oscil_m = new osciloscop(desen, pozx, pozy, n_maxx, n_maxy, val_max,val_min);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            double pas = (x1 - x0) / n_maxx;
            double x = x0;
            for (int i = 1; i <= n_maxx; i++) {
                x = x + pas;

                valori[i] = Math.Sin(x) * Math.Sin(160 * x);
            }
            oscil_m.sterg();
            oscil_m.auto_sx(x1, x0);
            oscil_m.setval(valori, n_maxx, Color.Red);
            oscil_m.display();
        }
        // --------------Clasa osciloscop ------------------
        public class osciloscop
        {
            int x0, y0, w, h, val, val_v;
            double v_max, v_min, x_max, x_min;
            System.Drawing.Graphics zona_des;
            System.Drawing.Pen creion_r = new System.Drawing.Pen(System.Drawing.Color.Red);
            System.Drawing.Font font_ni = new System.Drawing.Font("Nina", 8);
            System.Drawing.SolidBrush pens_blu = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            System.Drawing.SolidBrush radiera = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            System.Drawing.Bitmap img;
            System.Drawing.Bitmap ims;
            public void sterg() {
                img = new Bitmap(ims);
            }
            public void display() {
                zona_des.DrawImage(img, x0, y0);
            }
            public void auto_sx(double x_maxim, double x_minim)
            {
                if (x_max - x_min != 0) {
                    x_max = x_maxim;
                    x_min = x_minim;
                }
                else {
                    x_max = w;
                    x_min = 0;
                }
            }

            public void setval(double[] vals, int nrv, System.Drawing.Color cul)
            {
                int i, j;
                if (w > 0 && h > 0) {
                    // afisare grafic sub forma de puncte
                    double amplif;
                    if ((v_max - v_min) != 0)
                        amplif = (System.Convert.ToDouble(h) / System.Convert.ToDouble(v_max - v_min));
                    else
                        amplif = 1;
                    double transl = v_min * amplif;
                    val_v = System.Convert.ToInt16(h + transl - amplif * System.Convert.ToDouble(vals[0])); //scalare
                    if (val_v >= h)
                        val_v = h - 1;
                    if (val_v <= 0)
                        val_v = 1;
                    for (i = 0; i < w; i++) {
                        val = System.Convert.ToInt16(h + transl - amplif * System.Convert.ToDouble(vals[i])); //scalare
                        if (val >= h)
                            val = h - 1;
                        if (val <= 0)
                            val = 1;
                        if (val_v < val) {
                            // unesc doua puncte cu o linie crescatoare
                            for (j = val_v; j <= val; j++) {
                                img.SetPixel(i, j, cul);
                            }
                        }
                        else {
                            // unesc doua puncte cu o linie descrescatoare
                            for (j = val; j <= val_v; j++)
                            {
                                img.SetPixel(i, j, cul);
                            }
                        }
                        val_v = val;
                    }
                    Graphics g = Graphics.FromImage(img);
                    //valori axa x
                    double vx = x_min;
                    double pasx = System.Convert.ToDouble(x_max - x_min) / System.Convert.ToDouble(w) * 50;
                    for (i = 50; i < w; i += 50) {
                        vx = vx + pasx;
                        g.DrawString(Math.Round(vx, 2).ToString(), font_ni, pens_blu, i, h - 15);
                    }
                    //valori axa y
                    double vy = v_min;
                    double pasy = System.Convert.ToDouble(v_max - v_min) / System.Convert.ToDouble(h) * 50;
                    for (i = 50; i < h; i += 50) {
                        vy = vy + pasy;
                        g.DrawString(Math.Round(vy, 2).ToString(), font_ni, pens_blu, 2, h - i - 10);
                    }
                }
            }
            public osciloscop(System.Drawing.Graphics desen, int pozx, int pozy, int n_maxx, int n_maxy, double val_max, double val_min)
            {
                x0 = pozx;
                y0 = pozy;
                w = n_maxx;
                h = n_maxy;
                v_max = val_max;
                v_min = val_min;
                x_max = n_maxx;
                x_min = 0;
                zona_des = desen;
                if (w > 0 && h > 0)
                {
                    img = new Bitmap(w, h, zona_des);
                    ims = new Bitmap(w, h, zona_des);
                    int i, j;
                    // sterg imaginea
                    for (j = 0; j < h; j++)
                    {
                        for (i = 0; i < w; i++)
                        {
                            ims.SetPixel(i, j, System.Drawing.Color.WhiteSmoke);
                        }
                    }
                    // grid
                    for (j = 0; j < h; j++)
                    {
                        // grid orizontal
                        if (j % 10 == 0)
                        {
                            for (i = 0; i < w; i++)
                            {
                                if (j % 50 == 0)
                                    ims.SetPixel(i, j, System.Drawing.Color.Gray);
                                else
                                    ims.SetPixel(i, j, System.Drawing.Color.LightGray);
                            }
                        }
                        else
                        {
                            // grid orizontal vertical
                            for (i = 0; i < w; i++)
                            {
                                if (i % 10 == 0)
                                {
                                    if (i % 50 == 0)
                                        ims.SetPixel(i, j, System.Drawing.Color.Gray);
                                    else
                                        ims.SetPixel(i, j, System.Drawing.Color.LightGray);
                                }
                            }
                        }
                    }
                    //chenar
                    for (i = 0; i < w; i++)
                    {
                        ims.SetPixel(i, 0, System.Drawing.Color.Blue);
                        ims.SetPixel(i, h - 1, System.Drawing.Color.Blue);
                    }
                    for (j = 0; j < h; j++)
                    {
                        ims.SetPixel(0, j, System.Drawing.Color.Blue);
                        ims.SetPixel(w - 1, j, System.Drawing.Color.Blue);
                    }
                }
            }
        }
        // --------------Sfarsit clasa osciloscop ------------------

    }
}
